using BlogPostApi.Dtos;
using BlogPostApi.Models;
using Mapster;

namespace BlogPostApi.Mapping
{
    public class PostMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Post, PostResponse>()
                .Map(dest => dest.CreatedOn, src => src.CreatedDate)
                .Map(dest => dest.Category, src => src.Category.Name)
                .TwoWays();
            // .Map(dest => dest.Category, src => src.Category != null ? src.Category.Name : "Uncategorized");
        }
    }
}
