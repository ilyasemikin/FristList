namespace FristList.Service.PublicApi.Models.Account;

public class AuthorizeModel
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}