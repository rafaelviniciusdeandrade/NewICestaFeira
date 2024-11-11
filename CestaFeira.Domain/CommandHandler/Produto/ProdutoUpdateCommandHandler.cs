using AutoMapper;
using CestaFeira.Domain.Command.Produto;
using CestaFeira.Domain.CommandHandler.Base;
using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.DataModule;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.CommandHandler.Produto
{
    public class ProdutoUpdateCommandHandler
        : UpdateCommandHandlerBase<ProdutoUpdateCommad, ProdutoEntity>
    {
        public ProdutoUpdateCommandHandler(
            IDataModuleDBAps dataModule,
            IMapper mapper,
            IValidator<ProdutoUpdateCommad> validator)
        : base(dataModule,
              mapper,
              validator,
              dataModule.ProdutoRepository)
        {

            OnRequestRepositoryData += async (ProdutoUpdateCommad request) =>
            {
                var dbData = await dataModule.ProdutoRepository.DataSet.FirstOrDefaultAsync(x => x.Id.Equals(request.Id))
                ??
                throw new ArgumentException("Produto não localizado.");

                var dbEntity = mapper.Map<ProdutoEntity>(dbData);
                dbEntity.quantidade = dbEntity.quantidade-request.quantidade;

                await Repository.UpdateAsync(dbEntity);

                return dbData;
            };
        }
    }
}