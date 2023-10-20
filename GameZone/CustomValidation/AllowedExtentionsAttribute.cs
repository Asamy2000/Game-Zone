using System.ComponentModel.DataAnnotations;

namespace GameZone.CustomValidation
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string _allowedExtensions;
        public AllowedExtensionsAttribute(string allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // casting the value which object to IFormFile 
            var file = value as IFormFile;

            if (file is not null)
            {
               //get the uploaded file extension
               var extension = Path.GetExtension(file.FileName);

               // extensions which is allowed  [array of string]  [if it one of them with ignore CAPTALIZATION ]
               var isAllowed = _allowedExtensions.Split(',').Contains(extension,StringComparer.OrdinalIgnoreCase);

               if (!isAllowed)
                {
                    return new ValidationResult($"only {_allowedExtensions} are allowed!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
