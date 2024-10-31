using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Validator.Usuario
{
    public class UsuarioCreateValidator : BaseValidator<UsuarioCreateCommand>
    {
        public UsuarioCreateValidator(IDataModuleDBAps dataModuleDBAps)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Informe o e-mail do usuário.");
            RuleFor(x => x).MustAsync(async (command, cancellation) => await IsUnique(dataModuleDBAps, command)).WithMessage("E-mail já cadastrado, faça login ou utilize um novo");
            RuleFor(x => x).MustAsync(async (command, cancellation) => await IsUniqueCpf(dataModuleDBAps, command)).WithMessage("CPF já cadastrado, faça login ou utilize um novo");

            RuleFor(x => x.Senha).NotEmpty().WithMessage("Informe A SENHA do usuário.");

        }
        private async Task<bool> IsUnique(IDataModuleDBAps dataModuleDBAps, UsuarioCreateCommand command)
        {
            var result = await dataModuleDBAps.UsuarioRepository.DataSet
                .Where(x => x.Email.Equals(command.Email))
                .ToListAsync();

            return result.Count().Equals(0);
        }

        private async Task<bool> IsUniqueCpf(IDataModuleDBAps dataModuleDBAps, UsuarioCreateCommand command)
        {
            var result = await dataModuleDBAps.UsuarioRepository.DataSet
                .Where(x => x.cpf.Equals(command.cpf))
                .ToListAsync();

            return result.Count().Equals(0);
        }
    }
}