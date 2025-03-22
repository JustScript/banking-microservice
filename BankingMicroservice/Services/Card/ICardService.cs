using BankingMicroservice.Models;

namespace BankingMicroservice.Services
{
    public interface ICardService
    {
        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
    }
}