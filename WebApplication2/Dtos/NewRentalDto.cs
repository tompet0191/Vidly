using System.Collections.Generic;

namespace Vidly.Dtos
{
    public class NewRentalDto
    {
        public int CustomerId { get; set; }
        public IEnumerable<int> MovieIds { get; set; }
    }
}