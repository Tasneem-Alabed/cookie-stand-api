using cookie_stand_api.Model.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace cookie_stand_api.Model.Interfaces
{
    public interface IUser
    {
        public Task<UserDTO> SignUp(RegisterDTO registerDTO,ModelStateDictionary modelState, ClaimsPrincipal claims);

        public Task<UserDTO> SignIn(string username,string password,ModelStateDictionary modelState);

        public Task<UserDTO> GetUser(ClaimsPrincipal claims);

    }
}
