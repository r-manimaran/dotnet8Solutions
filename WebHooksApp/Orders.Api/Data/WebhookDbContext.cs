﻿using Microsoft.EntityFrameworkCore;
using Orders.Api.Models;

namespace Orders.Api.Data;

internal sealed class WebhookDbContext : DbContext
{
    public WebhookDbContext(DbContextOptions<WebhookDbContext> options) :base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }

    public DbSet<WebhookDeliveryAttempt> WebhookDeliveryAttempts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.Id);
        });

        modelBuilder.Entity<WebhookSubscription>(builder =>
        {
            builder.ToTable("subscriptions", "webhooks");
            builder.HasKey(o => o.Id);
        });

        modelBuilder.Entity<WebhookDeliveryAttempt>(builder =>
        {
            builder.ToTable("delivery_attempts", "webhooks");
            builder.HasKey(o => o.Id);

            // Foreign Key to subscription table
            builder.HasOne<WebhookSubscription>()
                .WithMany()
                .HasForeignKey(d => d.WebhookSubscriptionId);

        });
    }
        
}
