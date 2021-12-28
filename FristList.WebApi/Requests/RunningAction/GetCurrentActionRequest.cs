using FristList.Data.Responses;
using FristList.Models;
using MediatR;

namespace FristList.WebApi.Requests.RunningAction;

public record GetCurrentActionRequest(string UserName) : IRequest<Data.Dto.RunningAction?>;