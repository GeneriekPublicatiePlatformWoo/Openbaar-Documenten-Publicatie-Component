﻿using Microsoft.AspNetCore.Mvc;

namespace ODPC.Features.Publicaties.PublicatieRegistreren
{
    [ApiController]
    public class PublicatieRegistrerenController : ControllerBase
    {
        [HttpPost("api/v1/publicaties")]
        public IActionResult Post(Publicatie publicatie)
        {
            publicatie.Uuid = Guid.NewGuid();
            publicatie.Creatiedatum = DateOnly.FromDateTime(DateTime.Now);
            PublicatiesMock.Publicaties[publicatie.Uuid] = publicatie;
            return Ok(publicatie);
        }
    }
}
