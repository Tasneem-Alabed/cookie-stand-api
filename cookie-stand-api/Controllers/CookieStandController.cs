using cookie_stand_api.Model;
using cookie_stand_api.Model.DTO;
using cookie_stand_api.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cookie_stand_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CookieStandController : ControllerBase
    {
        private readonly ICookieStand _cookie;

        public CookieStandController(ICookieStand cookieStand)
        {
             _cookie = cookieStand;
        }
        // GET: api/<cookiestand>
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<CookieStandDTO>>> Get()
        {
            var cookieStands = await _cookie.GetAllCookieStands();
            if (cookieStands.Count == 0)
            {
                return NoContent();
            }
            else
                return cookieStands;
        }

        // GET api/<cookiestand>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CookieStandDTO>> Get(int id)
        {
            var cookieStand = await _cookie.GetCookieStandById(id);
            if (cookieStand != null)
                return cookieStand;
            else
                return NoContent();
        }

        // POST api/<cookiestand>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CookieStand>> Post([FromBody] CookieStandPostDTO cookieStand)
        {
            var cookieStandPost = await _cookie.AddCookieStand(cookieStand);
            if (cookieStandPost != null)
            {
                return cookieStandPost;
            }
            else
                return NoContent();
        }

        // PUT api/<cookiestand>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CookieStand>> Put(int id, [FromBody] CookieStandPostDTO cookieStand)
        {
            var cookieStandToUpdate = await _cookie.UpdateCookieStand(id, cookieStand);

            if (cookieStandToUpdate != null)
            {
                return cookieStandToUpdate;
            }
            else
            {
                return NoContent(); 
            }
        }

        // DELETE api/<cookiestand>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _cookie.DeleteCookieStand(id);
            return Ok();
        }
    }
}
