using cookie_stand_api.Data;
using cookie_stand_api.Model.DTO;
using cookie_stand_api.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace cookie_stand_api.Model.Services
{
    public class ApplicationUserService : IUser
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly JWTTokenService _jwtTokenService;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, JWTTokenService jwtTokenService )
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;

        }

        public async Task<UserDTO> SignUp(RegisterDTO registerDTO, ModelStateDictionary modelState, ClaimsPrincipal claims)
        {
            var user = new ApplicationUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                return new UserDTO
                {
                    ID = user.Id,
                    Name = user.UserName,
                    Token = await _jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1))
                };
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    var errorMessage = error.Code.Contains("Password") ? nameof(registerDTO.Password) :
                                         error.Code.Contains("Email") ? nameof(registerDTO.Email) :
                                           error.Code.Contains("Username") ? nameof(registerDTO.Username) :
                                           "";
                    modelState.AddModelError(errorMessage, error.Description);
                };
                return null;
            }
        }

        public async Task<UserDTO> GetUser(ClaimsPrincipal claims)
        {
            var user = await _userManager.GetUserAsync(claims);

            return new UserDTO
            {
                ID = user.Id,
                Name = user.UserName,
                Token = await _jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1))

            };
        }

        public async Task<UserDTO> SignIn(string username, string password, ModelStateDictionary modelState)
        {
            var user = await _userManager.FindByNameAsync(username);

            bool isVaild = await _userManager.CheckPasswordAsync(user, password);
            if (isVaild)
            {
                return new UserDTO
                {
                    ID = user.Id,
                    Name = user.UserName,
                    Token = await _jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1))

                };
            }
            else
                modelState.AddModelError(string.Empty, "userName Or Password Wrong");
                return null;

        }

    }
}
