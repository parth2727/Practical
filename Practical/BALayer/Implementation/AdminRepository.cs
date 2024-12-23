using DALayer.ViewModels;
using DALayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BALayer.Interface;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;

namespace BALayer.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ManagementSystem _context;

        public AdminRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _context = new ManagementSystem(connectionStrings.Value.ManagementSystem);
        }

        public Response GetAllEmployees()
        {
            Response response = new Response();
            try
            {
                DataTable data = _context.ExecuteQuery("SELECT * FROM Employee");
                var employeeList = new List<EmployeeModel>();

                foreach (DataRow row in data.Rows)
                {
                    employeeList.Add(new EmployeeModel
                    {
                        EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                        Employee_Name = row["Employee_Name"].ToString(),
                        Email = row["Email"].ToString(),
                        ContactNumber = row["ContactNumber"].ToString(),
                        Emplyee_Password = row["Emplyee_Password"].ToString(),
                        LeaveBalance = Convert.ToInt32(row["LeaveBalance"])
                    });
                }

                response.IsSuccess = true;
                response.Result = employeeList;
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
                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@EmployeeId", employeeId)
                };

                DataTable data = _context.ExecuteQuery("SELECT * FROM Employee WHERE EmployeeId = @EmployeeId", parameters);

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
                        LeaveBalance = Convert.ToInt32(row["Leavebalance"])
                    };

                    response.IsSuccess = true;
                    response.Result = employee;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Employee not found" };
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public Response AddEmployee(EmployeeModel employee)
        {
            Response response = new Response();
            try
            {
                /* var existsResponse = EmployeeExists(employee.Email);
                 if (!existsResponse.IsSuccess)
                 {
                     // If the existence check fails, return the error response
                     return existsResponse;
                 }

                 if (existsResponse.Result != null && (bool)existsResponse.Result)
                 {
                     // If the employee already exists, return an error response
                     response.IsSuccess = false;
                     response.ErrorMessages = new List<string> { "Employee already exists with the same email" };
                     return response;
                 }*/


                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@Employee_Name", employee.Employee_Name),
                    new NpgsqlParameter("@Email", employee.Email),
                    new NpgsqlParameter("@ContactNumber",employee.ContactNumber),
                    new NpgsqlParameter("@Emplyee_Password", employee.Emplyee_Password),
                };

                string query = "INSERT INTO Employee (Employee_Name, Email, Emplyee_Password,IsAdmin) VALUES (@Employee_Name, @Email, @Emplyee_Password,false)";
                int rows = _context.ExecuteNonQuery(query, parameters, false);

                if (rows > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Failed to add employee" };
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public Response UpdateEmployee(EmployeeModel employee)
        {
            Response response = new Response();
            try

            {
                /* var existsResponse = EmployeeExists(employee.EmployeeId, employee.Email);
                 if (!existsResponse.IsSuccess)
                 {
                     // If the existence check fails, return the error response
                     return existsResponse;
                 }

                 if (existsResponse.Result != null && (bool)existsResponse.Result)
                 {
                     // If another employee with the same email exists, return an error response
                     response.IsSuccess = false;
                     response.ErrorMessages = new List<string> { "Another employee already exists with the same email" };
                     return response;
                 }*/
                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@EmployeeId", employee.EmployeeId),
                    new NpgsqlParameter("@Employee_Name", employee.Employee_Name),
                    new NpgsqlParameter("@Email", employee.Email),
                    new NpgsqlParameter("@ContactNumber", employee.ContactNumber),
                    new NpgsqlParameter("@Emplyee_Password", employee.Emplyee_Password),
                    new NpgsqlParameter("@LeaveBalance", employee.LeaveBalance)
                };

                string query = "UPDATE Employee SET Employee_Name = @Employee_Name, Email = @Email,ContactNumber=@ContactNumber, Emplyee_Password = @Emplyee_Password, LeaveBalance = @LeaveBalance WHERE EmployeeId = @EmployeeId";
                int rows = _context.ExecuteNonQuery(query, parameters, false);

                if (rows > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Failed to update employee" };
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
        /* public Response EmployeeExists(int employeeId, string email)
         {
             Response response = new Response();
             try
             {
                 // Query to check if an employee exists with the same email but a different employee ID
                 var data = _context.ExecuteQuery("SELECT * FROM Employee WHERE EmployeeId != @EmployeeId AND Email = @Email",
                     new NpgsqlParameter[]
                     {
                 new NpgsqlParameter("@EmployeeId", employeeId),
                 new NpgsqlParameter("@Email", email)
                     });

                 // Set the result based on whether any rows are returned
                 response.IsSuccess = true;
                 response.Result = data.Rows.Count > 0; // Returns true if employee exists, false otherwise
             }
             catch (Exception ex)
             {
                 response.IsSuccess = false;
                 response.ErrorMessages = new List<string> { ex.Message };
             }

             return response;
         }*/


        /*  public Response EmployeeExists(string email)
          {
              Response response = new Response();
              try
              {
                  // Query to check if an employee with the specified email exists
                  var data = _context.ExecuteQuery("SELECT * FROM Employee WHERE Email = @Email",
                      new NpgsqlParameter[] { new NpgsqlParameter("@Email", email) });

                  // Set the result based on whether any rows are returned
                  response.IsSuccess = true;
                  response.Result = data.Rows.Count > 0; // Returns true if employee exists, false otherwise
              }
              catch (Exception ex)
              {
                  response.IsSuccess = false;
                  response.ErrorMessages = new List<string> { ex.Message };
              }

              return response;

          }*/
        public Response DeleteEmployee(int employeeId)
        {
            Response response = new Response();
            try
            {
                NpgsqlParameter[] parameters = {

                    new NpgsqlParameter("@EmployeeId", employeeId)
                };

                string query = "DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                int rows = _context.ExecuteNonQuery(query, parameters, false);

                if (rows > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Failed to delete employee or employee not found" };
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
        public Response GetPendingRequests()
        {
            Response response = new Response();
            try
            {
                string query = @"
            SELECT EmployeeId, Employee_Name, RequestStatus, LeaveBalance
            FROM Employee
            WHERE RequestStatus = TRUE";

                DataTable data = _context.ExecuteQuery(query);
                var employees = new List<EmployeeModel>();

                foreach (DataRow row in data.Rows)
                {
                    employees.Add(new EmployeeModel
                    {
                        EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                        Employee_Name = row["Employee_Name"].ToString(),
                        RequestStatus = Convert.ToBoolean(row["RequestStatus"]),
                        LeaveBalance = Convert.ToInt32(row["LeaveBalance"])
                    });
                }

                response.IsSuccess = true;
                response.Result = employees;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public Response UpdateLeaveRequestStatus(int employeeId, bool isApproved)
        {
            Response response = new Response();
            try
            {
                string query;
                if (isApproved)
                {
                    query = @"
                UPDATE Employee
                SET RequestStatus = FALSE, LeaveBalance = LeaveBalance - 1
                WHERE EmployeeId = @EmployeeId AND LeaveBalance > 0";
                }
                else
                {
                    query = @"
                UPDATE Employee
                SET RequestStatus = FALSE
                WHERE EmployeeId = @EmployeeId";
                }

                NpgsqlParameter[] parameters = {
            new NpgsqlParameter("@EmployeeId", employeeId)
        };

                int rows = _context.ExecuteNonQuery(query, parameters, false);

                response.IsSuccess = rows > 0;
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
