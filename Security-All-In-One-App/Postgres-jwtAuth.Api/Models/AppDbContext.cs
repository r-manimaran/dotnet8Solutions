﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Postgres_jwtAuth.Api.Models;

public class AppDbContext: IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
    {
        
    }

    public DbSet<TokenInfo> TokenInfos { get; set; }
}
