using BALayer.Interface;
using DALayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Practical.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJWTService _jwtService;
        public AuthController(IAuthRepository authRepository, IJWTService jWTService)
        {
            _authRepository = authRepository;
            _jwtService = jWTService;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var response = await _authRepository.ValidateUserLogin(email, password);

            if (response.IsSuccess)
            {

                // Explicitly cast response.Result to EmployeeModel
                EmployeeModel model = response.Result as EmployeeModel;

                if (model != null )
                {
                    string token = _jwtService.GenerateJwtToken(model);
                    HttpContext.Session.SetString("JWTToken", token);
                   // HttpContext.Session.SetInt32("EmployeeId", model.EmployeeId);
                    // Redirect to Admin index if the user is an Admin
                    if (model.IsAdmin)
                    {

                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        // Redirect to Employee index if the user is not an Admin
                        return RedirectToAction("Index", "Employee");
                    }
                } 
                else
                {
                    // Handle the case where response.Result is not of type EmployeeModel
                    TempData["ErrorMessage"] = "Unexpected error: User data is invalid.";
                    return View();
                }
            }
            else
            {
                // Display error message if login fails
                TempData["ErrorMessage"] = response.ErrorMessages[0];
                return View();
            }
        }
    }
}