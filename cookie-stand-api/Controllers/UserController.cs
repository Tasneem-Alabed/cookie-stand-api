using cookie_stand_api.Model;
using cookie_stand_api.Model.DTO;
using cookie_stand_api.Model.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace cookie_stand_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _user;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUser user, UserManager<ApplicationUser> userMaanger)
        {
            _user = user;
            _userManager = userMaanger;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var user = await _user.SignUp(registerDTO, this.ModelState, User);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _user.SignIn(loginDTO.Username, loginDTO.Password, this.ModelState);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return Unauthorized();
                }
            }
            else { return BadRequest(new ValidationProblemDetails(ModelState));}
        }
    }
}
