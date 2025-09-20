using FluentValidation;
using SchooProjectlApi.DTOs.CourseDtos;

namespace SchooProjectlApi.Validators;
public class CourseDtoValidator : AbstractValidator<CourseDto>
{
    public CourseDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TeacherId).GreaterThan(0);
    }
}
