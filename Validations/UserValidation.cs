using FluentValidation;
using LabAllianceTest.Models;

namespace LabAllianceTest.Validations
{
    internal class UserValidation : AbstractValidator<UserModel>
    {
        public UserValidation()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Логин не может быть пустым.")
                .Length(3, 20).WithMessage("Логин должен содержать от 3 до 20 символов.");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Длина пароля должна составлять минимум 8 символов")
                .MaximumLength(16).WithMessage("Длина пароля недолжна  превышать 16 символов");
        }
    }
}
