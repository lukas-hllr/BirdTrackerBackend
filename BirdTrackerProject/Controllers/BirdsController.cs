using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BirdTrackerProject;
using BirdTrackerProject.DTO;
using System.Globalization;

namespace BirdTrackerProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BirdsController : ControllerBase
    {
        private readonly BirdTrackerMSSQLContext _context;

        public BirdsController(BirdTrackerMSSQLContext context)
        {
            _context = context;
        }

        // GET: Birds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bird>>> GetBirds()
        {
            return await _context.Birds.ToListAsync();
        }
        //GET: Birds/Amsel
        [HttpGet("{species}")]
        public async Task<ActionResult<IEnumerable<Bird>>> GetBirdsBySpecies(string species)
        {
            return await _context.Birds.Where(b=>b.Species==species).ToListAsync();
        }

        // GET: Birds/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Bird>> GetBird(int id)
        {
            var bird = await _context.Birds.FindAsync(id);

            if (bird == null)
            {
                return NotFound();
            }

            return bird;
        }
        // GET: Birds/Coordinates/xMin+yMax+xMax+yMin(+Birdspecies)
        // returns coordinates of all birds in the given rectangle, if a bird species is specified it only returns coordinates of that species.
        // Coordinates have to be in decimal format and seperated by a "+"
        [HttpGet("Coordinates/{coords}")]
        public ActionResult<List<CoordsResponseDTO>> GetHeatMapCoords(string coords)
        {
            List<CoordsResponseDTO> response = new List<CoordsResponseDTO>();
            string[] coordsList = coords.Split("+");
            decimal xMin = decimal.Parse(coordsList[0], CultureInfo.InvariantCulture);
            decimal yMax = decimal.Parse(coordsList[1], CultureInfo.InvariantCulture);
            decimal xMax = decimal.Parse(coordsList[2], CultureInfo.InvariantCulture);
            decimal yMin = decimal.Parse(coordsList[3], CultureInfo.InvariantCulture);
            List<Bird> birds = new List<Bird>();
            if (coordsList.Length == 4)
            {
                birds = _context.Birds.Where(b => b.Latitude < yMax && b.Latitude > yMin && b.Longitude < xMax && b.Longitude > xMin).ToList();
            }
            else
            if (coordsList.Length == 5)
            {
                birds = _context.Birds.Where(b => b.Latitude < yMax && b.Latitude > yMin && b.Longitude < xMax && b.Longitude > xMin && b.Species == coordsList[4]).ToList();
            }
            else { return BadRequest(); }
            foreach (var element in birds)
            {
                response.Add(new CoordsResponseDTO() { Latitude = element.Latitude, Longitude = element.Longitude });
            }
            return response;
        }

        // PUT: Birds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBird(int id, Bird bird)
        {
            if (id != bird.Id)
            {
                return BadRequest();
            }

            _context.Entry(bird).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BirdExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Birds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bird>> PostBird(Bird bird)
        {
            int maxId = _context.Birds.Max(b => b.Id);
            bird.Id = maxId + 1;
            //bird.Latitude = Decimal.Parse(bird.Latitude + "");
            //bird.Longitude = Decimal.Parse(bird.Longitude + "");

            _context.Birds.Add(bird);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BirdExists(bird.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBird", new { id = bird.Id }, bird);
        }

        // DELETE: Birds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBird(int id)
        {
            var bird = await _context.Birds.FindAsync(id);
            if (bird == null)
            {
                return NotFound();
            }

            _context.Birds.Remove(bird);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BirdExists(int id)
        {
            return _context.Birds.Any(e => e.Id == id);
        }
    }
}
