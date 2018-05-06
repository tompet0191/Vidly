using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class MembershipType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int DiscountRate { get; set; }

        public static readonly string Unknown = null;
    }
}
