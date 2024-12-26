namespace Validation.Api;

public class GitHubService
{
    private readonly HttpClient httpClient;

    public GitHubService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    //Other Methods goes here
}
