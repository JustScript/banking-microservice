using BankingMicroservice.Models;

namespace BankingMicroservice.Services
{
    public interface IActionsService
    {
        IEnumerable<string> GetAllowedActions(CardDetails cardDetails);
    }
}