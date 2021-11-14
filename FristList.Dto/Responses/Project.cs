using FristList.Dto.Responses.Base;

namespace FristList.Dto.Responses
{
    public class Project : DtoObjectBase
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}