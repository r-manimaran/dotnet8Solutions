using MinimalApi.Registration.Models;

namespace MinimalApi.Registration.Repositories;

public interface ICustomerRepo
{
    Customer AddCustomer(Customer customer);
    Customer UpdateCustomer(Customer customer);
    Customer GetCustomer(int id);
    bool DeleteCustomer(int id);
}
