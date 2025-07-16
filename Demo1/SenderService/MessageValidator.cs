using FluentValidation;
class MessageValidator : AbstractValidator<SocialProfileDetails>
{
  public MessageValidator()
  {
    RuleFor(p => p.Id).NotEmpty();
    RuleFor(p => p.Name).NotEmpty();
    RuleFor(p => p.Discord).NotEmpty();
    RuleFor(p => p.Bluesky).NotEmpty();
    RuleFor(p => p.Linkedin).NotEmpty();
  }
}