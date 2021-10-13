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
            CreateMap<UsuariosModel, RegistrationModel>();
            CreateMap<RegistrationModel, UsuariosModel>();
            CreateMap<UsuariosModel, Usuario>();
            CreateMap<Usuario, UsuariosModel>();
            CreateMap<UserInfo, UsuariosModel>();
            CreateMap<UsuariosModel, UserInfo>();
            CreateMap<CentrosEducativosSelectModel, CentrosEducativosModel>();
        }
    }
}
