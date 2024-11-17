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
                .Length(3, 20).WithMessage("Логин должен содержать от 3 до 20 символов.")
                .Matches("^[^@]+$").WithMessage("Логин не должен содержать символ '@'.")
                .Matches("^[^\\s]+$").WithMessage("Логин не должен содержать пробелов.");

            RuleFor(x => x.Password)
                .MinimumLength(8).WithMessage("Длина пароля должна составлять минимум 8 символов")
                .MaximumLength(16).WithMessage("Длина пароля не должна превышать 16 символов")
                .Matches("[A-Za-zА-Я]").WithMessage("Пароль должен содержать хотя бы одну букву (русскую или английскую)")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Matches("[^a-zA-Z0-9А-Яа-я]").WithMessage("Пароль должен содержать хотя бы один специальный символ")
                .Matches("[A-ZА-Я]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву (русскую или английскую)");
        }
    }
}
