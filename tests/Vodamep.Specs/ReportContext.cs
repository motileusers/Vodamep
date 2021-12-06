using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf;
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
        }

        public IReport Report { get; set; }

        public IMessage ReportM => (IMessage)this.Report;

        public IValidator Validator { get; set; }
        public ValidationResult Result
        {
            get
            {
                if (_validationResult == null)
                {
                    _validationResult = Validator.Validate(this.Report);
                }

                return _validationResult;
            }
        }

       public GetPropertiesByTypeDelegate GetPropertiesByType { get; set; }
    }
}
