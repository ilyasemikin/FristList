using AutoMapper;
using FristList.Service.Data;
using FristList.Service.PublicApi.Controllers.Base;
using FristList.Service.PublicApi.Data.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Route("api/v1/user")]
public class UserController : BaseController
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public UserController(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    [HttpGet("{userName}")]
    [SwaggerResponse(Http200, Type = typeof(ApiUser))]
    [SwaggerResponse(Http400)]
    [SwaggerResponse(Http404)]
    public async Task<IActionResult> GetUserAsync([FromRoute] string userName)
    {
        if (string.IsNullOrEmpty(userName))
            return BadRequest();
        
        var user = await _appDbContext.Users.Where(u => u.NormalizedUserName == userName.ToUpper())
            .FirstOrDefaultAsync();
        if (user is null)
            return NotFound();

        var apiUser = _mapper.Map<ApiUser>(user);
        return Ok(apiUser);
    }
}