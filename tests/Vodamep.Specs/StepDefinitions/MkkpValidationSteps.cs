using FluentValidation;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Vodamep.Data;
using Vodamep.Data.Dummy;
using Vodamep.Mkkp.Model;
using Vodamep.Mkkp.Validation;
using Xunit;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class MkkpValidationSteps
    {

        private MkkpReportValidationResult _result;
        private Activity _dummyActivities;

        public MkkpValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new MkkpDisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            // todo

        }

        public MkkpReport Report { get; private set; }

        public MkkpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (MkkpReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }


        [Given(@"eine Meldung3 ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

        // todo
    }
}
