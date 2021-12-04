using FristList.Dto.Queries.Import;
using FristList.Dto.Responses.Base;
using MediatR;

namespace FristList.WebApi.Requests.Import
{
    public class ImportDataRequest : IRequest<IResponse>
    {
        public ImportActionsQuery Query { get; init; }
        public string UserName { get; init; }
    }
}