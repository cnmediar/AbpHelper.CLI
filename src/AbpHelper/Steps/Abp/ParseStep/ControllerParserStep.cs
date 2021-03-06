﻿using System.Collections.Generic;
using System.Linq;
using EasyAbp.AbpHelper.Extensions;
using EasyAbp.AbpHelper.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyAbp.AbpHelper.Steps.Abp.ParseStep
{
    public class ControllerParserStep : BaseParserStep<ClassDeclarationSyntax>
    {
        protected override string GetOutputVariableName()
        {
            return "ControllerInfo";
        }

        protected override IEnumerable<MethodInfo> GetMethodInfos(INamedTypeSymbol symbol)
        {
            return symbol
                    .GetBaseTypesAndThis()
                    .SelectMany(type => type.GetMembers())
                    .Where(type => type.Kind == SymbolKind.Method)
                    .Cast<IMethodSymbol>()
                    .Select(SymbolExtensions.ToMethodInfo)
                ;
        }
    }
}