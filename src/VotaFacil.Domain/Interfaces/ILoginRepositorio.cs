using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotaFacil.Domain.Interfaces
{
    public interface ILoginRepositorio
    {
        Task<bool> Login(string username, string password);
        Task Logout();

    }
}
