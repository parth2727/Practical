using BALayer.Implementation;
using BALayer.Interface;
using DALayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Practical.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJWTService _jwtService;

        public EmployeeController(IEmployeeRepository employeeRepository,IJWTService jWTService)
        {
            _employeeRepository = employeeRepository;
            _jwtService= jWTService;
        }

        public IActionResult Index()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");

            // If EmployeeId is null or 0, redirect to login
            if (employeeId == null || employeeId == 0)
            {
                TempData["ErrorMessage"] = "You must be logged in to view your details.";
                return RedirectToAction("Login", "Auth");
            }

            // Fetch the employee details
            Response trip = _employeeRepository.GetEmployeeById(employeeId.Value);

            if (trip.IsSuccess)
            {
                // Pass employee data to the view
                return View(trip.Result);
            }
            else
            {
                // Handle the case where the employee data wasn't found
                TempData["ErrorMessage"] = "Failed to fetch employee details.";
                return RedirectToAction("Error", "Home");
            }
        }

        

        [HttpPost]
        public IActionResult SubmitLeaveRequest()
        {

            // var employeeId = HttpContext.Session.GetInt32("EmployeeId");
            var employeeId = _jwtService.GetUserIdFromToken(HttpContext.Session.GetString("JWTToken"));

            if (employeeId == null || employeeId == 0)
            {
                TempData["ErrorMessage"] = "You must be logged in to submit a leave request.";
                return RedirectToAction("Login", "Auth");
            }

            var response = _employeeRepository.SubmitLeaveRequest(employeeId);

            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Leave request submitted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to submit leave request: " + string.Join(", ", response.ErrorMessages);
            }

            return RedirectToAction("RequestLeave");
        }

    }
}