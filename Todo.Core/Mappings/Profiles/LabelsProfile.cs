using System.Collections.Generic;
using AutoMapper;
using Todo.Core.MappingProfiles.Converters;
using Todo.Core.Models.Dtos;
using Todo.Core.Models.Sql;

namespace Todo.Core.Mappings.Profiles
{
    public class LabelsProfile : Profile
    {
        public LabelsProfile()
        {
            CreateMap<Label, LabelDto>();

            CreateMap<List<Label>, List<string>>();
            CreateMap<Label, string>().ConvertUsing<LabelToStringConverter>();
        }
    }
}
