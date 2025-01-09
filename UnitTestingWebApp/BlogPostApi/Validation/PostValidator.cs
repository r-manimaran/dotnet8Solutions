using BlogPostApi.Models;
using FluentValidation;

namespace BlogPostApi.Validation
{
    public class PostValidator :AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Post cannot be null");

            When(x => x != null, () =>
            {
                RuleFor(p => p.Title)
                            .NotNull()
                            .NotEmpty()
                            .MaximumLength(100)
                            ;
            });
        }
    }
}
