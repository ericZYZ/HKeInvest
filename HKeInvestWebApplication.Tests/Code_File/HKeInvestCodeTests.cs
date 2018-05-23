using NUnit.Framework;
using HKeInvestWebApplication.Code_File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKeInvestWebApplication.Code_File.Tests
{
    [TestFixture()]
    public class HKeInvestCodeTests
    {
        [Test()]
        [TestCase("HKD", "1.000", "EUR", "8.848", 250, 28.25)]
        [TestCase("HKD", "1.000", "USD", "7.756", 250, 32.23)]
        [TestCase("HKD", "1.000", "JPY", "0.072", 250, 3472.22)]
        [TestCase("HKD", "1.000", "HKD", "1.000", 250, 250)]
        [TestCase("HKD", "1.000", "GBP", "11.047", 250, 22.63)]
        public void convertCurrencyTest(
            string fromCurrency,
            string fromCurrencyRate,
            string toCurrency,
            string toCurrencyRate,
            decimal value,
            decimal expectedResult)
        {
            HKeInvestCode myHKeInvestCode = new HKeInvestCode();

            Assert.That(
                myHKeInvestCode.convertCurrency(fromCurrency, fromCurrencyRate, toCurrency, toCurrencyRate, value),
                Is.EqualTo(expectedResult)
                );
        }
    }
}