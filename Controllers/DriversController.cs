using MapperApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MapperApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private static List<Driver> drivers = new();

    public DriversController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // Get all drivers
    [HttpGet]
    public IActionResult GetDrivers()
    {
        var allDrivers = drivers.Where(x => x.Status == 1).ToList();
        return Ok(allDrivers);
    }

    
    [HttpGet]
    [Route("GetDriver")]
    public IActionResult GetDriver(Guid id)
    {
        var item = drivers.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateDriver(Driver data)
    {
        if (ModelState.IsValid)
        {
            drivers.Add(data);
            return CreatedAtAction("GetDriver", new { data.Id }, data);
        }

        return new JsonResult("Something went wrong") { StatusCode = 500 };
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDriver(Guid id, Driver data)
    {
        if (id == data.Id)
        {
            return BadRequest();
        }

        var existingDriver = drivers.FirstOrDefault(x => x.Id == data.Id);

        if (existingDriver == null)
        {
            return NotFound();
        }

        existingDriver.DriverNumber = data.DriverNumber;
        existingDriver.FirstName = data.FirstName;
        existingDriver.LastName = data.LastName;
        existingDriver.WorldChampionships = data.WorldChampionships;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDriver(Guid id)
    {
        var existingDriver = drivers.FirstOrDefault(x => x.Id == id);

        if (existingDriver == null)
        {
            return NotFound();
        }

        existingDriver.Status = 0;

        return NoContent();
    }
}