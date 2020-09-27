using System;
using CommandLine;

namespace MortgageHelper
{
    public class Options
    {
        [Option('p', "TotalPrincipal", Required = true, HelpText = "Please provide the total principal on the mortgage.")]
        public double TotalPrincipal { get; set; }

        [Option('i', "InterestRate", Required = true, HelpText = "Please provide the interest rate on the mortgage.")]
        public double InterestRate { get; set; }

        [Option('m', "MonthlyPayment", Required = true, HelpText = "Please provide the monthly payment amount on the mortgage.")]
        public double MonthlyPayment { get; set; }

        [Option('t', "MonthlyTaxesAndInsurance", Required = true, HelpText = "Please provide the monthly taxes and insurance payment on the home.")]
        public double MonthlyTaxesAndInsurance { get; set; }

        [Option('l', "LoanLength", Required = true, HelpText = "Please provide the loan length in months.")]
        public int LoanLength { get; set; }
    }
}