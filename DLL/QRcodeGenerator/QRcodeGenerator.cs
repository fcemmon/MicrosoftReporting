using System;
using QRcodeGenerator.models;
using Codecrete.SwissQRBill.Generator;
using System.Linq;
using Codecrete.SwissQRBill.Generator.Canvas;

namespace QRcodeGenerator
{
    public class QRcodeGenerator
    {
        Bill code;
        public static QRcodeGenerator Instance(
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
            QRcodeGenerator instance = new QRcodeGenerator(account,
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

            return svg;
        }
    }
}
