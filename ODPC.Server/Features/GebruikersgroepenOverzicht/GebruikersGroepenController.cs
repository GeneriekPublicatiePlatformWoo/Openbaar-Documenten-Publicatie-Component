﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;
using ODPC.Features.Gebruikersgroepen;

namespace ODPC.Features.GebruikersgroepenOverzicht
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikersGroepenController(OdpcDbContext context) : ControllerBase
    {

        private readonly OdpcDbContext _context = context;

        [HttpGet]
        public IAsyncEnumerable<GebruikersgroepModel> Get()
        {
            return _context
              .Gebruikersgroepen
              .OrderBy(x => x.Name)
              .Select(x => new GebruikersgroepModel { Name = x.Name, Id = x.Id })
              .AsAsyncEnumerable();
        }
    }
}
