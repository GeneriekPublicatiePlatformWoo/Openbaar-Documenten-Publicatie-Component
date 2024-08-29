using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODPC.Data;

namespace ODPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly OdpcDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(OdpcDbContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _context.WeatherForecasts.AsEnumerable();
        }
    }
}