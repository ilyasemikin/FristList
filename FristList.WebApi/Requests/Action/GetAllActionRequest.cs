using System.Collections.Generic;
using FristList.Data.Queries;
using FristList.Data.Responses;
using MediatR;

namespace FristList.WebApi.Requests.Action;

public record GetAllActionRequest(int Page, int PageSize, string UserName) : IRequest<PagedDataResponse<Data.Dto.Action>>;