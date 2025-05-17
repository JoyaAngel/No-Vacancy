using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;

namespace NoVacancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CarritoCabeceraController : ControllerBase
    {
        readonly NoVacancyDbContex _context;
        public CarritoCabeceraController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: api/CarritosCabecera
        [HttpGet]
        public IActionResult GetCarritosCabecera()
        {
            var carritosCabecera = _context.CarritosCabecera.ToList();
            return Ok(carritosCabecera);
        }

        // POST: api/CarritosCabecera
        [HttpPost]
        public IActionResult Create([FromBody] CarritoCabecera carritoCabecera)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _context.CarritosCabecera.Add(carritoCabecera);
            _context.SaveChanges();
            return Ok(carritoCabecera);
        }
    }
}
