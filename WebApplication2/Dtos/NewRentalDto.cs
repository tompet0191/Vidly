using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vidly.Dtos
{
    public class NewRentalDto
    {
        public int CustomerId { get; set; }
        public IEnumerable<int> MovieIds { get; set; }
    }
}
