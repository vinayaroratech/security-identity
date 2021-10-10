using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using VA.Security.API.Config;
using VA.Security.Identity.Jwt;
using VA.Security.Identity.Jwt.Model;
using VA.Security.Identity.Model;

namespace VA.Security.API.Controllers
{
    [Route("api/custom-account")]
    public class AuthCustomConfigController : MainController
    {
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly AppJwtSettings _appJwtSettings;

        public AuthCustomConfigController(SignInManager<MyIdentityUser> signInManager,
            UserManager<MyIdentityUser> userManager,
            IOptions<AppJwtSettings> appJwtSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appJwtSettings = appJwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            MyIdentityUser user = new()
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                return CustomResponse(GetUserResponse(user.Email));
            }

            foreach (IdentityError error in result.Errors)
            {
                AddError(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUser loginUser)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                /* ANOTHER OPTIONS */
                UserResponse<string> userResponse = GetUserResponse(loginUser.Email);
                string jwtUserClaims = GetJwtWithUserClaims(loginUser.Email);
                string jwtNoClaims = GetJwtWithoutClaims(loginUser.Email);

                string fullJwt = GetFullJwt(loginUser.Email);
                return CustomResponse(fullJwt);
            }

            if (result.IsLockedOut)
            {
                AddError("This user is blocked");
                return CustomResponse();
            }

            AddError("Incorrect user or password");
            return CustomResponse();
        }

        private UserResponse<string> GetUserResponse(string email)
        {
            return new JwtBuilder<MyIdentityUser>()
                .WithUserManager(_userManager)
                .WithJwtSettings(_appJwtSettings)
                .WithEmail(email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .BuildUserResponse();
        }

        private string GetFullJwt(string email)
        {
            return new JwtBuilder<MyIdentityUser>()
                .WithUserManager(_userManager)
                .WithJwtSettings(_appJwtSettings)
                .WithEmail(email)
                .WithJwtClaims()
                .WithUserClaims()
                .WithUserRoles()
                .BuildToken();
        }

        private string GetJwtWithoutClaims(string email)
        {
            return new JwtBuilder<MyIdentityUser>()
                .WithUserManager(_userManager)
                .WithJwtSettings(_appJwtSettings)
                .WithEmail(email)
                .BuildToken();
        }

        private string GetJwtWithUserClaims(string email)
        {
            return new JwtBuilder<MyIdentityUser>()
                .WithUserManager(_userManager)
                .WithJwtSettings(_appJwtSettings)
                .WithEmail(email)
                .WithUserClaims()
                .BuildToken();
        }
    }
}