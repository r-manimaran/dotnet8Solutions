﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PolicyBasedAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthConstants.UserGroupMobile)]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = new[]
            {
                new {Id=1,Name="Laptop"},
                new {Id=2,Name="Mouse"},
                new {Id=3,Name="Mobile"}
            };
            return Ok(products);
        }
    }
}
