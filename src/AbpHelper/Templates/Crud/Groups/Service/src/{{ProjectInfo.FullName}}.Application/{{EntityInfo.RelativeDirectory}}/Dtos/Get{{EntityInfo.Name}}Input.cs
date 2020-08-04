using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;

namespace {{ EntityInfo.Namespace }}.Dtos
{
 
    [Serializable]
    public class Get{{EntityInfo.Name }}Input  :PagedAndSortedResultRequestDto, IShouldNormalize   
    {
        {{~ for prop in EntityInfo.GetinProperties ~}}
        {{~ if prop | abp.is_ignore_property; continue; end ~}}
        public {{ prop.Type}} {{ prop.Name }} { get; set; }
        {{~ if !for.last ~}}

        {{~ end ~}}
        {{~ end ~}}

 public void Normalize()
    {
        if (string.IsNullOrEmpty(Sorting))
        {
     
        {{ if EntityInfo.PrimaryKey }}
            Sorting = "{{ EntityInfo.PrimaryKey}}";
            {{ end }}

        }
    }
    }
  
}