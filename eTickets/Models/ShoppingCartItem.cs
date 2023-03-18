using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTickets.Models
{
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        public Movie Movie { get; set; }

        public int Amount { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

    }
}
