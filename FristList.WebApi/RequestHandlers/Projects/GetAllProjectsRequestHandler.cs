using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FristList.Dto.Responses.Base;
using FristList.Models;
using FristList.Services;
using FristList.WebApi.Requests.Projects;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FristList.WebApi.RequestHandlers.Projects
{
    public class GetAllProjectsRequestHandler : IRequestHandler<GetAllProjectsRequest, IResponse>
    {
        private readonly IUserStore<AppUser> _userStore;
        private readonly IProjectRepository _projectRepository;

        public GetAllProjectsRequestHandler(IUserStore<AppUser> userStore, IProjectRepository projectRepository)
        {
            _userStore = userStore;
            _projectRepository = projectRepository;
        }

        public async Task<IResponse> Handle(GetAllProjectsRequest request, CancellationToken cancellationToken)
        {
            var query = request.Query;
            
            var user = await _userStore.FindByNameAsync(request.UserName, cancellationToken);
            var projectsCount = await _projectRepository.CountByUserAsync(user);
            var projects = _projectRepository
                .FindByUserAsync(user, (query.PageNumber - 1) * query.PageSize, query.PageSize)
                .Select(p => new Dto.Project
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .ToEnumerable();

            return PagedDataResponse<Dto.Project>.Create(projects, query.PageNumber, query.PageSize, projectsCount);
        }
    }
}