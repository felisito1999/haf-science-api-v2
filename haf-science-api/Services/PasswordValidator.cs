using FluentValidation;
using haf_science_api.Models;

namespace haf_science_api.Services
{
    public class PasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public PasswordValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña no debe estar vacía o con espacios vacíos")
                .MinimumLength(6)
                .WithMessage("La contraseña debe tener al menos 6 caracteres")
                .MaximumLength(30)
                .WithMessage("La contraseña no debe tener más de 30 caracteres")
                .Matches("[A-Z]")
                .WithMessage("La contraseña debe contener al menos una mayúscula")
                .Matches("[a-z]")
                .WithMessage("La contraseña debe contener al menos una minúscula")
                .Matches("[0-9]")
                .WithMessage("La contraseña debe tener por lo menos un número")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("La contraseña debe contener al menos un carácter especial");
        }
    }
}
