namespace HRMS.Application.Interfaces.Services.Dtos;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserInfoDto UserInfo { get; set; }
}