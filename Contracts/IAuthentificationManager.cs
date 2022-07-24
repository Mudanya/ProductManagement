using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthentificationManager
    {
        Task<bool> ValidateUser(UserForAuthenticationDto authenticationDto);
        Task<string> CreateToken();
    }
}
