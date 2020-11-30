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

        // todo

        public MkkpValidationSteps()
        {
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

    }
}
