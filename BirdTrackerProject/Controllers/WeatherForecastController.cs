using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BirdTrackerProject.Controllers
{
    //TODO DELETE
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private BirdTrackerMSSQLContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,BirdTrackerMSSQLContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
           
            var Bird = new Bird()
            {
                Id = 3,
                Species = "Amsel",
                Adress = "Koenigsstrasse 12",
                Plz = 76131,
                NestDate = new DateTime(),
                Temperature = 20,
                NumberChicks = 2,
                BoxKind = "tree",
                Compass = "north"
            };
            _context.Birds.Add(Bird);
            _context.SaveChanges();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
