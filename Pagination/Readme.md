## Implementing Pagination in .Net using Offset pagination and Keyset pagination

There are two ways to implement pagination.
1. Offset pagination
2. Keyset pagination or cursor-based pagination.

**Offset pagination**:
- The offset pagination requires two values in the input.
  - 1. the page number
  - 2. the page size
- This two will be used to query the database and fetch the results.
- It uses the Skip method (FETCH NEXT/ OFFSET in SQL) to return the data.
- This approach **supports random access pagination**, which means the users can jump to any page they wants. 

**Keyset pagination**:
- Keyset pagination also known as seek-based pagination or cursor based pagination.
- Its an alternative to the offset pagination.
- It uses where clause to skip instead of using an offset (Skip).
- In this approach the client knows the last searched element and uses it as a filter in the where condition.
- The keyset pagination requires two properties as input:
  - a reference value : which can be some sequential identifier for the last retured value.
  - a page size value : size of page
- The Key must be some sortable property such as Sequential id or a date-time property that we can compare. 

**Database creation and seeding**
![alt text](images\image.png)

**Endpoints: Products**

![alt text](images\image-1.png)

![alt text](images\image-4.png)

![alt text](images\image-2.png)

![alt text](images\image-3.png)

**Endpoints:Orders**

![alt text](images\image-6.png)

![alt text](images\image-5.png)


**Offset vs Keyset pagination consideration and usecases**:
- Consider Keyset pagination where you do not need to jump to a random page, instead you only need to access the previous and the next page.
- Consider where you want to create an endless scroll content application such as showing posts on social media etc..
 

- Offset pagination is recommended for cases when you don't have a large amount of data, or
- When you needed to have a possibility of jump to a specific page.
- If you want to navigate to not only the previous or next pages, but also jump to specific page.

**OffSet**:
  - **Pros:**
    - Possible to jump to a random page
    - Easier implementation
  -**Cons:**
    - Can possibly miss or duplicate items
    - Not performant for large data sets

**Keyset**:
  - **Pros:**
    - Better Performance
    - The results are consistent
  - **Cons:**
    - Not Possible to jump to a random page
    - The records needed to be stored in a sequential way
    - Client needs to manage the keyset value