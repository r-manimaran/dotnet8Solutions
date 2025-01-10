namespace BlogPostApi.Dtos;

public class PostStatistics
{
    public int TotalPosts { get; set; }
    public int TotalCategories { get; set; }
    public DateTime MostRecentPost { get; set; }
    public Dictionary<string, int> PostsPerCategory { get; set; }
}
