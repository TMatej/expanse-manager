using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseManagerServiceLibrary.Services.Security
{
    public interface IPasswordService
    {
        bool ValidatePassword(string password);
        bool VerifyPassword(string hashedPassword, string rawPassword);
        string EncodePassword(string password);
    }
}
