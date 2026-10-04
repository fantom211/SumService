using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SumService.Models
{
    public class CreditOfferEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BankId { get; set; }

        public BankEntity Bank { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Precision(5, 2)]
        public decimal InterestRate { get; set; }

        [Required]
        public int MinTermInMonths { get; set; }

        [Required]
        public int MaxTermInMonths { get; set; }

        [Required]
        [Precision(20, 2)]
        public decimal MinAmount { get; set; }

        [Required]
        [Precision(20, 2)]
        public decimal MaxAmount { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }
    }
}