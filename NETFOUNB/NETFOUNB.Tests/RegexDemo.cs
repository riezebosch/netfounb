using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class RegexDemo
    {
        [TestMethod]
        public void TestMethod1()
        {
            new Iban("NL86 INGB 0002 4455 88");
        }

        struct Iban
        {
            public Iban(string input) : this()
            {
                var match = Regex.Match(input, @"^[a-zA-Z]{2}\d{2}\s?(?<code>.{4}).*");
                if (!match.Success)
                {
                    throw new ArgumentException(nameof(input), "dit is geen geldige iban");
                }

                BankCode = match.Groups["code"].Value;
            }

            public string BankCode { get; private set; }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IbanNietGeldigWannerNietBegintMetLandCode()
        {
            new Iban("0086 INGB 0002 4455 88");
        }

        [TestMethod]
        public void IbanWelOfNietGeldigWanneerLandCodeInKleineLetters()
        {
            new Iban("nl86 ingb 0002 4455 88");
        }

        [TestMethod]
        public void BepaalDeBankCodeUitEenIban()
        {
            var iban = new Iban("NL86 INGB 0002 4455 88");
            Assert.AreEqual("INGB", iban.BankCode);
        }


        [TestMethod]
        public void BepaalDeAndereBankCodeUitEenIban()
        {
            var iban = new Iban("NL05 RABO 1234 1234 00");
            Assert.AreEqual("RABO", iban.BankCode);
        }

        [TestMethod]
        public void BepaalDeBankCodeUitEenIbanZonderSpaties()
        {
            var iban = new Iban("NL05RABO1234123400");
            Assert.AreEqual("RABO", iban.BankCode);
        }
    }
}
