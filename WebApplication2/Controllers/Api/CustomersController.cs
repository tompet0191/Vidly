using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vidly.Dtos;
using Vidly.mongoDB;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    [Produces("application/json")]
    //[Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly IDbContext _ctx;

        public CustomersController(IDbContext ctx)
        {
            _ctx = ctx;
        }

        //GET /api/Customers
        [HttpGet]
        [Route("api/Customers")]
        public IEnumerable<CustomerDto> GetCustomers()
        {
            var customers =
                Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(_ctx.Customers.Find(x => true).ToList());

            foreach (var customer in customers)
            {
                customer.MembershipTypeDto = new MembershipTypeDto
                {
                    MembershipType = customer.MembershipType,
                    Name = _ctx.MembershipTypes.Find(x => x.Id == customer.MembershipType).SingleOrDefault().Name
                };
            }

            return customers;
            //return _ctx.Customers.Find(x => true).ToList().Select(Mapper.Map<Customer, CustomerDto>);
        }

        //GET /api/Customers/1
        [HttpGet]
        [Route("api/Customers/{Id:int}")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _ctx.Customers.Find(x => x.CustomerId == id);

            if (customer.Count() == 0)
                return NotFound();

            return Ok(Mapper.Map<Customer, CustomerDto>(customer.FirstOrDefault()));
        }

        //POST /api/Customers
        [HttpPost]
        [Route("api/Customers")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = _ctx.Customers.Find(x => true).SortByDescending(d => d.CustomerId).Limit(1).First();

            customerDto.CustomerId = result.CustomerId + 1;

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            await _ctx.Customers.InsertOneAsync(customer);

            return Created($"api/Customers/" + customerDto.CustomerId, customerDto);
        }

        //PUT /api/Customers/1
        [HttpPut]
        [Route("api/Customers/{Id:int}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customerInDb = _ctx.Customers.Find(x => x.CustomerId == id);

            if (customerInDb.Count() == 0)
                return NotFound();

            var update = Builders<Customer>.Update.Set(x => x.BirthDate, customerDto.BirthDate)
                .Set(x => x.FirstName, customerDto.FirstName).Set(x => x.LastName, customerDto.LastName)
                .Set(x => x.IsSubscribedToNewsLetter, customerDto.IsSubscribedToNewsLetter)
                .Set(x => x.MembershipType, customerDto.MembershipType);
            await _ctx.Customers.UpdateOneAsync(x => x.CustomerId == id, update);

            Mapper.Map(customerInDb.FirstOrDefault(), customerDto);

            return Ok(customerDto);
        }

        // DELETE /api/Customers/1
        [HttpDelete]
        [Route("api/Customers/{Id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customerInDb = _ctx.Customers.Find(x => x.CustomerId == id);

            if (customerInDb.Count() == 0)
                return NotFound();

            await _ctx.Customers.DeleteOneAsync(x => x.CustomerId == id);

            return Ok();
        }
    }
}