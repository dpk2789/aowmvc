using AowCore.AppWeb.ViewModels;
using AowCore.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AowCore.AppWeb.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UsersController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users;
            var viewModelList = new List<ApplicationUserViewModel>();
            foreach (var user in users)
            {
                var viewModel = new ApplicationUserViewModel();
                viewModel.Email = user.Email;
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }
    }
}
