using FluentValidation;
using SchooProjectlApi.DTOs;

namespace SchooProjectlApi.Validators;
public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Password).NotEmpty();
    }
}
