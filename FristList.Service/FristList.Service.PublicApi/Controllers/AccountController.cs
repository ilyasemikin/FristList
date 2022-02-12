using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FristList.Service.PublicApi.Controllers;

[AllowAnonymous]
[Microsoft.AspNetCore.Components.Route("api/v1/account")]
public class AccountController : BaseController
{
    
}