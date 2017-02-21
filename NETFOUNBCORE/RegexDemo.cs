
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace NETFOUNB.Tests
{
    public class RegexDemo
    {
        [Fact]
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

        [Fact]
        public void IbanNietGeldigWannerNietBegintMetLandCode()
        {
            Assert.Throws<ArgumentException>(() => new Iban("0086 INGB 0002 4455 88"));
        }

        [Fact]
        public void IbanWelOfNietGeldigWanneerLandCodeInKleineLetters()
        {
            new Iban("nl86 ingb 0002 4455 88");
        }

        [Fact]
        public void BepaalDeBankCodeUitEenIban()
        {
            var iban = new Iban("NL86 INGB 0002 4455 88");
            Assert.Equal("INGB", iban.BankCode);
        }

        [Fact]
        public void BepaalDeAndereBankCodeUitEenIban()
        {
            var iban = new Iban("NL05 RABO 1234 1234 00");
            Assert.Equal("RABO", iban.BankCode);
        }

        [Fact]
        public void BepaalDeBankCodeUitEenIbanZonderSpaties()
        {
            var iban = new Iban("NL05RABO1234123400");
            Assert.Equal("RABO", iban.BankCode);
        }
    }
}