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
- 
