using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookSmart.Api.Exceptions;

public class ModelValidationException : Exception
{
    public ModelValidationException(ModelStateDictionary modelState)
    {
        Errors = modelState.Values.Select(e => e.Errors.Select(error => error.ErrorMessage).ToArray().First())
            .ToArray();
    }

    public string[] Errors { get; }
}