using BankingMicroservice.Models;

namespace BankingMicroservice.Extensions
{
    public static class ActionPermissionsExtensions
    {
        public static bool IsMatch(this ActionPermissions actionPermissions, CardDetails cardDetails)
        {
            bool statusMatch = cardDetails.CardStatus switch
            {
                CardStatus.Ordered => actionPermissions.Ordered,
                CardStatus.Inactive => actionPermissions.Inactive,
                CardStatus.Active => actionPermissions.Active,
                CardStatus.Restricted => actionPermissions.Restricted,
                CardStatus.Blocked => actionPermissions.Blocked,
                CardStatus.Expired => actionPermissions.Expired,
                CardStatus.Closed => actionPermissions.Closed,
                _ => false
            };

            bool typeMatch = cardDetails.CardType switch
            {
                CardType.Prepaid => actionPermissions.Prepaid,
                CardType.Debit => actionPermissions.Debit,
                CardType.Credit => actionPermissions.Credit,
                _ => false
            };

            return statusMatch && typeMatch;
        }
    }
}