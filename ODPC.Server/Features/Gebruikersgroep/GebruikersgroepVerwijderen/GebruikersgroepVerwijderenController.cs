﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Authentication;
using ODPC.Data;

namespace ODPC.Features.Gebruikersgroep.GebruikersgroepVerwijderen
{
    [ApiController]
    [Authorize(AdminPolicy.Name)]
    public class GebruikersgroepVerwijderenController(OdpcDbContext context) : ControllerBase
    {
        [HttpDelete("api/v1/gebruikersgroepen/{uuid:guid}")]
        public async Task<IActionResult> Delete(Guid uuid, CancellationToken token)
        {
            await context.Gebruikersgroepen.Where(x => x.Uuid == uuid)
                .ExecuteDeleteAsync(token);
            return NoContent();
        }
    }
}
