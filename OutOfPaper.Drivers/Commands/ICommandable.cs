namespace OutOfPaper.Drivers.Commands
{
    interface ICommandable
    {
        void LoginCachier(string cachierPassword);
        void LogoutCachier();
        void setUpPrinterMeansOfPayment(string means, string description);
        void SetUpPrinteTime(string hours, string minutes, string seconds);
        void SetUpPrinteDate(string day, string month, string year);
        void SetUpPrinterTaxOptions(string[] taxPercentage);
        void SetUpPrinterHeader(string header);
        void SetupPrinterFooter(string footer);
        void SetupPrinterFlag(string flag, string value);
        void VerifyPrinterTaxOptions();
        void SetDocumentClientRUC(string ruc);
        void SetDocumentCostumerSocialReason(string reason);
        void SetDocumentCostumerAdditionalInfo(int linesAmout, string info);
        void AddDocumentComent(string coment);

        /*
         * Agregar items a la factura require que se registren 
         * Los diferentes modos de impuestos; Soporta 3.
         */

        void AddItemToInvoice(string taxRate, string price, string quantity, string description);
        void RemoveLastItem();
        void PrintSubtotal();
        void ShowSubtotal();
        void ApplyPercentageDiscount(decimal percentage);
        void ApplyAmountDicount(string amount);
        void RemoveItemFromInvoice(string taxRate, string price, string quantity, string description);
        void CancelInvoce();
        void SetMeansOfPayment(string meansOfPayment);
        void SetPartialMeansOfPayment(string meansOfPayment, string amount);
        void SetCreditNoteRelatedRecipt(string relatedReceipt);
        void AddItemToCreditNote(int tax, string price, string description);
        void RemoveItemFromCreditNote(int tax, string price, string description);
        void PayTotalOnCreditNote(string meansOfPayment);
        void PayAmountOnCreditNote(string meansOfPayment, string amount);        
    }
}
