using CommandLine;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MortgageHelper
{
    class Program
    {
        private static double TotalPrincipal { get; set; }

        private static double InterestRate { get; set; }

        private static double MonthlyPayment { get; set; }

        private static double MonthlyTaxesAndInsurance { get; set; }

        private static IEnumerable<int> LoanLengthInMonths { get; set; }

        private static double MonthInterest(double outstandingPrincipal, double mortgageRate) => outstandingPrincipal * mortgageRate / 12;

        private static double MonthPrincipal(double monthlyPaidInterest) => MonthlyPayment - monthlyPaidInterest;

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       TotalPrincipal = o.TotalPrincipal;
                       InterestRate = o.InterestRate;
                       MonthlyPayment = o.MonthlyPayment;
                       MonthlyTaxesAndInsurance = o.MonthlyTaxesAndInsurance;
                       LoanLengthInMonths = Enumerable.Range(1, o.LoanLength);
                   });

            var runningPrincipal = TotalPrincipal;
            var payments = new List<MonthlyPayment>();

            foreach (var month in LoanLengthInMonths)
            {
                var monthlyPaidInterest = MonthInterest(runningPrincipal, InterestRate);
                var monthlyPaidPrincipal = MonthPrincipal(monthlyPaidInterest) - MonthlyTaxesAndInsurance;
                var monthlyEndingBalance = runningPrincipal - monthlyPaidPrincipal;

                payments.Add(new MonthlyPayment
                {
                    MonthNumber = month,
                    StartingBalance = runningPrincipal,
                    Payment = MonthlyPayment,
                    InterestPaid = monthlyPaidInterest,
                    PrincipalPaid = monthlyPaidPrincipal,
                    TaxesAndInsurance = MonthlyTaxesAndInsurance,
                    EndingBalance = monthlyEndingBalance
                });
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
