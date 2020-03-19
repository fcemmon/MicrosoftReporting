using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwissBillQR.Net.Generator;
using SwissBillQR.Net.Generator.Canvas;
using System.IO;

namespace SwissQRBillGenerator
{
    public class SwissQRBillGeneratorClass
    {
        Bill code;
        public static SwissQRBillGeneratorClass Instance(
            string account,
            string creditorName,
            string creditorAddressLine1,
            string creditorAddressLine2,
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
            SwissQRBillGeneratorClass instance = new SwissQRBillGeneratorClass(account,
                creditorName,
                creditorAddressLine1,
                creditorAddressLine2,
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

        public SwissQRBillGeneratorClass(
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
            code = new Bill
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

        public static string getReturn(string account)
        {
            return account;
        }
        public static byte[] generateQRcode(
            string account,
            string creditorName,
            string creditorAddressLine1,
            string creditorAddressLine2,
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
            Bill bill = new Bill
            {
                Account = account,
                Creditor = new Address
                {
                    Name = creditorName,
                    AddressLine1 = creditorAddressLine1,
                    AddressLine2 = creditorAddressLine2,
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
            
            string path = "qrbill.png";
            using (PNGCanvas canvas = new PNGCanvas(QRBill.QrBillWidth, QRBill.QrBillHeight, 144, "Arial"))
            {
               QRBill.Draw(bill, canvas);
                canvas.SaveAs(path);
            }

             byte[] array = File.ReadAllBytes(Path.GetFullPath(path));

            return array;
        }
    }
}
