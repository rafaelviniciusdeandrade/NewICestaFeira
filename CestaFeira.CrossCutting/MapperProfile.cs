﻿
using AutoMapper;
using CestaFeira.CrossCutting.Mappings.Produto;
using CestaFeira.CrossCutting.Mappings.Usuario;

namespace CestaFeira.CrossCutting
{
    public static class MapperProfile
    {
        public static MapperConfiguration Configure()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UsuarioProfile());
                cfg.AddProfile(new ProdutoProfile());
            });

            return config;
        }
    }
}