{{-
if Option.SeparateDto
    createDto = "Create" + EntityInfo.Name + "Dto"
    updateDto = "Update" + EntityInfo.Name + "Dto"
else
    createDto = "CreateUpdate" + EntityInfo.Name + "Dto"
    updateDto = "CreateUpdate" + EntityInfo.Name + "Dto"
end
if Option.CustomRepository 
    repositoryType = "IRepository<" + EntityInfo.Name + ">"
    repositoryName = "_repository"

else
    if EntityInfo.CompositeKeyName
        repositoryType = "IRepository<" + EntityInfo.Name + ">"
    else
        repositoryType = "IRepository<" + EntityInfo.Name + ", " + EntityInfo.PrimaryKey + ">"
    end
    repositoryName = "_" + EntityInfo.Name + "Repository"
end ~}}
using System;
{{~ if !EntityInfo.CompositeKeyName
    crudClassName = "AsyncCrudAppService"
else
    crudClassName = "AbstractKeyCrudAppService"
~}}
using System.Linq;
using System.Threading.Tasks;
{{~ end -}}
{{~ if !Option.SkipPermissions 
    permissionNamesPrefix = ProjectInfo.Name + "Permissions." + EntityInfo.Name
~}}
using {{ ProjectInfo.FullName }}.Permissions;
{{~ end ~}}
using {{ EntityInfo.Namespace }}.Dtos;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
{{~ if !Option.CustomRepository ~}}
using Abp.Domain.Repositories;
{{~ end ~}}

namespace {{ EntityInfo.Namespace }}
{
    public class {{ EntityInfo.Name }}AppService : {{ crudClassName }}<{{ EntityInfo.Name }}, {{ EntityInfo.Name }}Dto, {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }}, PagedAndSortedResultRequestDto, {{ createDto }}, {{ updateDto }}>,
        I{{ EntityInfo.Name }}AppService
    {
        {{~ if !Option.SkipPermissions ~}}
        protected override string GetPermissionName { get; set; } = {{ permissionNamesPrefix }}.Default;
        protected override string GetAllPermissionName { get; set; } = {{ permissionNamesPrefix }}.Default;
        protected override string CreatePermissionName { get; set; } = {{ permissionNamesPrefix }}.Create;
        protected override string UpdatePermissionName { get; set; } = {{ permissionNamesPrefix }}.Update;
        protected override string DeletePermissionName { get; set; } = {{ permissionNamesPrefix }}.Delete;
        {{~ end ~}}

        {{~ if !Option.CustomRepository ~}}
        private readonly {{ repositoryType }} {{ repositoryName }};
       

public {{ EntityInfo.Name }}AppService({{ repositoryType }} repository) : base(repository)
        {
            {{ repositoryName }} = repository;
        }
        {{~ else ~}}
        public {{ EntityInfo.Name }}AppService({{ repositoryType }} repository) : base(repository)
        {
        }
        {{~ end ~}}
        {{~ if EntityInfo.CompositeKeyName ~}}
        
        protected override Task DeleteByIdAsync({{ EntityInfo.CompositeKeyName }} id)
        {
            // TODO: AbpHelper generated
            return {{ repositoryName }}.DeleteAsync(e =>
            {{~ for prop in EntityInfo.CompositeKeys ~}}
                e.{{ prop.Name }} == id.{{ prop.Name}}{{ if !for.last}} &&{{end}}
            {{~ end ~}}
            );
        }

        protected override async Task<{{ EntityInfo.Name }}> GetEntityByIdAsync({{ EntityInfo.CompositeKeyName }} id)
        {
            // TODO: AbpHelper generated
            return await AsyncQueryableExecuter.FirstOrDefaultAsync(
                {{ repositoryName }}.Where(e =>
                {{~ for prop in EntityInfo.CompositeKeys ~}}
                    e.{{ prop.Name }} == id.{{ prop.Name}}{{ if !for.last}} &&{{end}}
                {{~ end ~}}
                )
            ); 
        }
       {{~ end ~}} 
    }
}
