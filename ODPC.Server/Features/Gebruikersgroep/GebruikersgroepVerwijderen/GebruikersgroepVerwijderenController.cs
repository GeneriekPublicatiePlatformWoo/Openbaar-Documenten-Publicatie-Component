﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepVerwijderen
{
    [ApiController]
    public class GebruikersgroepVerwijderenController(OdpcDbContext context) : ControllerBase
    {
        [HttpDelete("api/gebruikersgroepen/{uuid:guid}")]
        public async Task<IActionResult> Delete(Guid uuid, CancellationToken token)
        {
            await context.Gebruikersgroepen.Where(x => x.Uuid == uuid)
                .ExecuteDeleteAsync(token);
            return NoContent();
        }
    }
}