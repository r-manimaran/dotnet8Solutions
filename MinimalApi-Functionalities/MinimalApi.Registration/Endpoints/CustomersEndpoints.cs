using Microsoft.AspNetCore.Mvc;
using MinimalApi.Registration.Models;
using MinimalApi.Registration.Repositories;
using MinimalApis.Discovery;

namespace MinimalApi.Registration.Endpoints;

public class CustomersEndpoints : IApi
{
    public void Register(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api");

        group.MapPost("Customers", CreateCustomers);
        group.MapGet("/{id}", GetCustomerById);
    }

    private Customer CreateCustomers(Customer customer, ICustomerRepo customerRepo)
    {
        var response = customerRepo.AddCustomer(customer);
        return response;
    }
    private Customer GetCustomerById(int Id, ICustomerRepo customerRepo) 
    { 
        var response = customerRepo.GetCustomer(Id);
        return response;
    }
}
