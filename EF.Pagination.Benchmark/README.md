
dotnet ef migrations add intialmigration

dotnet ef database update

![alt text](Images/image-1.png)

![alt text](Images\image.png)

![alt text](image-2.png)

![alt text](image-4.png)

![alt text](image-3.png)

**Execution:**
```bash
dotnet run --configuration Release
dotnet run -c Release
# to build in Release Mode
dotnet build -c Release
# to publish in Release Mode
dotnet publish -c Release
```
**Benchmark Results**

![alt text](image-5.png)

![alt text](image-6.png)

**SQL Server Query Execution and Plans**

![alt text](image-7.png)

- Enable the Query Execution plan in the SQL server and execute the query.

**Query 1:**

  ![alt text](image-8.png)

  ![alt text](image-9.png)

  ![alt text](image-10.png)

**Query 2:**

![alt text](image-11.png)

**Query 3:**
![alt text](image-12.png)

**Pagination Techniques**

- **Offset-based Pagination**:
    
    - In this approach we use skip() to skip a number of records based on the current page and .Take() to fetch a fixed number of records.
    - For example, for page number 2 and page size of 10, Skip((pageNumber-1)* pageSize) will skip 10 records and Take(pageSize) will take the next 10 records.
-  **Cursor-based Pagination**:
  
   - In cursor based pagination, we query for records greater than the last known ProductId (the cursor), which allows continuous navigation without the potential issues of offset pagination when records change between requests
   - This pagination technique is useful for large datasets, where offset-based pagination might become inefficient due to recalculating offsets.  
   - This approach drawback is that you can't switch to desired page in the pagination.
   - Diffcult to use when you have various sorting and filtering opttions.
   - Good usecases are, Useful in places like Real-time data like Social media timelines where you can implement infinite scrolling.


