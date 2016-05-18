using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Microsoft.AspNet.Authorization;
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
                return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);

            String userName = User.GetUserName();
            String userId = User.GetUserId();

            User user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user == null)
                return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);

            return new JsonResult(GetUser(user));
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(String email, String password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

            if (result == SignInResult.Success)
                return new HttpStatusCodeResult(200);
            else
                return new HttpStatusCodeResult(401);
        }

        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return new HttpStatusCodeResult(200);
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
