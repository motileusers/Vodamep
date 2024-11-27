using Microsoft.VisualBasic;
using System;
using System.Globalization;
using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class InsuranceCodeProviderTests
    {
        private const string dateFormat = "dd.MM.yyyy";


        [Theory]
        [InlineData("19", null, true)]
        [InlineData("GKK", null, false)]
        [InlineData("01", "01.01.2024", false)]
        [InlineData("01", "31.12.2023", true)]
        [InlineData("19", "01.01.2024", true)]

        [InlineData("FL", "31.12.2024", true)]  // FL für alte Meldungen
        [InlineData("FL", "01.01.2025", false)] // FL nur bis 31.12.2024
        [InlineData("NA", "01.01.2025", true)]  // NA ab 2024 möglich
        [InlineData("NA", "31.12.2024", true)]  // NA auch für alte Meldungen
        [InlineData(null, null, false)]
        [InlineData("", null, false)]        
        public void IsValid(string code, string dateString, bool expected)
        {
            var p = InsuranceCodeProvider.Instance;
            DateTime date = new DateTime(2006, 1, 1);

            if(!String.IsNullOrEmpty(dateString))
            {
                date = DateTime.ParseExact(dateString, dateFormat, CultureInfo.InvariantCulture);
            }

            Assert.Equal(expected, p.IsValid(code, date));
        }
    }
}
