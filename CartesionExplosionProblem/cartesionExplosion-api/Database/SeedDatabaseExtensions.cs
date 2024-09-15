using Bogus;
using cartesionExplosion_api.Entities;

namespace cartesionExplosion_api.Database
{
    public static class SeedDatabaseExtensions
    {
        public static async System.Threading.Tasks.Task SeedDB(this AppDbContext context)
        {
            Randomizer.Seed = new Random(5000);

            var departmentFakr = new Faker<Department>()
                .RuleFor(d => d.Name, f => f.Commerce.Department())
                .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                .RuleFor(d => d.Location, f => f.Address.City())
                .RuleFor(d => d.Budget, f => f.Finance.Amount(100000, 1000000))
                .RuleFor(d => d.EstablishedDate, f => f.Date.Past(10).ToUniversalTime())
                .RuleFor(d => d.HeadOfDepartment, f => f.Name.FullName())
                .RuleFor(d => d.IsActive, f => f.Random.Bool());

            var teamFakr = new Faker<Team>()
                .RuleFor(t => t.Name, f => f.Commerce.ProductName())
                .RuleFor(t => t.MissionStatement, f => f.Company.CatchPhrase())
                .RuleFor(t => t.TargetProductivity, f => f.Random.Int(80, 120))
                .RuleFor(t => t.FormationDate, f => f.Date.Past(5).ToUniversalTime())
                .RuleFor(t => t.TeamLead, f => f.Name.FullName())
                .RuleFor(t => t.IsCrossFunctional, f => f.Random.Bool());

            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.JobTitle, f => f.Name.JobTitle())
                .RuleFor(e => e.HireDate, f => f.Date.Past(3).ToUniversalTime())
                .RuleFor(e => e.Salary, f => f.Finance.Amount(2000, 10000))
                .RuleFor(e => e.Skills, f => string.Concat(f.Lorem.Words(5), ','))
                .RuleFor(e => e.YearsOfExperience, f => f.Random.Int(0, 20))
                .RuleFor(e => e.IsManager, f => f.Random.Bool());

            var taskFaker = new Faker<Entities.Task>()
                .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                .RuleFor(t => t.DueDate, f => f.Date.Future(30).ToUniversalTime())
                .RuleFor(t => t.Priority, f => f.Random.Int(1, 5))
                .RuleFor(t => t.Status, f => f.PickRandom(new[] { "Not-Started", "In-Progress", "Completed" }))
                .RuleFor(t => t.StartDate, f => f.Date.Past(14).ToUniversalTime())
                .RuleFor(t => t.CompletionDate, (f, t) => t.Status == "Completed" ? f.Date.Between(t.StartDate, DateTime.Now):DateTime.MinValue);

            var salaryPaymentFaker = new Faker<Entities.SalaryPayment>()
                .RuleFor(s => s.GrossAmount, f => f.Finance.Amount(1500, 8000))
                .RuleFor(s => s.NetAmount, (f, s) => s.GrossAmount * 0.7m)
                .RuleFor(s => s.PaymentDate, f => f.Date.Past(2).ToUniversalTime())
                .RuleFor(s => s.PaymentMethod, f => f.PickRandom(new[] { "Direct Deposit", "Check", "Cash" }))
                .RuleFor(s => s.PaymentStatus, f => "Processed")
                .RuleFor(s => s.Currency, f => "USD");

            await context.Database.BeginTransactionAsync();

            var departments = departmentFakr.Generate(1);
            context.departments.AddRange(departments);
            await context.SaveChangesAsync();

            foreach(var department in departments)
            {
                var teams = teamFakr.Generate(100);
                department.Teams = teams;
                await context.SaveChangesAsync();

                foreach(var team in teams)
                {
                    team.DepartmentId = department.Id;
                    var employees = employeeFaker.Generate(Randomizer.Seed.Next(100, 1000));
                    team.Employees = employees;
                    await context.SaveChangesAsync();

                    foreach(var employee in employees)
                    {
                        employee.TeamId = team.Id;
                        var tasks = taskFaker.Generate(Randomizer.Seed.Next(50,1000));
                        foreach(var task in tasks)
                        {
                            task.EmployeeId = employee.Id;
                        }
                        employee.Tasks = tasks;
                        await context.SaveChangesAsync();

                        var salaryPayments = salaryPaymentFaker.Generate(Randomizer.Seed.Next(20, 200));
                        foreach(var payment in salaryPayments)
                        {
                            payment.EmployeeId = employee.Id;
                        }
                        employee.SalaryPayments = salaryPayments;
                        await context.SaveChangesAsync();
                    }
                }
            }

            await context.SaveChangesAsync();
            await context.Database.CommitTransactionAsync();

        }
    }
}
