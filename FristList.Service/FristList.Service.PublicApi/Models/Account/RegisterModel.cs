namespace FristList.Service.PublicApi.Models.Account;

public class RegisterModel
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}