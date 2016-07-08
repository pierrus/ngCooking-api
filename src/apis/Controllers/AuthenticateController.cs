using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using apis.Models;

namespace apis.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticateController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        NgContext _context;

        public AuthenticateController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            NgContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoggedInStatus()
        {
            if (!User.Identity.IsAuthenticated)
                return new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);

            String userName = _userManager.GetUserName(User);
            String userId = _userManager.GetUserId(User);

            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user == null)
                return new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

            return new JsonResult(GetUser(user));
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(String email, String password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

            if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                return new StatusCodeResult(200);
            else
                return new StatusCodeResult(401);
        }

        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return new StatusCodeResult(200);
        }

        private User GetUser(User user)
        {
            return new User
            {
                Bio = user.Bio,
                BirthYear = user.BirthYear,
                City = user.City,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                Level = user.Level,
                Picture = user.Picture
            };
        }
    }
}
