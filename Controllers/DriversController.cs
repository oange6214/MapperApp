using AutoMapper;
using MapperApp.Models;
using MapperApp.Models.DTOs.Incoming;
using MapperApp.Models.DTOs.Outgoing;
using Microsoft.AspNetCore.Mvc;

namespace MapperApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMapper _mapper;

    // In-memory
    private static List<Driver> drivers = new();


    public DriversController(ILogger<WeatherForecastController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    // Get all drivers
    [HttpGet]
    public IActionResult GetDrivers()
    {
        var allDrivers = drivers.Where(x => x.Status == 1).ToList();

        var driver = _mapper.Map<IEnumerable<DriverDto>>(allDrivers);
        return Ok(driver);
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
    public IActionResult CreateDriver(DriverForCreationDto data)
    {
        if (ModelState.IsValid)
        {
            var _driver = _mapper.Map<Driver>(data);

            drivers.Add(_driver);

            var newDriver = _mapper.Map<DriverDto>(_driver);
            return CreatedAtAction("GetDriver", new { _driver.Id }, newDriver);
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