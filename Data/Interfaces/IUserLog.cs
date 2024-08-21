using Data.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserLog
    {
        Task<string> LoginAsync(LoginDTO login);

        Task<string> RegisterAsync(RegisterDTO register);

    }
}
