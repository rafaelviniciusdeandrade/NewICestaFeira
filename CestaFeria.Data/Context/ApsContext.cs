using System.Reflection;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace CestaFeria.Data.Context
{
    public class ApsContext : DbContext
    {


        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<ProdutoEntity> Produtos { get; set; }

        public DbSet<PedidoEntity> Vendas { get; set; }

        private CryptographyHelper _cryptografyHelper = new CryptographyHelper();

        public ApsContext(DbContextOptions<ApsContext> options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This method should be empty to apply migrations correctly (commented to skip sonarqube code smells)
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string salt = _cryptografyHelper.GenerateSalt();
            var senha = _cryptografyHelper.Encrypt("12345678", salt);
            base.OnModelCreating(modelBuilder);


            //Configurando Mapeamentos de bancos de dados.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //Configurando para armazenar todos dados em UpCase.
            var converter = new ValueConverter<string, string>(
                v => v.ToUpper(), //Input

                v => v.ToUpper() //Output
            );

            //modelBuilder.Entity<ProdutoEntity>()
            //   .HasMany(l => l.Pedidos)
            //   .WithOne(t => t.Produtos)
            //   .HasForeignKey(t => t.Id)
            //   .OnDelete(DeleteBehavior.Cascade);

            //Inserindo usuário padrão.
            modelBuilder.Entity<UsuarioEntity>().HasData(
                new UsuarioEntity
                {
                    Id = new Guid("4ab52682-7f30-4f2a-abfc-313261d73761"),
                    cpf = "13080460812",
                    Email = "rafael@gmail.com",
                    Senha = senha,
                    Nome = "Administrador",
                    NomeFantasia="Teste",
                    Cel = "(35)11111111",
                    Rua = "Juscelino Kubitschek",
                    Numero = 555,
                    Bairro = "Jardim São Carlos",
                    Cidade = "Alfenas",
                    Uf = "MG",
                    Data = DateTime.Now,
                    Ativo = true,
                    Perfil = "ADM"
                }
            );


        }
    }
}