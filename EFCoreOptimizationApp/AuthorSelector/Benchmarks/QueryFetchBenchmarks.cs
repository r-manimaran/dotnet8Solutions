using AuthorSelector.BenchmarkConfig;
using AuthorSelector.DBContext;
using AuthorSelector.DTOs;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector;

[MemoryDiagnoser(false)]
[HideColumns(BenchmarkDotNet.Columns.Column.Job,BenchmarkDotNet.Columns.Column.StdDev, BenchmarkDotNet.Columns.Column.AllocRatio)]
[Config(typeof(StyleConfig))]
public class QueryFetchBenchmarks
{
    public QueryFetchBenchmarks()
    {
        
    }
    /// <summary>
    /// Get 2 Authors (FirstName, LastName, UserName, Email,Age,Country)
    /// from Country Serbia aged 27, with the highest BooksCount
    /// and all his/her books (Book Name and Publisher Year) published before 1900
    /// </summary>
    /// <returns></returns>
    [Benchmark(Baseline = true)]
    public List<AuthorDto> GetAuthors()
    {
        using var dbContext = new AppDbContext();
        var authors = dbContext.Authors
                              .Include(x => x.User)
                              .ThenInclude(x => x.UserRoles)
                              .ThenInclude(x => x.Role)
                              .Include(x => x.Books)
                              .ThenInclude(x => x.Publisher)
                              .ToList()
                              .Select(x => new AuthorDto
                              {
                                  UserCreated = x.User.Created,
                                  UserEmailConfirmed = x.User.EmailConfirmed,
                                  UserFirstName = x.User.FirstName,
                                  UserLastName = x.User.LastName,
                                  UserEmail = x.User.Email,
                                  UserName = x.User.UserName,
                                  UserId = x.User.Id,
                                  RoleId = x.User.UserRoles.FirstOrDefault(y=>y.UserId  == x.UserId).RoleId,
                                  BooksCount = x.BooksCount,
                                  AllBooks = x.Books.Select(y=> new BookDto
                                  {
                                      Id = y.Id,
                                      Name = y.Name,
                                      Published = y.Published,
                                      ISBN = y.ISBN,
                                      PublisherName = y.Publisher.Name,
                                  }).ToList(),
                                  AuthorAge = x.Age,
                                  AuthorCountry = x.Country,
                                  AuthorNickName = x.NickName,
                                  Id = x.Id
                              })
                              .ToList()
                              .Where(x=>x.AuthorCountry == "Serbia" && x.AuthorAge ==27)
                              .ToList();

        var orderedAuthors = authors.OrderByDescending(x=>x.BooksCount).ToList().Take(2).ToList();

        List<AuthorDto> finalAuthors = new List<AuthorDto>();
        foreach(var author in orderedAuthors)
        {
            List<BookDto> books = new List<BookDto>();
            var allBooks = author.AllBooks;

            foreach (var book in allBooks) 
            { 
                if(book.Published.Year < 1900)
                {
                    book.PublishedYear = book.Published.Year;
                    books.Add(book);
                }
            }
            author.AllBooks = books;
            finalAuthors.Add(author);
        }
        return finalAuthors;
    }

   // [Benchmark]
    public List<AuthorDto> GetAuthorsOptimized()
    {
        using var dbContext = new AppDbContext();
        var authors = dbContext.Authors
                              //.AsNoTracking()
                              /*.Include(x => x.User)
                              .ThenInclude(x => x.UserRoles)
                              .ThenInclude(x => x.Role)
                              .Include(x => x.Books)
                              .ThenInclude(x => x.Publisher)*/
                              //.ToList() Step 1:
                              .Include(x=>x.Books.Where(b=>b.Published.Year <1900))
                              .OrderByDescending(x => x.BooksCount)
                              .Where(x => x.Country == "Serbia" && x.Age == 27)
                              .Select(x => new AuthorDto
                              {
                                  // UserCreated = x.User.Created,
                                  // UserEmailConfirmed = x.User.EmailConfirmed,
                                  UserFirstName = x.User.FirstName,
                                  UserLastName = x.User.LastName,
                                  UserEmail = x.User.Email,
                                  UserName = x.User.UserName,
                                  AuthorAge = x.Age,
                                  AuthorCountry = x.Country,
                                  // UserId = x.User.Id,
                                  // RoleId = x.User.UserRoles.FirstOrDefault(y => y.UserId == x.UserId).RoleId,
                                 // BooksCount = x.BooksCount,
                                  AllBooks = x.Books
                                        //.Where(x=>x.Published.Year < 1900)  // Step 4: Moved where condition Query here
                                        .Select(y => new BookDto
                                              {
                                                  Id = y.Id,
                                                  Name = y.Name,
                                                  Published = y.Published,
                                                  //ISBN = y.ISBN,
                                                 // PublisherName = y.Publisher.Name,
                                              })
                                            .ToList(),                                  
                                  //AuthorNickName = x.NickName,
                                  Id = x.Id
                              })
                              //.ToList() // Step 2:
                              //.Where(x => x.AuthorCountry == "Serbia" && x.AuthorAge == 27)
                              //.OrderByDescending(x => x.BooksCount)
                              .Take(2)
                              .ToList();
        // Step 3:
        //var orderedAuthors = authors
        //                      .OrderByDescending(x => x.BooksCount)
        //                      .ToList()
        //                      .Take(2)
        //                      .ToList();

        // Commented for Step 4:
        /*List<AuthorDto> finalAuthors = new List<AuthorDto>();
        foreach (var author in authors)
        {
            List<BookDto> books = new List<BookDto>();
            var allBooks = author.AllBooks;

            foreach (var book in allBooks)
            {
                if (book.Published.Year < 1900)
                {
                    book.PublishedYear = book.Published.Year;
                    books.Add(book);
                }
            }
            author.AllBooks = books;
            finalAuthors.Add(author);
        }
        return finalAuthors;*/

        return authors;
    }

    [Benchmark]
    public List<AuthorDto> GetAuthors_Optimized()
    {
        using var dbContext = new AppDbContext();
        var authors = dbContext.Authors
                              .Include(x => x.Books.Where(b => b.Published.Year < 1900))
                              .OrderByDescending(x => x.BooksCount)
                              .Where(x => x.Country == "Serbia" && x.Age == 27)
                              .Select(x => new AuthorDto
                              {                              
                                  UserFirstName = x.User.FirstName,
                                  UserLastName = x.User.LastName,
                                  UserEmail = x.User.Email,
                                  UserName = x.User.UserName,
                                  AuthorAge = x.Age,
                                  AuthorCountry = x.Country,
                                  AllBooks = x.Books
                                        .Select(y => new BookDto
                                        {
                                            Id = y.Id,
                                            Name = y.Name,
                                            Published = y.Published,                                 
                                        })
                                            .ToList(),
                                  Id = x.Id
                              })
                              .Take(2)
                              .ToList();       

        return authors;
    }

    [Benchmark]
    public List<AuthorDto> GetAuthors_Optimized_Compiled()
    {
        using var dbContext = new AppDbContext();
        var authors = CompileQuery(dbContext).ToList();
        foreach (var author in authors)
        {
            author.AllBooks = author.AllBooks
                        .Where(bookDto => bookDto.Published.Year < 1900)
                        .ToList();
        }

        return authors;
    }

    private static readonly Func<AppDbContext, IEnumerable<AuthorDto>> CompileQuery =
        EF.CompileQuery
        (
            (AppDbContext context) =>
              context.Authors
                     .Where(author =>
                       author.Country == "Serbia" && 
                       author.Age == 27)
            .OrderByDescending(author => author.BooksCount)
            .Select(x => new AuthorDto
            {
                UserFirstName = x.User.FirstName,
                UserLastName = x.User.LastName,
                UserEmail = x.User.Email,
                UserName = x.User.UserName,
                AuthorAge = x.Age,
                AuthorCountry = x.Country,
                AllBooks = x.Books
                           // .Where(book => book.Published.Year < 1900)
                            .Select(y => new BookDto
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Published = y.Published,
                            })
                                .ToList(),
                Id = x.Id
            })
            .Take(2)
        );

}
