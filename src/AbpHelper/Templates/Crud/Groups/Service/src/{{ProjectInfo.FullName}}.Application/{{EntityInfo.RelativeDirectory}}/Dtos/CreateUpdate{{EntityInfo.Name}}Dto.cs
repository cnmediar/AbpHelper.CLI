{{- SKIP_GENERATE = Option.SeparateDto -}}
using System;
{{~ if !Option.SkipLocalization }}using System.ComponentModel;{{ end ~}}
using Abp.Application.Services.Dto;
 

namespace {{ EntityInfo.Namespace }}.Dtos
{
    [Serializable]
    public class CreateUpdate{{ EntityInfo.Name }}Dto: {{ EntityInfo.BaseType | string.replace "AggregateRoot" "Entity"}}Dto{{ if EntityInfo.PrimaryKey }}<{{ EntityInfo.PrimaryKey}}>{{ end }}
    {
        {{~ for prop in EntityInfo.Properties ~}}
        {{~ if prop | abp.is_ignore_property; continue; end ~}}
        {{~ if !Option.SkipLocalization && Option.SkipViewModel ~}}
        [DisplayName("{{ EntityInfo.Name + prop.Name}}")]
        {{~ end ~}}
        public {{ prop.Type}} {{ prop.Name }} { get; set; }
        {{~ if !for.last ~}}

        {{~ end ~}}
        {{~ end ~}}
    }
}