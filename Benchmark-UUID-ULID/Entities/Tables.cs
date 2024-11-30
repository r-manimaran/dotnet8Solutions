using System;

namespace Benchmark_UUID_ULID.Entities;

public class Table1_Int
{
    public int Id { get; set; }
}
public class Table2_Guid
{
    public Guid Id { get; set; }
}
public class Table3_Ulid
{
    public Ulid Id { get; set; }
}
public class Table4_UlidBinary
{
    public Ulid Id { get; set; }
}
public class Table5_DateTime
{
    public Guid Id { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
}
