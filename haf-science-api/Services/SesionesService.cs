using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class SesionesService : ISessionService<SesionesModel, PaginatedSesionesView>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public SesionesService(HafScienceDbContext dbContext, ILogger<SesionesService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        
    }
}
