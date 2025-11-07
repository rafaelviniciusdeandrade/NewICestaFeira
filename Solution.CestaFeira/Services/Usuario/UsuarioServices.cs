using CestaFeira.Domain.Command.Usuario;
using CestaFeira.Domain.Dtos.Usuario;
using CestaFeira.Domain.Query.Usuario;
using CestaFeira.Web.Models.Usuario;
using CestaFeira.Web.Services.Interfaces;
using MediatR;
namespace CestaFeira.Web.Services.Usuario
{

    public class UsuarioServices : IUsuarioService
    {
        private readonly IMediator _mediator;

        public UsuarioServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<UsuarioModel> ValidarUsuario(UsuarioLoginModel login)
        {
            var loginCommand = new LoginUsuarioCommand
            {
                Email = login.Email,
                Senha = login.Senha
            };

            var result = await _mediator.Send(loginCommand);

            if (result.Success)
            {
                var usuarioDto = (result.Result as LoginDtoResult)?.Usuario;
                return new UsuarioModel
                {
                    Email = usuarioDto.Email,
                    Perfil = usuarioDto.Perfil,
                    Id = usuarioDto.Id,
                };
            }

            return null;
        }

        public async Task<bool> CadastrarUsuario(UsuarioModel usuario)
        {
            var usuarioCommand = new UsuarioCreateCommand
            {
                cpf = usuario.cpf,
                Email = usuario.Email,
                Senha = usuario.Senha,
                Nome = usuario.Nome,
                NomeFantasia=usuario.NomeFantasia,
                Cel = usuario.cpf,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Uf=usuario.Uf,
                Data = usuario.Data=DateTime.Now,
                Ativo = usuario.Ativo = true,
                Perfil= usuario.Perfil
            };

            var result = await _mediator.Send(usuarioCommand);

            if (result.Success)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public async Task<UsuarioModel> ConsultarUsuario(Guid id)
        {
            // 1. Criação do Query/Command para consulta
            // Assumindo a existência de um 'GetUsuarioQuery' no seu Domain
            var getUsuarioQuery = new UsuarioQuery // Este nome é uma sugestão
            {
                UsuarioId = id
            };

            // 2. Envia a Query através do MediatR
            // Assumindo que o resultado retornado pelo handler tem um campo 'Result'
            // que contém o DTO do usuário, e que o DTO é 'UsuarioDtoResult' (ou similar).
            var result = await _mediator.Send(getUsuarioQuery);

            // 3. Verifica o sucesso da operação
            if (result.Id != null)
            {

                // 4. Mapeia o DTO para o Model de Apresentação (UsuarioModel)
                return new UsuarioModel
                {
                    Id = result.Id,
                    Nome = result.Nome,
                    Email = result.Email,
                    Perfil = result.Perfil,
                    cpf = result.cpf, // ou Cpf, dependendo da sua DTO
                                      // Mapear todos os outros campos relevantes
                    NomeFantasia = result.NomeFantasia,
                    Rua = result.Rua,
                    Bairro = result.Bairro,
                    Cidade = result.Cidade,
                    Uf = result.Uf,
                    Data = result.Data,
                    Ativo = result.Ativo
                };
            }
            else { return null; }
        }


        }

    }
