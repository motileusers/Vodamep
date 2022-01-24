using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Vodamep.ReportBase;

namespace Vodamep.Specs
{

    public delegate IEnumerable<IMessage> GetPropertiesByTypeDelegate(string typeName);

    public class ReportContext 
    {
        private ValidationResult _validationResult;

        public ReportContext()
        {            
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
            this.Validate = () => this.Report.Validate();
        }

        public IReport Report { get; set; }

        public IReport PrecedingReport { get; set; }

        public IMessage ReportM => (IMessage)this.Report;

        public ValidationResult Result
        {
            get
            {
                if (_validationResult == null)
                {
                    _validationResult = Validate();
                }

                return _validationResult;
            }
        }

        public GetPropertiesByTypeDelegate GetPropertiesByType { get; set; }

        public Func<ValidationResult> Validate { get; set; }
    }
}
