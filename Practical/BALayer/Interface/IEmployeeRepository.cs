using DALayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.Interface
{
    public interface IEmployeeRepository
    {
        Response SubmitLeaveRequest(int employeeId);
        Response GetEmployeeById(int employeeId);
    }
}
