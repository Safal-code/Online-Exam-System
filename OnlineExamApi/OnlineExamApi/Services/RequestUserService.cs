namespace OnlineExamApi.Services;

public class RequestUserService
{
    private readonly CurrentUserService _currentUserService;

    public RequestUserService(CurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<Models.User?> GetCurrentUser(HttpRequest request) //entire incoming webrequest(url,header,body)
    {
        var userIdHeader = request.Headers["X-User-Id"].FirstOrDefault(); //look for a header named X-User-Id.

        if (!int.TryParse(userIdHeader, out int userId)) //if parsing success assign converted value to userId
        {
            return null;
        }

        return await _currentUserService.GetUser(userId);
    }
}

//whole job is "read the X-User-Id header,
//and if it's a real number pointing to a real user
//give me that user's full record
//now send that user id to currentUserService
