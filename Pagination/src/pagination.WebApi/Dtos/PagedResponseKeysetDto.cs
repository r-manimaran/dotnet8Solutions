using System;

namespace pagination.WebApi.Dtos;

public class PagedResponseKeysetDto<T>
{
    public int Reference {get; init;}

    public List<T> Data {get; init;}
}
