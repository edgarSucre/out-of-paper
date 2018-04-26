using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TfhkaNet.IF.PA;

namespace OutOfPaper.Drivers.Commands.ApiCommands
{
    public class TallyDascon1125Commands : ICommandable
    {
        private Tfhka printer;
        #region commands string values
        private const string LOGIN_CACHIER_CMD = "5";
        private const string LOGOUT_CACHIER_CMD = "6";
        private const string SET_DOCUMENT_COSTUMER_RUC_CMD = "JR";
        private const string SET_DOCUMENT_COSTUMER_SOCIAL_REASON_CMD = "JS";
        private const string ADD_DOCUMENT_COSTUMER_ADDITIOAL_INFO_CMD = "J";
        private const string ADD_DOCUMENT_COMENT_CMD = "@"; // need test, this is for invoice
        // private const string COMENT_CMD = "A"; need test, this is for credit note.
        private const string INVOICE_TAX_RATE_FREE_ITEM_CMD = " ";
        private const string INVOICE_TAX_RATE_ONE_ITEM_CMD = "!";
        private const string INVOICE_TAX_RATE_TWO_ITEM_CMD = "\"";
        private const string INVOICE_TAX_RATE_THREE_ITEM_CMD = "#";
        private const string DOCUMENT_ITEM_CORRECTION_CMD = "k";
        private const string PRINTED_SUBTOTAL_CMD = "3";
        private const string SCREEN_SUBTOTAL_CMD = "4";
        private const string PERCENTAGE_DISCOUNT_CMD = "p";
        private const string AMOUNT_DISCOUNT_CMD = "q";
        private const string TAX_RATE_FREE_ITEM_CANCELATION_CMD = " "; // needs test;
        private const string TAX_RATE_ONE_ITEM_CANCELATION_CMD = "¡";
        private const string TAX_RATE_TWO_ITEM_CANCELATION_CMD = "¢";
        private const string TAX_RATE_THREE_ITEM_CANCELATION_CMD = "£";
        private const string CANCEL_INVOICE_CMD = "7";        
        private const string MEANS_OF_PAYMENT_CMD = "1";
        private readonly string[] MEANS_OF_PAYMENT_CASH = { "01", "02", "03", "04"};
        private readonly string[] MEANS_OF_PAYMENT_CHECK = { "05", "06", "07", "08" };
        private readonly string[] MEANS_OF_PAYMENT_CARD = { "09", "10", "11", "12" };
        private readonly string[] MEANS_OF_PAYMENT_TICKET = { "13", "14", "15", "16" };
        private const string PARTIAL_MEAN_OF_PAYMENT_CMD = "2";
        private const string CREDIT_NOTE_RELATED_RECEIPT_CMD = "JF";
        private const string ADD_ITEM_TO_CREDIT_NOTE_CMD = "d";
        private const string REMOVE_ITEM_FROM_CREDIT_NOTE = "ä"; //needs test
        private const string CREDIT_NOTE_PAY_TOTAL_CMD = "f";
        private const string CREDIT_NOTE_PAY_AMOUNT_CMD = "2";
        private const string SETUP_PRINTER_TIME_CMD = "PF";
        private const string SETUP_PRINTER_DATE_CMD = "PG";
        private const string SETUP_PRINTER_TAXES_CMD = "PT";
        private const string SETUP_PRINTER_MEANS_OF_PAYMENT_CMD = "PE";
        private const string SETUP_PRINTER_HEADER_CMD = "PH01"; //needs test
        private const string SETUP_PRINTER_FOOTER_CMD = "PH91"; //needs test
        private const string SETUP_PRINTER_FLAG_CMD = "PJ";
        private const string VERIFY_PRINTER_TAX_OPTIONS_CMD = "Pt";



        #endregion

        TallyDascon1125Commands()
        {
            printer = new Tfhka();
        }

        public void OpenPrinter(string port)
        {
            Execute(port);
        }

        public void LoginCachier(string cachierPassword)
        {
            var command = LOGIN_CACHIER_CMD + CompleteDigitsAtBegining(cachierPassword, 5);
            Execute(command);
        }

        public void LogoutCachier()
        {
            Execute(LOGOUT_CACHIER_CMD);
        }

        public void setUpPrinterMeansOfPayment(string means, string description)
        {
            var command = SETUP_PRINTER_MEANS_OF_PAYMENT_CMD + CompleteDigitsAtBegining(means, 2) + CompleteTextToLeft(description, 14);
            Execute(command);
        }

        public void SetUpPrinteTime(string hours, string minutes, string seconds = "00")
        {
            var command = SETUP_PRINTER_TIME_CMD + CompleteTextToLeft(hours, 2) + CompleteTextToLeft(minutes, 2) + CompleteTextToLeft(seconds, 2);
            Execute(command);
        }

        public void SetUpPrinteDate(string day, string month, string year)
        {
            var command = SETUP_PRINTER_DATE_CMD + CompleteTextToLeft(day, 2) + CompleteTextToLeft(month, 2) + CompleteTextToLeft(year, 2);
            Execute(command);
        }

        public void SetUpPrinterTaxOptions(string[] taxPercentage)
        {
            var taxes = from tax in taxPercentage select "0" + CompleteDeciamlValue(tax, 2, 2);
            var command = SETUP_PRINTER_TAXES_CMD + string.Join("", taxes);
            Execute(command);
        }

        public void SetUpPrinterHeader(string header)
        {
            var command = SETUP_PRINTER_HEADER_CMD + CompleteTextToLeft(header, 40);
            Execute(command);
        }
        public void SetupPrinterFooter(string footer)
        {
            var command = SETUP_PRINTER_FOOTER_CMD + CompleteTextToLeft(footer, 40);
            Execute(command);
        }

        public void SetupPrinterFlag(string flag, string value)
        {
            var command = SETUP_PRINTER_FLAG_CMD + CompleteDigitsAtBegining(flag, 2) + CompleteDigitsAtBegining(value, 2);
            Execute(command);
        }

        public void VerifyPrinterTaxOptions()
        {
            Execute(VERIFY_PRINTER_TAX_OPTIONS_CMD);
        }

        public void SetDocumentClientRUC(string ruc)
        {
            var command = SET_DOCUMENT_COSTUMER_RUC_CMD + CompleteTextToLeft(ruc, 20);
            Execute(command);
        }

        public void SetDocumentCostumerSocialReason(string reason)
        {
            var command = SET_DOCUMENT_COSTUMER_SOCIAL_REASON_CMD + CompleteTextToLeft(reason, 40);
            Execute(command);
        }

        public void SetDocumentCostumerAdditionalInfo(int linesAmout, string info)
        {
            var command = ADD_DOCUMENT_COSTUMER_ADDITIOAL_INFO_CMD + linesAmout +  CompleteTextToLeft(info, 40);
            Execute(command);
        }

        public void AddDocumentComent(string coment)
        {
            var command = ADD_DOCUMENT_COMENT_CMD + CompleteTextToLeft(coment, 20);
            Execute(command);
        }       

        public void AddItemToInvoice(string taxRate, string price, string quantity, string description)
        {
            var command = taxRate + CompleteDeciamlValue(price, 8, 2) + CompleteDeciamlValue(quantity, 5, 3) + CompleteTextToLeft(description, 116);
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

        /// <summary>
        /// returns the appropiated amount of digits for a price which is 8 and 2 floats.
        /// </summary>
        /// <returns>real price</returns>
        private string CompleteDeciamlValue(string price, int integerPart, int decimalPart) {
            string[] prices = price.Split('.');
            if (prices.Length == 1)
            {
                return CompleteDigitsAtBegining(prices[0], integerPart) + ".00";
            }
            return CompleteDigitsAtBegining(prices[0], integerPart) + "." + CompleteDigitsAtEnd(prices[1], decimalPart);
        }

        private string CompleteDigitsAtBegining(string intDigitis, int digitsRequired)
        {
            if (intDigitis.Length < digitsRequired)
            {
                CompleteDigitsAtBegining("0" + intDigitis, digitsRequired);
            }
            return intDigitis; 
        }

        private string CompleteDigitsAtEnd(string intDigitis, int digitsRequired)
        {
            if (intDigitis.Length < digitsRequired)
            {
                CompleteDigitsAtBegining(intDigitis + "0", digitsRequired);
            }
            return intDigitis;
        }

        private string CompleteTextToLeft(string description, int charactersRequired)
        {
            if (description.Length < charactersRequired)
            {
                CompleteTextToLeft(" " + description, charactersRequired);
            }
            return description;
        }

        private string CompleteTextCenter(string description, int charactersRequired, bool toLeft)
        {
            if (description.Length < charactersRequired)
            {
                var message = toLeft ? " " + description : description + " ";
                CompleteTextCenter(message, charactersRequired, !toLeft);
            }
            return description;
        }
    }
}
