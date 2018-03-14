using System;
using System.Diagnostics;
using System.Reflection;
using TfhkaNet.IF.PA;

namespace OutOfPaper.Drivers.Commands.ApiCommands
{
    public class TallyDascon1125Commands : ICommandable
    {
        private Tfhka printer;
        #region commands string values
        private const string LOGIN_CACHIER = "5";
        private const string LOGOUT_CACHIER = "6";
        private const string COSTUMER_RUC = "JR";
        private const string COSTUMER_SOCIAL_REASON = "JS";
        private const string COSTUMER_ADDITIOAL_INFO = "J";
        private const string COMENT = "@";
        
        #endregion

        TallyDascon1125Commands()
        {
            printer = new Tfhka();
        }

        public void LoginCachier(int cachierPassword)
        {
            var command = LOGIN_CACHIER + cachierPassword;
            Execute(command);
        }

        public void LogoutCachier()
        {
            Execute(LOGOUT_CACHIER);
        }

        public void OpenPrinter(string port)
        {
            Execute(port);
        }        

        public void SetCostumerRuc(string ruc)
        {
            var command = COSTUMER_RUC + ruc;
            Execute(command);
        }

        public void SetCostumerSocialReason(string reason)
        {
            var command = COSTUMER_SOCIAL_REASON + reason;
            Execute(command);
        }

        public void SetCostumerAdditionalInfo(int linesAmout, string info)
        {
            var command = COSTUMER_ADDITIOAL_INFO + linesAmout +  info;
            Execute(command);
        }

        public void AddComent(string coment)
        {
            var command = COMENT + coment;
            Execute(command);
        }

        private void Execute(string command)
        {
            var result = printer.SendCmd(command);
            if (!result)
            {
                var methodName = GetMethodName();
                throw new Exception($"Failed Command: {methodName}: {command}");
            }
        }

        private string GetMethodName()
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase methodBase = stackTrace.GetFrame(2).GetMethod();
            return methodBase.Name;
        }

        public void AddItems(string taxRate, decimal price, int quantity, string description)
        {
            var command = taxRate + price + quantity + description;
            Execute(command);
        }
    }
}
