using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SumService.Models.DTOs
{
    public class PaymentScheduleDto
    {
        public int PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal RemainingPrincipal { get; set; }
    }
}
