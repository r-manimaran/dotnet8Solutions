using CompanyApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Services
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyResponse>> GetAllCompaniesAsync();
        Task<CompanyResponse> GetCompanyByIdAsync(int id);
    }
    public class CompanyService(AppDbContext dbContext, ILogger<CompanyService> logger)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly ILogger<CompanyService> _logger = logger;

        public async Task<IEnumerable<CompanyResponse>> GetAllCompaniesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all companies");

                var companies = await _dbContext.Companies
                    .AsNoTracking()
                    .Select(c => new CompanyResponse(c.Id, c.Name))
                    .ToListAsync();

                _logger.LogInformation("Successfully retrieved {Count} companies", companies.Count);

                return companies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all companies");
                throw new ApplicationException("Error occurred while retrieving companies", ex);
            }
        }

        public async Task<CompanyResponse> GetCompanyByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving company with ID: {CompanyId}", id);

                var company = await _dbContext.Companies
                    .AsNoTracking()
                    .Where(c => c.Id == id)
                    .Select(c => new CompanyResponse(c.Id, c.Name))
                    .FirstOrDefaultAsync();

                if (company == null)
                {
                    _logger.LogWarning("Company with ID: {CompanyId} not found", id);
                    throw new Exception($"Company with ID {id} not found");
                }

                _logger.LogInformation("Successfully retrieved company with ID: {CompanyId}", id);

                return company;
            }           
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving company with ID: {CompanyId}", id);
                throw new ApplicationException($"Error occurred while retrieving company with ID {id}", ex);
            }
        }
    }
}

    
