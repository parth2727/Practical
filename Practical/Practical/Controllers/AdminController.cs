using BALayer.Interface;
using DALayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Practical.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // GET: Admin/Index
        public ActionResult Index()
        {
          /*  var response = _adminRepository.GetAllEmployees();
            if (!response.IsSuccess)
            {
                return View("Error", response.ErrorMessages);
            }
            return View(response.Result);*/
            Response trip = _adminRepository.GetAllEmployees();
            if (trip == null)
            {
                // Handle case when the trip is not found
                return RedirectToAction("Error", new { message = "Trip not found" });
            }
            return View(trip.Result);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            Response response = _adminRepository.GetEmployeeById(id);
            if (!response.IsSuccess)
            {
                return View("Error", response.ErrorMessages);
            }
            return View(response.Result);
        }

        // GET: Admin/Add
        public ActionResult Add()
        {
            return View(); // Return empty form for creating an employee
        }

        // POST: Admin/Add
        [HttpPost]
        public ActionResult Add(EmployeeModel employee)
        {
            Response response = _adminRepository.AddEmployee(employee);
            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessages = response.ErrorMessages;
                return View("Error");
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            Response response = _adminRepository.GetEmployeeById(id);
            if (!response.IsSuccess)
            {
                return View("Error", response.ErrorMessages);
            }
            return View(response.Result);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeModel employee)
        {
            Response response = _adminRepository.UpdateEmployee(employee);
            if (!response.IsSuccess)
            {
                ViewBag.ErrorMessages = response.ErrorMessages;
                return View("Error");
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            _adminRepository.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Response response = _adminRepository.DeleteEmployee(id);
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");  // Redirect to the Index view after successful deletion
                }
                ViewBag.ErrorMessages = response.ErrorMessages;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessages = new[] { ex.Message };
                return View("Error");
            }
        }
        public IActionResult ManageRequests()
        {
            var response = _adminRepository.GetPendingRequests();
            if (!response.IsSuccess)
            {
                TempData["ErrorMessage"] = "Failed to load leave requests.";
                return View(new List<EmployeeModel>());
            }

            return View(response.Result);
        }
        [HttpPost]
        public IActionResult UpdateLeaveRequestStatus(int employeeId, bool isApproved)
        {
            var response = _adminRepository.UpdateLeaveRequestStatus(employeeId, isApproved);

            if (response.IsSuccess)
                TempData["SuccessMessage"] = "Request updated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to update request.";

            return RedirectToAction("ManageRequests");
        }
    }
}
