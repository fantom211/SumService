using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SumService.Models
{
    public class PaymentScheduleEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CreditId { get; set; }

        public CreditEntity Credit { get; set; } = null!;

        [Required]
        public int PaymentNumber { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Precision(20,2)]
        public decimal PaymentAmount { get; set; }

        [Required]
        [Precision(20,2)]
        public decimal PrincipalAmount { get; set; }

        [Required]
        [Precision(20, 2)]
        public decimal InterestAmount { get; set; }

        [Required]
        [Precision(20, 2)]
        public decimal RemainingPrincipal { get; set; }
    }
}