using AutoMapper;
using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Helpers;
using CestaFeira.Domain.Interfaces.DataModule;
using FluentValidation;


namespace CestaFeira.Domain.CommandHandler.Usuario
{
    public class UsuarioCreateCommadHandler : CreateCommandHandlerBase<UsuarioCreateCommand, UsuarioEntity>
    {
        private CryptographyHelper _cryptografyHelper = new CryptographyHelper();
        public UsuarioCreateCommadHandler(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<UsuarioCreateCommand> validator)
        : base(dataModule, mapper, validator, dataModule.UsuarioRepository)
        {
        }

        public override async Task<CommandBaseResult> Handle(UsuarioCreateCommand request, CancellationToken cancellationToken)
        {

            var objEntity = mapper.Map<UsuarioEntity>(request);

            string salt = _cryptografyHelper.GenerateSalt();

            objEntity.Senha = _cryptografyHelper.Encrypt(request.Senha, salt);

            var dbEnitty = await Repository.InsertAsync(objEntity);

            return new CommandBaseResult()
            {
                Result = dbEnitty.Id,
                Success = true
            };
        }

    }
}