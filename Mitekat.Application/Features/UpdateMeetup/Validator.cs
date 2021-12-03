namespace Mitekat.Application.Features.UpdateMeetup;

using FluentValidation;

internal class UpdateMeetupRequestValidator : AbstractValidator<UpdateMeetupRequest>
{
    public UpdateMeetupRequestValidator()
    {
        RuleFor(request => request.MeetupId).NotEmpty();

        RuleFor(request => request.Properties)
            .NotNull()
            .SetValidator(new EditableMeetupPropertiesValidator());
    }
}

internal class EditableMeetupPropertiesValidator : AbstractValidator<EditableMeetupProperties>
{
    public EditableMeetupPropertiesValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);
        
        RuleFor(request => request.Description)
            .NotNull()
            .MaximumLength(2500);
        
        RuleFor(request => request.Speaker)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(request => request.Duration)
            .NotEmpty()
            .GreaterThanOrEqualTo(60 / 2)
            .LessThanOrEqualTo(16 * 60);
        
        RuleFor(request => request.StartTime).NotEmpty();
    }
}
