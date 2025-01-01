namespace PolicyBasedAuthorization;

public class UserService
{
    public List<string> GetUserClaims(string userName)
    {
        return [AuthConstants.WebClaim, AuthConstants.MobileClaim];
    }
}
