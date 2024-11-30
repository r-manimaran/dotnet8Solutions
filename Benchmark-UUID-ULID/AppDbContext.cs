using System;
using Benchmark_UUID_ULID.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Benchmark_UUID_ULID;

public class AppDbContext : DbContext
{

    public const string ConnectionString = "Server=DARSHAN-PC\\SQLEXPRESS;Database=UUID_Benchmark;Trusted_Connection=True;TrustServerCertificate=true;";

    public DbSet<Table1_Int> Table1 { get; set; }
    public DbSet<Table2_Guid> Table2 { get; set; }

    public DbSet<Table3_Ulid> Table3 { get; set; }

    public DbSet<Table4_UlidBinary> Table4 { get; set; }

    public DbSet<Table5_DateTime> Table5 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table1_Int>(b => b.HasKey(x => x.Id));

        modelBuilder.Entity<Table2_Guid>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Table3_Ulid>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever().HasConversion<UlidToStringConverter>();
        });

        modelBuilder.Entity<Table4_UlidBinary>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever().HasConversion<UlidToBytesConverter>();
        });

        modelBuilder.Entity<Table5_DateTime>(b =>
        {
            b.HasKey(x => x.Id).IsClustered(false);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x=>x.CreatedOnUtc).HasDefaultValueSql("GetUTCDATE()");
            b.HasIndex(x=>x.CreatedOnUtc).IsClustered();
        });
    }
}

public class UlidToBytesConverter : ValueConverter<Ulid, byte[]>
{
    private static readonly ConverterMappingHints DefaultHints = new (size:16);
    public UlidToBytesConverter(): this(null)
    {
        
    }
    public UlidToBytesConverter(ConverterMappingHints mappingHints =null) : base(
        convertToProviderExpression: v => v.ToByteArray(),
        convertFromProviderExpression: v => new Ulid(v),
        mappingHints: DefaultHints.With(mappingHints))
    {
        
    }
}



public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints DefaultHints = new (size:26);
    public UlidToStringConverter() : this(null)
    {

    }
    public UlidToStringConverter(ConverterMappingHints? mappingHints =null) : base(
        convertToProviderExpression: v => v.ToString(),
        convertFromProviderExpression: v => Ulid.Parse(v),
        mappingHints: DefaultHints.With(mappingHints))
    {

    }
}
