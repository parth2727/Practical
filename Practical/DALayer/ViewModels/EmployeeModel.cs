using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.ViewModels
{
    public class EmployeeModel
    {
        [DisplayName("Employee id")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Name is required.")]
        public string Employee_Name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact number is required.")]
        public string ContactNumber {  get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Emplyee_Password { get; set; }
        public int LeaveBalance { get;  set; }
        public bool IsAdmin { get; set; }
        public bool RequestStatus { get; set; } // New field for leave request status
      //  public DateTime? RequestDate { get; set; } 

    }
}
