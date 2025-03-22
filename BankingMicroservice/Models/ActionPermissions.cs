namespace BankingMicroservice.Models
{
    public record ActionPermissions(CardActions Action, bool Prepaid, bool Debit, bool Credit, bool Ordered, bool Inactive, bool Active, bool Restricted, bool Blocked, bool Expired, bool Closed);
}