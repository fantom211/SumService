using Microsoft.EntityFrameworkCore;
using SumService.Data;
using SumService.Models;
using SumService.Models.DTOs;
using SumService.Services;

namespace SumService.Service
{
    public class CreditCalculationService
    {
        private readonly CreditDbContext _context;

        public CreditCalculationService(CreditDbContext context)
        {
            _context = context;
        }

        public async Task<List<CreditResponseDto>> GetAllAsync(Guid userId)
        {
            var credits = await _context.Credits
                .Include(c => c.PaymentSchedule)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return credits
                .Select(CreditMapper.ToDto)
                .ToList();
        }

        public async Task<CreditResponseDto> GetByIdAsync(Guid id)
        {
            var credit = await _context.Credits
                .Include(x=>x.PaymentSchedule)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (credit == null)
                throw new KeyNotFoundException("Такого кредита нет.");
            return CreditMapper.ToDto(credit);
        }

        public async Task<CreditResponseDto> CreateAsync(CreditCreateDto dto, Guid userId)
        {
            CreditOfferEntity? offer = null;
            decimal interestRate = dto.InterestRate;

            if(dto.CreditOfferId.HasValue)
            {
                offer = await _context.CreditOffers
                    .FirstOrDefaultAsync(x => x.Id == dto.CreditOfferId.Value);

                if (offer is null)
                {
                    throw new KeyNotFoundException("Кредитное предложение не найдено."); 
                }

                interestRate = offer.InterestRate;

                if (offer.InterestRate != dto.InterestRate)
                    throw new Exception("Ваша процентная ставка не соответсвует офферу");
                if (offer.MinTermInMonths > dto.TermInMonths)
                    throw new Exception("Слишком маленький срок кредита.");
                if (offer.MaxTermInMonths < dto.TermInMonths)
                    throw new Exception("Слишком большой срок кредита.");
            }

            var credit = CreditMapper.ToEntity(dto, interestRate, offer, userId);

            var schedule = CalculateSchedule(credit);

            credit.PaymentSchedule = schedule;

            _context.Credits.Add(credit);
            await _context.SaveChangesAsync();

            return CreditMapper.ToDto(credit);

            
        }

        public async Task CalculateCreditAsync(CreditCreateDto dto, Guid userId)
        {
            CreditOfferEntity? offer = null;
            decimal interestRate = dto.InterestRate;

            if (dto.CreditOfferId.HasValue)
            {
                offer = await _context.CreditOffers
                    .FirstOrDefaultAsync(x => x.Id == dto.CreditOfferId.Value);

                if (offer is null)
                {
                    throw new KeyNotFoundException("Кредитное предложение не найдено.");
                }
                interestRate = offer.InterestRate;

                if (offer.InterestRate != dto.InterestRate)
                    throw new Exception("Ваша процентная ставка не соответсвует офферу");
                if (offer.MinTermInMonths > dto.TermInMonths)
                    throw new Exception("Слишком маленький срок кредита.");
                if (offer.MaxTermInMonths < dto.TermInMonths)
                    throw new Exception("Слишком большой срок кредита.");
            }

            var credit = new CreditEntity
            {
                UserId = userId,
                CreditOfferId = offer?.Id,
                CreditOffer = offer,
                Name = string.IsNullOrWhiteSpace(dto.Name) ? "Мой кредит" : dto.Name.Trim(),
                PrincipalAmount = dto.PrincipalAmount,
                InterestRate = interestRate,
                TermInMonths = dto.TermInMonths,
                PaymentType = dto.PaymentType,
                CreatedAt = DateTime.UtcNow
            };

            credit.PaymentSchedule = CalculateSchedule(credit);
        }



        public List<PaymentScheduleEntity> CalculateSchedule(CreditEntity credit)
        {
            return credit.PaymentType switch
            {
                PaymentType.Annuity =>
                    CalculateAnnuitySchedule(credit),

                PaymentType.Differentiated =>
                    CalculateDifferencialSchedule(credit),

                _ => throw new Exception("Unknown payment type.")
            };
        }

        public static List<PaymentScheduleEntity> CalculateAnnuitySchedule(CreditEntity credit)
        {
            var schedule = new List<PaymentScheduleEntity>();

            decimal remaining = credit.PrincipalAmount;

            decimal monthlyRate = credit.InterestRate/1200m;

            int months = credit.TermInMonths;

            if(monthlyRate==0)
            {
                decimal monthlyPrincipal =
                    Math.Round(credit.PrincipalAmount / months, 2);

                for (int month = 1; month <= months; month++)
                {
                    decimal principal = monthlyPrincipal;

                    if (month == months)
                        principal = remaining;

                    remaining -= principal;

                    schedule.Add(new PaymentScheduleEntity
                    {
                        CreditId = credit.Id,
                        PaymentNumber = month,
                        PaymentDate = DateTime.UtcNow.AddMonths(month),
                        PaymentAmount = principal,
                        PrincipalAmount = principal,
                        InterestAmount = 0m,
                        RemainingPrincipal = Math.Max(remaining, 0m)
                    });
                }

                return schedule; 
            }
            decimal factor = (decimal)Math.Pow((double)(1 + monthlyRate), months);

            decimal monthlyPayment =
                credit.PrincipalAmount
                * monthlyRate
                * factor
                / (factor - 1);

            monthlyPayment = Math.Round(monthlyPayment, 2);

            for (int month = 1; month <= months; month++)
            {
                decimal interest = Math.Round(
                    remaining * monthlyRate,
                    2);

                decimal payment = monthlyPayment;

                decimal principal = payment - interest;

                if (month == months)
                {
                    principal = remaining;
                    payment = principal + interest;
                }

                remaining -= principal;

                schedule.Add(new PaymentScheduleEntity
                {
                    CreditId = credit.Id,
                    PaymentNumber = month,
                    PaymentDate = DateTime.UtcNow.AddMonths(month),

                    PaymentAmount = Math.Round(payment, 2),

                    PrincipalAmount = Math.Round(principal, 2),

                    InterestAmount = Math.Round(interest, 2),

                    RemainingPrincipal = Math.Max(
                        Math.Round(remaining, 2),
                        0m)
                });
            }

            return schedule;
        }

        public static List<PaymentScheduleEntity> CalculateDifferencialSchedule(CreditEntity credit)
        {
            var schedule = new List<PaymentScheduleEntity>();

            decimal remaining = credit.PrincipalAmount; //текущий остаток кредита
            decimal monthlyPrincipal = 
                Math.Round(credit.PrincipalAmount / credit.TermInMonths, 2);

            decimal monthlyRate = credit.InterestRate / 1200m;

            for (int month = 1; month <=credit.TermInMonths; month++)
            {
                decimal interest = Math.Round(remaining * monthlyRate, 2);

                decimal principal = monthlyPrincipal;

                if (month == credit.TermInMonths)
                    principal = remaining;

                decimal payment = principal + interest;

                decimal newRemainig = remaining - principal;

                schedule.Add(new PaymentScheduleEntity
                {
                    CreditId = credit.Id,
                    PaymentNumber = month,
                    PaymentDate = DateTime.UtcNow.AddMonths(month),
                    PaymentAmount = payment,
                    PrincipalAmount = principal,
                    InterestAmount = interest,
                    RemainingPrincipal = Math.Max(newRemainig, 0)
                });

                remaining = newRemainig;
            }
            return schedule;
        }

        
    }
}