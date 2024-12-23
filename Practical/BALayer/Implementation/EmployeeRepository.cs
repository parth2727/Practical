using BALayer.Interface;
using DALayer;
using DALayer.ViewModels;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.Implementation
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly ManagementSystem _context;

        public EmployeeRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _context = new ManagementSystem(connectionStrings.Value.ManagementSystem);
        }
        public Response SubmitLeaveRequest(int employeeId)
        {
            Response response = new Response();
            try
            {
                NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@EmployeeId", employeeId),
            new NpgsqlParameter("@RequestStatus", true)
        };

                string query = @"
                 UPDATE Employee 
                 SET RequestStatus = @RequestStatus
                 WHERE EmployeeId = @EmployeeId ";

                int rows = _context.ExecuteNonQuery(query, parameters, false);

                if (rows > 0)
                {
                    response.IsSuccess = true;
                    response.Result = "Leave request submitted successfully.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Cannot submit another leave request while one is pending." };
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
        public Response GetEmployeeById(int employeeId)
        { 
            Response response = new Response();
            try
            {
                string query = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
                NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@EmployeeId", employeeId)
            };

                DataTable data = _context.ExecuteQuery(query, parameters);
                if (data.Rows.Count > 0)
                {
                    DataRow row = data.Rows[0];
                    var employee = new EmployeeModel
                    {
                        EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                        Employee_Name = row["Employee_Name"].ToString(),
                        Email = row["Email"].ToString(),
                        ContactNumber = row["ContactNumber"].ToString(),
                        Emplyee_Password = row["Emplyee_Password"].ToString(),
                        LeaveBalance = Convert.ToInt32(row["LeaveBalance"]),
                        RequestStatus = row["RequestStatus"] != DBNull.Value ? Convert.ToBoolean(row["RequestStatus"]) : false
                    };

                    response.IsSuccess = true;
                    response.Result = employee;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Employee not found." };
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
    }
}
