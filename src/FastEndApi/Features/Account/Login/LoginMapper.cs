using FastEndApi.Data.Models;

namespace FastEndApi.Features.Account.Login;

public class LoginMapper : ResponseMapper<LoginResponse, User>
{
    public override LoginResponse FromEntity(User user)
    {
        return new()
        {
            UserName = user.UserName,
            ApiToken = user.Tokens.First().Value
        };
    }
}