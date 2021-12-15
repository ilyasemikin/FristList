namespace FristList.Data.Dto;

public class Tokens : DtoObjectBase
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    
    public Tokens()
    {
        AccessToken = RefreshToken = string.Empty;
    }
}