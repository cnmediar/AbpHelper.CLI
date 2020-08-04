{{- if Option.SeparateDto
createDto = "Create" + EntityInfo.Name + "Dto"
updateDto = "Update" + EntityInfo.Name + "Dto"
else
createDto = "CreateUpdate" + EntityInfo.Name + "Dto"
updateDto = "CreateUpdate" + EntityInfo.Name + "Dto"
end
-}}
using System;
using {{ EntityInfo.Namespace }}.Dtos;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace {{ EntityInfo.Namespace }}
{
    public interface I{{ EntityInfo.Name }}AppService :
        IAsyncCrudAppService< 
            {{ EntityInfo.Name }}Dto, 
            {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }}, 
            PagedAndSortedResultRequestDto,
            {{ createDto }},
            {{ updateDto }}>
    {

    }
}