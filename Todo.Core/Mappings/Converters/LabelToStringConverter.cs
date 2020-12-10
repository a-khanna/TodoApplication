using AutoMapper;
using Todo.Core.Models.Sql;

namespace Todo.Core.MappingProfiles.Converters
{
    /// <summary>
    /// Converter to extract name from label entity
    /// </summary>
    public class LabelToStringConverter : ITypeConverter<Label, string>
    {
        public string Convert(Label source, string destination, ResolutionContext context)
        {
            return source.Name;
        }
    }
}
