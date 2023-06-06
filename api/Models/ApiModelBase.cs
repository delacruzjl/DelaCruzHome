using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Api.Models;

public abstract class ApiModelBase  {
     public virtual async Task Validate<T>(IValidator<T> validator, ILogger logger) where T : class {
        using (logger.BeginScope("NewsletterContact.Validate"))
        {
            logger.LogInformation("Validating NewsletterContact");
            var validationResult = await validator.ValidateAsync(this as T);
            if (!validationResult.IsValid) {
                var message = string.Join(", ", validationResult.Errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}"));
                logger.LogError($"NewsletterContact Validation failed - {message}");
                throw new ArgumentException(message);
            }
        }
    }
}