using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi
{
    public class ExampleUsage(AppDbContext dbContext)
    {
        public void NewCreationExample()
        {
            var company = new Company
            {
                Name = "Tech Corp"
            };

            var user = new User
            {
                Name = "John Doe",
                Company = company
            };

            var project = new Project
            {
                Name = "New WebSite",
                Company = company
            };

            var userProject = new UserProject
            {
                User = user,
                Project = project,
                Role = "Lead Developer",
                AssignedDate = DateTime.Now,
            };

        }

        public async void QueryExamples()
        {
            // Get company and all users and projects
            int companyId = 1;
            var companyDetails = await dbContext.Companies
                                .Include(c => c.Users)
                                .Include(c => c.Projects)
                                .FirstOrDefaultAsync(c => c.Id == companyId);

            // Get User with their projects in a company
            var userWithProjects = await dbContext.Users
                                   .Include(u=>u.UserProjects)
                                   .ThenInclude(up=>up.Project)
                                   .Where(c => c.Id == companyId)
                                   .ToListAsync();


        }
    }
}
