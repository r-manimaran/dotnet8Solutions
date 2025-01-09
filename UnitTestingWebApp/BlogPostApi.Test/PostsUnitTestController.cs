using BlogPostApi.Controllers;
using BlogPostApi.Dtos;
using BlogPostApi.Models;
using BlogPostApi.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public PostsUnitTestController()
    {
        _mockLogger = new Mock<ILogger<PostRepository>>();
        _mockRepository = new Mock<IPostRepository>();
        _controller = new PostsController(_mockRepository.Object, _mockLogger.Object);
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
        await _controller.Invoking(c=>c.CreatePost(nullPost))
            .Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData((string)null)]
    [InlineData("  ")]
    public async Task CreatePost_ShouldThrowExeption_WhenTitleIsInvalid(string invalidTitle)
    {
        // Arrange
        var post = new Post { Title = invalidTitle, Content = "Content" };

        // Act & Assert
        await _controller.Invoking(c => c.CreatePost(post))
            .Should().ThrowAsync<ValidationException>()
            .WithMessage("Title is required");
    }

}
