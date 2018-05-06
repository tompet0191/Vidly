using System.Collections.Generic;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class CustomerViewModel
    {
        public List<Customer> Customers { get; set; }
        public List<MembershipType> MembershipTypes { get; set; }

        public CustomerViewModel()
        {
            Customers = new List<Customer>();
            MembershipTypes = new List<MembershipType>();
        }
    }
}
