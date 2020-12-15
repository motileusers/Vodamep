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
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Xunit;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class AgpValidationSteps
    {

        private AgpReportValidationResult _result;
        private Activity _dummyActivities;

        // todo

        public AgpValidationSteps()
        {
        }

        public AgpReport Report { get; private set; }

        public AgpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (AgpReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }


        [Given(@"eine Meldung2 ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

    }
}
