using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Query.Pedido;
using CestaFeira.Domain.Query.Usuario;
using CestaFeira.Domain.Validator.Base;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Validator.Usuario
{
    public class UsuarioQueryValidation : BaseValidator<UsuarioQuery>
    {
        public UsuarioQueryValidation(IDataModuleDBAps dataModuleDBAps)
        {
            RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("Informe o id do usuário.");
        }
    }
}
