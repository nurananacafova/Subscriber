using FluentValidation;
using SubscriberService.Models;

namespace SubscriberService.Validation;

public class SubscriberValidator : AbstractValidator<SubscriberModel>
{
    public SubscriberValidator()
    {
        RuleFor(subscriber => subscriber.id).NotNull().WithMessage("ID Required!");
        RuleFor(subscriber => subscriber.language).NotEmpty().WithMessage("Language Required!").Length(1, 50)
            .WithMessage("Not Valid");
        RuleFor(subscriber => subscriber.email).NotEmpty().WithMessage("Email Required!").Length(1, 50)
            .WithMessage("Not Valid");
    }
}