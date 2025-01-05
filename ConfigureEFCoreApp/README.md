# EntityFrameworkCore Configuration

**IEntityTypeConfiguration**:

- IEntityTypeConfiguration is an interface in Entity Framework Core that allows you to separate and organize your entity configurations into individual classes, rather than putting all configuration logic in the OnModelCreating method of your DbContext. 
- We use to configure the entity directly in the OnModelCreating of DbContext. While this works fine for small applications, as your application grows with more entities and complex configurations, the OnModelCreating method can become large and difficult to maintain.
- Use IEntityTypeConfiguration to configure the individual entities.

**Key benefits of IEntityTypeConfiguration**:

1. Better organization and separation of concerns
2. More maintainable code as configurations are isolated per entity
3. Easier to test individual configurations
4. Reduces the complexity of the DbContext class
5. Makes it easier to find and modify entity-specific configurations

**Interceptors**:
 - Interceptors in Entity Framework core allow you to intercept and modify database operations before they are executed.
 - Some usecases are
    - Logging Interceptor : To Log SQL queries and their execution time:
    - Audit Trail Interceptor: To automatically track changes.
    - Transaction Interceptor: To manage transacations
    - Connection Interceptor: To handle connection Events.
    - Error handling interceptor: To handle database errors.
    

