using MinimalApi.Registration.Models;

namespace MinimalApi.Registration.Repositories;

public class CustomerRepo : ICustomerRepo
{
    private List<Customer> customers = new List<Customer>();
    public Customer AddCustomer(Customer customer)
    {
        
        customers.Add(customer); 
        return customer;
    }

    public bool DeleteCustomer(int id)
    {
        var customer = customers.FirstOrDefault(x => x.Id == id);
        if (customer == null)
        {
            return false;
        }
        customers.Remove(customer);
        return true;
    }

    public Customer GetCustomer(int id)
    {
        var customer = customers.FirstOrDefault(x => x.Id == id);
        return customer;
    }

    public Customer UpdateCustomer(Customer customer)
    {
        var existingCustomer = customers.FirstOrDefault(x => x.Id == customer.Id);
        if (existingCustomer != null)
        {
            customers.Remove(existingCustomer);
            customers.Add(customer);
        }
        return customer;
    }
}
