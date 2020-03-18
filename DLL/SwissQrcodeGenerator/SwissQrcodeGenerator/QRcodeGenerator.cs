using System;
using System.Collections.Generic;
using System.Text;
using Codecrete.SwissQRBill.Generator;
using SwissQrcodeGenerator.models;
namespace SwissQrcodeGenerator
{
    public class QRcodeGenerator
    {
        QRCode code;

        public static QRcodeGenerator Instance(
            string account, 
            string creditorName, 
            string creditorAddresssLine1,
            string creditorAddresssLine2,
            string creditorCountryCode,
            decimal amount,
            string currency,
            string debtorName,
            string debtorAddressLine1,
            string debtorAddressLine2,
            string debtorCountryCode,
            string reference,
            string unstructuredMessage)
        {
            QRcodeGenerator instance = new QRcodeGenerator(account, 
                creditorName, 
                creditorAddresssLine1, 
                creditorAddresssLine2, 
                creditorCountryCode, 
                amount, 
                currency, 
                debtorName, 
                debtorAddressLine1, 
                debtorAddressLine2, 
                debtorCountryCode, 
                reference, 
                unstructuredMessage);

            return instance;
        }

        public QRcodeGenerator(
            string account,
            string creditorName,
            string creditorAddresssLine1,
            string creditorAddresssLine2,
            string creditorCountryCode,
            decimal amount,
            string currency,
            string debtorName,
            string debtorAddressLine1,
            string debtorAddressLine2,
            string debtorCountryCode,
            string reference,
            string unstructuredMessage)
        {
            code = new QRCode
            {
                Account = account,
                Creditor = new Address
                {
                    Name = creditorName,
                    AddressLine1 = creditorAddresssLine1,
                    AddressLine2 = creditorAddresssLine2,
                    CountryCode = creditorCountryCode
                },
                Amount = amount,
                Currency = currency,
                Debtor = new Address
                {
                    Name = debtorName,
                    AddressLine1 = debtorAddressLine1,
                    AddressLine2 = debtorAddressLine2,
                    CountryCode = debtorCountryCode
                },
                Reference = reference,
                UnstructuredMessage = unstructuredMessage
            };
        }
        
        public byte[] generateQRcode() {
            Bill bill = new Bill
            {
                Account = this.code.Account,
                Creditor = this.code.Creditor,
                Amount = this.code.Amount,
                Currency = this.code.Currency,
                Debtor = this.code.Debtor,
                Reference = this.code.Reference,
                UnstructuredMessage = this.code.UnstructuredMessage
            };

            byte[] svg = QRBill.Generate(bill);

            return svg;
        }
    }
}
