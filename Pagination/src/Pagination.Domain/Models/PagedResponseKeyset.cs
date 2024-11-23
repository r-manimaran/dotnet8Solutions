using System;

namespace Pagination.Domain.Models;

public record PagedResponseKeyset<T>
{
    public PagedResponseKeyset(List<T> data, int references)
    {
        Data = data;
        References = references;
    }

    public List<T> Data { get; init; }
    public int References { get; init; }
}
