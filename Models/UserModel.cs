using FluentValidation.Results;
using LabAllianceTest.Validations;

namespace LabAllianceTest.Models
{
    public class UserModel
    {
        public Guid Id { get; }
        public string Login { get; } = string.Empty;
        public string Password { get; } = string.Empty;

        public UserModel(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public static (Dictionary<string, string> errors, UserModel user) Create(string login, string password, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            UserModel user = new UserModel(login, password);
            if (!useValidation) { return (errors, user); }

            UserValidation userValidation = new UserValidation();
            ValidationResult result = userValidation.Validate(user);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, user);
        }
    }
}
