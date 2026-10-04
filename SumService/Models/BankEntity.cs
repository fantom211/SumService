using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SumService.Models
{
    public class BankEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<CreditOfferEntity> CreditOffers { get; set; }
            = new List<CreditOfferEntity>();
    }
}
