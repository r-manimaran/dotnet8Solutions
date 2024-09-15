using cartesionExplosion_api.Database;
using Microsoft.EntityFrameworkCore;
using cartesionExplosion_api.Extensions;
using cartesionExplosion_api.Endpoints;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

//with query splitting option in the database level
// builder.Services.AddDbContext<AppDbContext>(options =>
//                 options.UseNpgsql(builder.Configuration.GetConnectionString("Database"),
//                 o=>o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddEndpoints();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();

   //await app.SeedDatabase();

}

//Endpoints
app.MapGet("departments/{id}/teams",async(int id, AppDbContext context)=>{
    var departments = await context
                        .departments
                        .Include(d=>d.Teams)
                        .AsNoTracking()
                        .Where(d=>d.Id == id)
                        .ToListAsync();
        return departments;
});

app.MapGet("departments/{id}/teams/employees",async(int id, AppDbContext context)=>{
    var departments = await context
                        .departments
                        .Include(d=>d.Teams)
                        .ThenInclude(e=>e.Employees)
                        .AsNoTracking()
                        .Where(d=>d.Id == id)
                        .ToListAsync();
    return departments;
});

app.MapGet("departments/{id}/teams/employees/tasks",async(int id, AppDbContext context)=>{
    var departments = await context
                        .departments
                        .Include(d=>d.Teams)
                        .ThenInclude(e=>e.Employees)
                        .ThenInclude(t=>t.Tasks)
                        .AsNoTracking()
                        .Where(d=>d.Id == id)
                        .ToListAsync();
        return departments;
}); //This endpoints will give time-out for large data sets

app.MapGet("employees/{id}/tasks",async(int id, AppDbContext context)=>{
    var employees = await context
                        .employees                  
                        .Include(t=>t.Tasks)
                        .AsNoTracking()
                        .Where(e=>e.Id == id)
                        .ToListAsync();
     return employees;
});

app.MapGet("employees/{id}/salary",async(int id, AppDbContext context)=>{
    var employees = await context
                        .employees                  
                        .Include(t=>t.SalaryPayments)
                        .AsNoTracking()
                        .Where(e=>e.Id == id)
                        .ToListAsync();
    return employees;
});
app.MapGet("employees/{id}",async(int id, AppDbContext context)=>{
    var employees = await context
                        .employees 
                        .Include(t=>t.Tasks)                 
                        .Include(t=>t.SalaryPayments)
                        .AsNoTracking()
                        .Where(e=>e.Id == id)
                        .ToListAsync();
         return employees;
}); //cross product will happen with this queries

app.MapGet("employeesWithSplitQuery/{id}",async(int id, AppDbContext context)=>{
    var employees = await context
                        .employees 
                        .Include(t=>t.Tasks)                 
                        .Include(t=>t.SalaryPayments)
                        .AsNoTracking()
                        .AsSplitQuery() //This will solve cross product issue                      
                        .Where(e=>e.Id == id)
                        .ToListAsync();
         return employees;
}); 

app.UseHttpsRedirection();

app.Run();

