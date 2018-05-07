using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vidly.mongoDB;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly IDbContext _ctx;

        public CustomersController(IDbContext ctx)
        {
            _ctx = ctx;
        }

        //GET /api/Customers
        public IEnumerable<Customer> GetCustomers()
        {
            return _ctx.Customers.Find(x => true).ToList();
        }
    }
}