using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoomService.WebAPI.Data;
using RoomService.WebAPI.Services;

namespace RoomService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomsService _roomsService;

        public RoomsController(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = (await _roomsService.Get(new[] { id }, null)).FirstOrDefault();
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] Filters filters)
        {
            var newsItems = await _roomsService.Get(null, filters);
            return Ok(newsItems);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Room room)
        {
            await _roomsService.Add(room);
            return Ok(room);
        }
    }
}
