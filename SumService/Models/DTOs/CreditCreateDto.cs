using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SumService.Models.DTOs
{
    public class CreditCreateDto
    {
        public Guid? CreditOfferId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        //сумма кредита
        [Required]
        [Precision(20, 2)]
        [Range(typeof(decimal), "0.01", "999999999999999999.99",
            ParseLimitsInInvariantCulture = true)]
        public decimal PrincipalAmount { get; set; }

        //кредитная ставка
        [Required]
        [Precision(5, 2)]
        [Range(typeof(decimal), "0", "999.99",
            ParseLimitsInInvariantCulture = true)]
        public decimal InterestRate { get; set; }


        //срок кредита в месяцах
        [Required]
        [Range(1, int.MaxValue)]
        public int TermInMonths { get; set; }


        //тип кредита
        [Required]
        public PaymentType PaymentType { get; set; }

        //public DateTime CreatedAt { get; set; }
    }
}
