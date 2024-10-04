using AutoMapper;
using FluentValidation;
using CestaFeira.Domain.Command.Base;
using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Dtos.AppSettings;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Helpers;
using CestaFeira.Domain.Interfaces.DataModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using CestaFeira.Domain.Interfaces.CryptographyService;

namespace CestaFeira.Domain.CommandHandler.Usuario
{
    public class LoginUsuarioCommandHandler : CreateCommandHandlerBase<LoginUsuarioCommand, UsuarioEntity>
    {
        private readonly IOptions<AppSettings> settings;
        private readonly ICryptographyService cryptographyService;
        private CryptographyHelper _cryptografyHelper = new CryptographyHelper();

        public LoginUsuarioCommandHandler(
            IDataModuleDBAps dataModule,
        IMapper mapper,
            IValidator<LoginUsuarioCommand> validator,
            ICryptographyService cryptographyService,
            IOptions<AppSettings> settings)
        : base(dataModule, mapper, validator, dataModule.UsuarioRepository)
        {
            this.cryptographyService = cryptographyService;
            this.settings = settings;
        }

        public override async Task<CommandBaseResult> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
        {

            var _dto = await Repository?.DataSet
                 .AsNoTracking()
                 .Where(x => x.Email.Equals(request.Email))
                 .FirstOrDefaultAsync();


            if (_dto == null)
            {
                return new CommandBaseResult
                {
                    Success = false,
                    Message = "Usuário não localizado."
                };
            }

            var objUsuario = mapper.Map<UsuarioDto>(_dto);

            string senhaDescriptografado = _cryptografyHelper.Decrypt(objUsuario.Senha);


            if (!senhaDescriptografado.Equals(request.Senha))
            {
                return new CommandBaseResult
                {
                    Success = false,
                    Message = "Senha incorreta."
                };
            }

            objUsuario.Senha = string.Empty;


            return new CommandBaseResult()
            {
                Result = new LoginDtoResult
                {
                    Usuario = objUsuario
                },
                Success = true
            };
        }

    }
}
