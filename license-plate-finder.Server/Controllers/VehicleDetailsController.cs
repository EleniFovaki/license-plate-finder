using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using license_plate_finder.Server.Models;
using license_plate_finder.Server.Services;
using System.Net.Http;
using System.Text.Json;


namespace license_plate_finder.Server.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleDetailsController : ControllerBase
    {
        private readonly VehicleDetailsService _vehicleService;

        public VehicleDetailsController(VehicleDetailsService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> GetVehicleDetails(string licensePlate)
        {
            var vehicle = await _vehicleService.GetVehicleDetailsAsync(licensePlate);

            if (vehicle == null)
                return NotFound("Vehicle not found");

            return Ok(vehicle);
        }
    }
}
