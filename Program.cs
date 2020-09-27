using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MortgageHelper
{
    class Program
    {
        private static double TotalPrincipal => 0;

        private static double InterestRate => .0;

        private static double MonthlyPayment => 0;

        private static double MonthlyTaxesAndInsureance => 0;

        private static IEnumerable<int> LoanLengthInMonths => Enumerable.Range(1, 60);

        private static double MonthInterest(double outstandingPrincipal, double mortgageRate) => outstandingPrincipal * mortgageRate / 12;

        private static double MonthPrincipal(double monthlyPaidInterest) => MonthlyPayment - monthlyPaidInterest;

        static void Main(string[] args)
        {
            var runningPrincipal = TotalPrincipal;
            var payments = new List<MonthlyPayment>();

            foreach (var month in LoanLengthInMonths)
            {
                var monthlyPaidInterest = MonthInterest(runningPrincipal, InterestRate);
                var monthlyPaidPrincipal = MonthPrincipal(monthlyPaidInterest) - MonthlyTaxesAndInsureance;
                var monthlyEndingBalance = runningPrincipal - monthlyPaidPrincipal;

                payments.Add(new MonthlyPayment { MonthNumber = month, StartingBalance = runningPrincipal, Payment = MonthlyPayment, InterestPaid = monthlyPaidInterest, PrincipalPaid = monthlyPaidPrincipal, TaxesAndInsurance = MonthlyTaxesAndInsureance, EndingBalance = monthlyEndingBalance });
                runningPrincipal = monthlyEndingBalance;
            }

            var currentDir = Directory.GetCurrentDirectory();
            using (var writer = new StreamWriter(@$"{currentDir}\output.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(payments);
            }
        }
    }
}
