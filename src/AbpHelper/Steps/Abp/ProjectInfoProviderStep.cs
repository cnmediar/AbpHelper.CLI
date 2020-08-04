﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.AbpHelper.Models;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Scripting.JavaScript;
using Elsa.Services.Models;

namespace EasyAbp.AbpHelper.Steps.Abp
{
    public class ProjectInfoProviderStep : Step
    {
        public WorkflowExpression<string> BaseDirectory
        {
            get => GetState(() => new JavaScriptExpression<string>("BaseDirectory"));
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var baseDirectory = await context.EvaluateAsync(BaseDirectory, cancellationToken);
            LogInput(() => baseDirectory);

            TemplateType templateType;
            if (Directory.EnumerateFiles(baseDirectory, "*.Migrator.csproj", SearchOption.AllDirectories).Any())
                templateType = TemplateType.Application;
            else if (Directory.EnumerateFiles(baseDirectory, "*.Host.Shared.csproj", SearchOption.AllDirectories).Any())
                templateType = TemplateType.Module;
            else
                throw new NotSupportedException($"Unknown ABP project structure. Directory: {baseDirectory}");

            // Assume the domain project must be existed for an ABP project
            var domainCsprojFile = Directory.EnumerateFiles(baseDirectory, "*.Core.csproj", SearchOption.AllDirectories).FirstOrDefault();
            if (domainCsprojFile == null) throw new NotSupportedException($"Cannot find the domain project file. Make sure it is a valid ABP project. Directory: {baseDirectory}");

            var fileName = Path.GetFileName(domainCsprojFile);
            var fullName = fileName.RemovePostFix(".Core.csproj");

            UiFramework uiFramework;
            if (Directory.EnumerateFiles(baseDirectory, "*.cshtml", SearchOption.AllDirectories).Any())
            {
                uiFramework = UiFramework.RazorPages;
                if (templateType == TemplateType.Application)
                {
                    context.SetVariable("AspNetCoreDir", Path.Combine(baseDirectory, "aspnet-core"));
                }
                else
                {
                    context.SetVariable("AspNetCoreDir", baseDirectory);
                }
            }
            else if (Directory.EnumerateFiles(baseDirectory, "app.module.ts", SearchOption.AllDirectories).Any())
            {
                uiFramework = UiFramework.Angular;
                context.SetVariable("AspNetCoreDir", Path.Combine(baseDirectory, "aspnet-core"));
            }
            else
            {
                uiFramework = UiFramework.None;
                context.SetVariable("AspNetCoreDir", baseDirectory);
            }

            var tiered = false;
            if (templateType == TemplateType.Application) tiered = Directory.EnumerateFiles(baseDirectory, "*.IdentityServer.csproj", SearchOption.AllDirectories).Any();

            var projectInfo = new ProjectInfo(baseDirectory, fullName, templateType, uiFramework, tiered);
            context.SetLastResult(projectInfo);
            context.SetVariable("ProjectInfo", projectInfo);
            LogOutput(() => projectInfo);

            return Done();
        }
    }
}