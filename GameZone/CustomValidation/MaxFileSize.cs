using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace GameZone.CustomValidation
{
    public class MaxFileSize: ValidationAttribute
    {
        private readonly int _maxFilesize;
        public MaxFileSize(int maxFilesize)
        {
            _maxFilesize = maxFilesize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // casting the value which object to IFormFile 
            var file = value as IFormFile;

            if (file is not null)
            {
                //Length [in bytes]
                if (file.Length > _maxFilesize)
                {
                    return new ValidationResult($"Maximum allowed Size is {_maxFilesize} bytes!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
