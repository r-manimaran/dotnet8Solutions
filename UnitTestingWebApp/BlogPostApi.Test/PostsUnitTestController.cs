using BlogPostApi.Controllers;
using BlogPostApi.Dtos;
using BlogPostApi.Models;
using BlogPostApi.Repositories;
using BlogPostApi.Validation;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPostApi.Test;

public class PostsUnitTestController
{
    private readonly Mock<IPostRepository> _mockRepository;
    private readonly Mock<ILogger<PostRepository>> _mockLogger;
    private readonly PostsController _controller;
    private readonly IValidator<Post> _validator;
    public PostsUnitTestController()
    {
        _mockLogger = new Mock<ILogger<PostRepository>>();
        _mockRepository = new Mock<IPostRepository>();
        _validator = new PostValidator(); //  use the actual Fluent Validator
        _controller = new PostsController(_mockRepository.Object, _mockLogger.Object, _validator);
    }

    [Fact]
    public async Task GetPosts_ShouldReturn200OK_WithListOfPosts()
    {
        // Arrange
        var expectedPosts = new List<PostResponse>
        {
            new PostResponse
            {
              Id = Guid.NewGuid(),
              Title="Test Title",
              Content = "Sample Content 1",
              CreatedOn = DateTime.Now,
              Category ="Test Category"
            },
            new PostResponse
            {
              Id = Guid.NewGuid(),
              Title="Test Post 2 ",
              Content = "Sample Content 2",
              CreatedOn = DateTime.Now,
              Category ="Test Category"
            }
        };

        _mockRepository.Setup(repo=> repo.GetAllAsync())
                    .ReturnsAsync(expectedPosts);

        // Act
        var result = await _controller.GetPosts();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<PostResponse>>().Subject;
        posts.Should().BeEquivalentTo(expectedPosts);
    }

    [Fact]
    public async Task GetPostById_ShouldReturn200Ok_WhenPostsExists()
    {
        // Arrange
        var postId = Guid.NewGuid();
        var expectedPost = new PostResponse
        {
            Id = postId,
            Title = "Test Post",
            Content = "Test Content",
            CreatedOn = DateTime.Now,
            Category = "Test Category"
        };

        _mockRepository.Setup(repo => repo.GetByIdAsync(postId))
            .ReturnsAsync(expectedPost);

        // Act
        var result = await _controller.GetPostById(postId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var post = okResult.Value.Should().BeOfType<PostResponse>().Subject;
        post.Should().BeEquivalentTo(expectedPost);

    }

    [Fact]
    public async Task GetPostById_ShouldThrowException_WhenPostNotFound()
    {
        // Arrange
        var postId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetByIdAsync(postId))
              .Throws(new Exception("Post not found"));

        // Act & Assert
        await _controller.Invoking(c => c.GetPostById(postId))
              .Should().ThrowAsync<Exception>()
              .WithMessage("Post not found");
    }

    [Fact]
    public async Task CreatePost_ShouldReturn200Ok_WithCreatedPost()
    {
        // Arrange
        var newPost = new Post
        {
            Title = "New Post",
            Content = " New Content",
            CreatedDate = DateTime.Now,
            CategoryId = 1
        };

        var expectedResponse = new PostResponse
        {
            Id = Guid.NewGuid(),
            Title = newPost.Title,
            Content = newPost.Content,
            CreatedOn = DateTime.Now,
            Category = "Test Category"
        };

        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<Post>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.CreatePost(newPost);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var createdPost = okResult.Value.Should().BeOfType<PostResponse>().Subject;
        createdPost.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task UpdatePost_ShouldReturn200Ok_WithUpdatedPost()
    {
        // Arrange
        var upatePost = new Post
        {
            Id = Guid.NewGuid(),
            Title = "Updated Post",
            Content = "Updated Content",
            CreatedDate = DateTime.Now,
            CategoryId = 1
        };

        var expectedResponse = new PostResponse
        {
            Id = upatePost.Id,
            Title = upatePost.Title,
            Content = upatePost.Content,
            CreatedOn = upatePost.CreatedDate,
            Category = "Test Category"
        };

        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Post>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.UpdatePost(upatePost);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var updatedPost = okResult.Value.Should().BeOfType<PostResponse>().Subject;
        updatedPost.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task DeletePost_ShouldReturn200Ok_WhenPostDelete()
    {
        // Arrange
        var postId = Guid.NewGuid(); 
        _mockRepository.Setup(repo=>repo.DeleteAsync(postId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePost(postId);

        // Assert
        result.Should().BeOfType<OkResult>();
        _mockRepository.Verify(repo => repo.DeleteAsync(postId), Times.Once);
    }

    [Fact]
    public async Task CreatePost_ShouldThrowException_WhenPostIsNull()
    {
        // Arrange
        Post nullPost = null;

        //Act & Assert
        await FluentActions.Invoking(() => _controller.CreatePost(nullPost))
            .Should().ThrowAsync<ArgumentNullException>();
    }


    [Fact]
    public async Task PostExists_ShouldReturnTrue_WhenPostExists()
    {
        //Arrange
        var postId = Guid.NewGuid();
        _mockRepository.Setup(repo=>repo.ExistsAsync(postId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.PostExists(postId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(true);
    }

    [Fact]
    public async Task GetRecentPosts_ShouldReturnSpecificNumberOfPosts()
    {
        // Arrange
        var count = 2;
        var expectedPosts = new List<PostResponse>
        {
            new PostResponse { Id = Guid.NewGuid(), Title = "Recent Post 1" },
            new PostResponse { Id = Guid.NewGuid(), Title = "Recent Post 2" }
        };

        _mockRepository.Setup(repo=>repo.GetRecentPostsAsync(count))
            .ReturnsAsync(expectedPosts);

        // Act
        var result = await _controller.GetRecentPosts(count);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<PostResponse>>().Subject;
        posts.Should().BeEquivalentTo(expectedPosts);
    }

    [Fact]
    public async Task GetStatistics_ShouldReturnPostStatistics()
    {
        // Arrange
        var expectedStats = new PostStatistics
        {
            TotalPosts = 10,
            TotalCategories = 3,
            MostRecentPost = DateTime.UtcNow,
            PostsPerCategory = new Dictionary<string, int>
            {
                { "Category1", 5 },
                { "Category2", 3 },
                { "Category3", 2 }
            }
        };

        _mockRepository.Setup(repo => repo.GetPostStatisticsAsync())
            .ReturnsAsync(expectedStats);

        // Act
        var result = await _controller.GetStatistics();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var stats = okResult.Value.Should().BeOfType<PostStatistics>().Subject;
        stats.Should().BeEquivalentTo(expectedStats);
    }

    [Fact]
    public async Task SearchPosts_ShouldReturnMatchingPosts()
    {
        // Arrange
        var searchTerm = "test";
        var expectedPosts = new List<PostResponse>
        {
            new PostResponse { Id = Guid.NewGuid(), Title = "Test Post", Content = "Test Content" }
        };

        _mockRepository.Setup(repo => repo.SearchPostsAsync(searchTerm))
            .ReturnsAsync(expectedPosts);

        // Act
        var result = await _controller.SearchPosts(searchTerm);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<PostResponse>>().Subject;
        posts.Should().BeEquivalentTo(expectedPosts);
    }
    [Fact]
    public async Task GetPostsByCategory_ShouldReturnPosts_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var expectedPosts = new List<PostResponse>
        {
            new PostResponse { Id = Guid.NewGuid(), Title = "Test Post 1", Category = "Test Category" },
            new PostResponse { Id = Guid.NewGuid(), Title = "Test Post 2", Category = "Test Category" }
        };

        _mockRepository.Setup(repo => repo.GetPostsByCategoryAsync(categoryId))
            .ReturnsAsync(expectedPosts);

        // Act
        var result = await _controller.GetPostsByCategory(categoryId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var posts = okResult.Value.Should().BeAssignableTo<IEnumerable<PostResponse>>().Subject;
        posts.Should().BeEquivalentTo(expectedPosts);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]   
    [InlineData("  ")]
    public async Task CreatePost_ShouldThrowExeption_WhenTitleIsInvalid(string invalidTitle)
    {
        // Arrange
        var invalidPost = new Post { Title = invalidTitle, Content = "Content", CreatedDate = DateTime.Now };

        // Act & Assert
        var validationException = await Assert.ThrowsAsync<ValidationException>(
            () => _controller.CreatePost(invalidPost));

        // Assert the specific error message
        validationException.Errors.Should().Contain(e =>
            e.ErrorMessage == "Title is required" &&
            e.PropertyName == "Title");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task CreatePost_ShouldThrowException_WhenContentIsInvalid(string invalidContent)
    {
        var invalidPost = new Post { Title = "Test", Content = invalidContent, CreatedDate = DateTime.Now };

        var validationException = await Assert.ThrowsAsync<ValidationException>(
            () => _controller.CreatePost(invalidPost));

        validationException.Errors.Should().Contain(e=>
            e.ErrorMessage == "Content is required" && 
            e.PropertyName == "Content");

    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetRecentPosts_ShouldThrowException_WhenCountIsInvalid(int invalidCount)
    {
        _mockRepository.Setup(repo => repo.GetRecentPostsAsync(invalidCount))
             .ThrowsAsync(new ArgumentException("Count must be greater than Zero"));

        // Act & Assert
        await _controller.Invoking(c => c.GetRecentPosts(invalidCount))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Count must be greater than Zero");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task SearchPosts_ShouldThrowException_WhenSearchTermIsInvalid(string searchTerm)
    {
        // Arrange
        _mockRepository.Setup(repo => repo.SearchPostsAsync(searchTerm))
            .ThrowsAsync(new ArgumentException("Search term cannot be empty"));

        // Act & Assert
        await _controller.Invoking(c => c.SearchPosts(searchTerm))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Search term cannot be empty");
    }
}
