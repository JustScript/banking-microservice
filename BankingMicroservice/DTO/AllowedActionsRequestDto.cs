using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using BankingMicroservice.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace BankingMicroservice.DTO
{
    public class AllowedActionsRequestDto : IValidate
    {
        private bool? _isValid;

        [IgnoreDataMember]
        [SwaggerIgnore]
        public bool IsValid => _isValid ?? Validate();

        [Required]
        [SwaggerParameter(Description = "User ID")]
        public required string UserId { get; set; }

        [Required]
        [SwaggerParameter(Description = "Card ID")]
        public required string CardId { get; set; }

        public bool Validate()
        {
            _isValid = !string.IsNullOrWhiteSpace(UserId) 
                    && !string.IsNullOrWhiteSpace(CardId) 
                    && Regex.IsMatch(UserId, @"^[a-zA-Z0-9]*$")
                    && Regex.IsMatch(CardId, @"^[a-zA-Z0-9]*$");

            return _isValid.Value;
        }
    }
}