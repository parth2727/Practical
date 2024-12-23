using DALayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.Interface
{
    public interface IAdminRepository
    {
        Response GetAllEmployees();
        Response GetEmployeeById(int employeeId);
        Response AddEmployee(EmployeeModel employee);
        Response UpdateEmployee(EmployeeModel employee);
        Response DeleteEmployee(int employeeId);
        Response GetPendingRequests();
        Response UpdateLeaveRequestStatus(int employeeId, bool isApproved);
        /* Response EmployeeExists(string email);
         Response EmployeeExists(int employeeId, string email);*/
        /*   Response GetAll();
          Response GetById(int employeeId);
          Response Add(EmployeeModel employee);
          Response Edit(EmployeeModel employee);
          Response Delete(int employeeId); */
    }
    
}
