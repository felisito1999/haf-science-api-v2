using AutoMapper;
using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsuarioModel, RegistrationModel>();
            CreateMap<RegistrationModel, UsuarioModel>();
            CreateMap<UsuarioModel, Usuario>();
            CreateMap<Usuario, UsuarioModel>();
        }
    }
}
