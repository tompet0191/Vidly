using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Vidly.ViewModels;
using Vidly.mongoDB;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private IDbContext _ctx;

        public CustomerController(IDbContext ctx)
        {
            _ctx = ctx;
        }

        [Route("Customers")]
        public ActionResult Index()
        {
            var customerViewModel = new CustomerViewModel();
            var customers = _ctx.Customers;
            var memberships = _ctx.MembershipTypes;

            customerViewModel.Customers = customers.Find(x => true).ToList(); //Find all customers and show them
            customerViewModel.MembershipTypes = memberships.Find(x => true).ToList(); //Find all movies and show them
            return View(customerViewModel);
        }

        [Route("Customers/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            var customerViewModel = new CustomerViewModel();
            
            var customers = _ctx.Customers;
            var memberships = _ctx.MembershipTypes;

            try
            {
                customerViewModel.Customers = customers.Find(x => x.CustomerId == id).ToList();
                customerViewModel.MembershipTypes = memberships.Find(x => true).ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return View(customerViewModel);
        }

        [Route("Customers/New")]
        public ActionResult New()
        {
            var newCustomerViewModel = new CustomerFormViewModel();
            var memberships = _ctx.MembershipTypes;

            newCustomerViewModel.MembershipTypes = memberships.Find(x => true).ToList();

            return View("CustomerForm", newCustomerViewModel);
        }

        [HttpPost]
        [Route("Customers/Save")]
        public ActionResult Save(Customer customer)
        {
            if (customer.CustomerId == 0)
            {
                var result = _ctx.Customers.Find(x => true).SortByDescending(d => d.CustomerId).Limit(1).First();

                customer.CustomerId = result.CustomerId + 1;
                _ctx.Customers.InsertOne(customer);
            }
            else
            {
                var update = Builders<Customer>.Update.Set(x => x.BirthDate, customer.BirthDate).Set(x => x.FirstName, customer.FirstName).Set(x => x.LastName, customer.LastName).Set(x => x.IsSubscribedToNewsLetter, customer.IsSubscribedToNewsLetter).Set(x => x.MembershipType, customer.MembershipType);
                _ctx.Customers.UpdateOne(x => x.CustomerId == customer.CustomerId, update);
            }

            return RedirectToAction("Index", "Customers");
        }
        
        [Route("Customers/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var customer = _ctx.Customers.Find(x => x.CustomerId == id).First();

            if (customer == null)
                return NotFound();

            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _ctx.MembershipTypes.Find(x => true).ToList()
            };

            return View("CustomerForm", viewModel);
        }
    }
}