using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SumService.Models
{
    public class CreditEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        public Guid? CreditOfferId { get; set; }
        public CreditOfferEntity? CreditOffer { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Precision(20,2)]
        public decimal PrincipalAmount { get; set; }

        [Required]
        [Precision(5,2)]
        public decimal InterestRate { get; set; }

        [Required]
        public int TermInMonth { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }    

        [Required]
        [Precision(20,2)]
        public decimal TotalInterest { get; set; }

        [Required]
        [Precision(20, 2)]
        public decimal TotalRepaymentAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PaymentScheduleEntity> PaymentSchedule { get; set; }
            = new List<PaymentScheduleEntity>();
    }
}
