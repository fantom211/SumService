using Microsoft.EntityFrameworkCore;
using SumService.Models;
using SumService.Models.DTOs;

namespace SumService.Services
{
    public static class CreditMapper
    {
        public static CreditResponseDto ToDto(CreditEntity entity)
        {
            return new CreditResponseDto
            {
                Id = entity.Id,
                CreditOfferId = entity.CreditOfferId,
                Name = entity.Name,
                PrincipalAmount = entity.PrincipalAmount,
                InterestRate = entity.InterestRate,
                TermInMonths = entity.TermInMonths,
                PaymentType = entity.PaymentType,
                CreatedAt = entity.CreatedAt,
                PaymentSchedule = entity.PaymentSchedule
                    .Select(ToDto).
                    ToList()
            };
        }

        public static CreditEntity ToEntity(
            CreditCreateDto dto, 
            decimal interestRate, 
            CreditOfferEntity? offer, 
            Guid userId)
        {
            return new CreditEntity
            {
                UserId = userId,
                CreditOfferId = offer?.Id,
                CreditOffer = offer,
                Name = string.IsNullOrWhiteSpace(dto.Name) 
                    ? "Мой кредит" : dto.Name.Trim(),
                PrincipalAmount = dto.PrincipalAmount,
                InterestRate = interestRate,
                TermInMonths = dto.TermInMonths,
                PaymentType = dto.PaymentType,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static PaymentScheduleDto ToDto(PaymentScheduleEntity entity)
        {
            return new PaymentScheduleDto
            {
                PaymentNumber = entity.PaymentNumber,
                PaymentDate = entity.PaymentDate,
                PaymentAmount = entity.PaymentAmount,
                PrincipalAmount = entity.PrincipalAmount,
                InterestAmount = entity.InterestAmount,
                RemainingPrincipal = entity.RemainingPrincipal
            };
        }
    }
}
