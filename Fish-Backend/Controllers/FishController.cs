using Fish_Backend.Data;
using Fish_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Fish_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishController : ControllerBase
    {
        private readonly AppDbContext context;

        public FishController(AppDbContext context) => this.context = context;

        [HttpGet("GetListFish")]
        public async Task<IActionResult> GetListFish(int idUser)
        {
            var fishList = await context.Fishes
                .Where(f => f.owner_id == idUser)
                .ToListAsync();
            // đóng gói lại ... check lại phần này
            return Ok(new 
            {    
                success = true,
                fishes = fishList 
            });
        }
        [HttpPost("AddFish")]
        public async Task<IActionResult> AddFish(string name, int ownerID, int level)
        {
            Fish newfish = new Fish
            {
                name = name,
                owner_id = ownerID,
                level = level,
                hunger_time = 0,
                pos_x = 0, pos_y = 0, pos_z = 0
            };
            context.Fishes.Add(newfish);
            await context.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                id = newfish.id
            });
        }
        [HttpPut("UpdateFish")]
        public async Task<IActionResult> UpdateFish(int id,int level, float hungerTime)
        {
            var fish = await context.Fishes.FindAsync(id);
            if (fish == null)
            {
                return BadRequest("Cá không tồn tại");
            }
            try
            {
                fish.level = level;
                fish.hunger_time = hungerTime;
        
                await context.SaveChangesAsync();
                return Ok(new
                {
                    success=true,
                    message = "Lưu lại dữ liệu cá"
                });
            }
            catch
            {
                return StatusCode(500, "Lỗi");
            }
        }
        [HttpDelete("RemoveFish")]
        public async Task<IActionResult> RemoveFish(int id)
        {
            //var fish = await context.Fishes
            //    .FirstOrDefaultAsync(i => i.id == id);
            var fish = await context.Fishes.FindAsync(id); // dùng cho primary key vì chạy nhanh hơn
            if (fish == null)
            {
                return NotFound(new
                {
                    message = "Lỗi API",
                    success = false,
                });
            }
            context.Fishes.Remove(fish);
            await context.SaveChangesAsync();
            return Ok(new
            {
                message = "Đã xoá",
                success = true,
            });
        }

        [HttpGet("GetAFish")]
        public async Task<IActionResult> GetAFish(int fishID)
        {
            var fish = await context.Fishes.FindAsync(fishID); 
            if (fish == null)
            {
                return NotFound("Không tìm thấy cá");
            }
            else
            {
                return Ok(new
                {
                    id = fishID,
                    name = fish.name,
                    level = fish.level
                });

            }
        }

    }

  
}
