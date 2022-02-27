using FristList.Service.Data.Models.Account;

namespace FristList.Service.PublicApi.Services.Abstractions;

public interface ITokenGenerator
{
    string Generate(User user);
}