namespace SumService.Models.DTOs
{
    public class CreditResponseDto
    {
        public Guid Id { get; set; }

        public Guid? CreditOfferId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal PrincipalAmount { get; set; }

        public decimal InterestRate { get; set; }

        public int TermInMonths { get; set; }

        public PaymentType PaymentType { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<PaymentScheduleDto> PaymentSchedule { get; set; } = new();
    }
}
