using DALayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.Interface
{
    public interface IAuthRepository
    {
        Task<Response> ValidateUserLogin(string email, string password);
    }
}
