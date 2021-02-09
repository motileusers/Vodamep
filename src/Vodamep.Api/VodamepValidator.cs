using System;
using System.Threading.Tasks;
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;
using Vodamep.Mkkp.Model;
using Vodamep.Mkkp.Validation;
using Vodamep.ReportBase;

namespace Vodamep.Api
{
    public class VodamepValidator
    {
        public async Task<Tuple<bool, string>> Validate(IReportBase report)
        {
            if (report is AgpReport agpReport)
            {
                var validationResult = await new AgpReportValidator().ValidateAsync(agpReport);
                var msg = new AgpReportValidationResultFormatter(Vodamep.Agp.Validation.ResultFormatterTemplate.Text, false).Format(agpReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            if (report is HkpvReport hkpvReport)
            {
                var validationResult = await new HkpvReportValidator().ValidateAsync(hkpvReport);
                var msg = new HkpvReportValidationResultFormatter(Vodamep.Hkpv.Validation.ResultFormatterTemplate.Text, false).Format(hkpvReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            if (report is MkkpReport mkkpReport)
            {
                var validationResult = await new MkkpReportValidator().ValidateAsync(mkkpReport);
                var msg = new MkkpReportValidationResultFormatter(Vodamep.Mkkp.Validation.ResultFormatterTemplate.Text, false).Format(mkkpReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            return null;
        }
    }
}