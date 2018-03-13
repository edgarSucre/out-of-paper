using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfPaper.Drivers.Commands.ApiCommands
{
    class TallyDascon1125Commands : ICommandable
    {
        #region commands string values
        string LOGIN_CACHIER = "5";
        string LOGOUT_CACHIER = "6";
        #endregion

        TallyDascon1125Commands()
        {
            //Injectar instancia de impresora
        }

        public void LoginCachier(int cachierPassword)
        {
            var command = LOGIN_CACHIER + cachierPassword;
            throw new NotImplementedException();
        }

        public void LogoutCachier()
        {
            throw new NotImplementedException();
        }
    }
}
