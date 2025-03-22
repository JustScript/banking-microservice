namespace BankingMicroservice.Validators
{
    public interface IValidate
    {
        bool IsValid { get; }

        bool Validate();
    }
}