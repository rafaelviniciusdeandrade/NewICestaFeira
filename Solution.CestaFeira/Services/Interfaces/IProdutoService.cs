﻿using CestaFeira.Web.Models.Produto;

namespace CestaFeira.Web.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<bool> CadastrarProduto(ProdutoModel login);
    }
}
