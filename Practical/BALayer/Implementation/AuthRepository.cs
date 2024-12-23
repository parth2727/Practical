using DALayer.ViewModels;
using DALayer;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BALayer.Interface;

namespace BALayer.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ManagementSystem _context;

        public AuthRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _context = new ManagementSystem(connectionStrings.Value.ManagementSystem);
        }

        public async Task<Response> ValidateUserLogin(string email, string password)
        {
            Response response = new Response();
            try
            {
                NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@Email", email),
                    new NpgsqlParameter("@Emplyee_Password", password)
                };

                // Query to check if the email and password match
                string query = "SELECT * FROM Employee WHERE Email = @Email AND Emplyee_Password = @Emplyee_Password";
                DataTable data = _context.ExecuteQuery(query, parameters);

                if (data != null && data.Rows.Count > 0)
                {
                    // Login successful
                    DataRow row = data.Rows[0];
                    var employee = new EmployeeModel
                    {
                        EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                        Employee_Name = row["Employee_Name"].ToString(),
                        Email = row["Email"].ToString(),
                        Emplyee_Password = row["Emplyee_Password"].ToString(),
                        LeaveBalance = Convert.ToInt32(row["Leavebalance"]),
                        ContactNumber = row["ContactNumber"].ToString(),
                        IsAdmin = Convert.ToBoolean(row["IsAdmin"])
                    };
                    response.IsSuccess = true;
                    response.Result = employee;
                } 
                else
                {
                    // Invalid credentials
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Invalid email or password" };
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
