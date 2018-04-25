namespace OutOfPaper.Drivers.Commands
{
    interface ICommandable
    {
        void LoginCachier(int cachierPassword);
        void LogoutCachier();
        void SetCostumerRuc(string ruc);
        void SetCostumerSocialReason(string reason);
        void SetCostumerAdditionalInfo(int linesAmout, string info);
        void AddComent(string coment);

        /*
         * Agregar items a la factura require que se registren 
         * Los diferentes modos de impuestos; Soporta 3.
         */

        void AddItems(string taxRate, decimal price, int quantity, string description);
        void RemoveLastItem();
        void PrintSubtotal();
        void ShowSubtotal();
        void ApplyPercentageDiscount(decimal percentage);
        void ApplyAmountDicount(decimal amunt);
        void CancelItem(string taxRate, decimal price, int quantity, string description);
        void CancelInvoce();
        void SetMeansOfPayment(string meansOfPayment);
        void SetPartialMeansOfPayment(string meansOfPayment, decimal amount);
        
    }
}
