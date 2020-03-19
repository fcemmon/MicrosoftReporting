    using System;
using System.Collections.Generic;
using Codecrete.SwissQRBill.Generator;

namespace QRcodeGenerator.models
{
    public sealed class QRCode : IEquatable<QRCode>
    {
        public QRCode() { }
        public string Account { get; set; }
        public string UnstructuredMessage { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public Address Creditor { get; set; }
        public string Reference { get; set; }
        public Address Debtor { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as QRCode);
        }

        public bool Equals(QRCode other)
        {
            return other != null &&
                Currency == other.Currency &&
                EqualityComparer<decimal?>.Default.Equals(Amount, other.Amount) &&
                Account == other.Account &&
                EqualityComparer<Address>.Default.Equals(Creditor, other.Creditor) &&
                Reference == other.Reference &&
                EqualityComparer<Address>.Default.Equals(Debtor, other.Debtor);
        }

        /// <summary>Gets the hash code for this instance.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hashCode = -765739998;
            hashCode = hashCode * -1521134295 + EqualityComparer<decimal?>.Default.GetHashCode(Amount);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Account);
            hashCode = hashCode * -1521134295 + EqualityComparer<Address>.Default.GetHashCode(Creditor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Reference);
            hashCode = hashCode * -1521134295 + EqualityComparer<Address>.Default.GetHashCode(Debtor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UnstructuredMessage);
            return hashCode;
        }
    }
}
