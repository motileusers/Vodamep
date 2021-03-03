using System;
using System.Threading.Tasks;
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Vodamep.Cm.Model;
using Vodamep.Cm.Validation;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;
using Vodamep.Mkkp.Model;
using Vodamep.Mkkp.Validation;
using Vodamep.Mohi.Model;
using Vodamep.Mohi.Validation;
using Vodamep.ReportBase;
using Vodamep.StatLp.Model;
using Vodamep.StatLp.Validation;
using Vodamep.Tb.Model;
using Vodamep.Tb.Validation;

namespace Vodamep.Api
{
    public class VodamepValidator
    {
        public async Task<Tuple<bool, string>> Validate(IReportBase report)
        {
            //#extend 
            if (report is AgpReport agpReport)
            {
                var validationResult = await new AgpReportValidator().ValidateAsync(agpReport);
                var msg = new AgpReportValidationResultFormatter(Vodamep.Agp.Validation.ResultFormatterTemplate.Text, false).Format(agpReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            if (report is CmReport cmReport)
            {
                var validationResult = await new CmReportValidator().ValidateAsync(cmReport);
                var msg = new CmReportValidationResultFormatter(Vodamep.Cm.Validation.ResultFormatterTemplate.Text, false).Format(cmReport, validationResult);

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

            if (report is MohiReport mohiReport)
            {
                var validationResult = await new MohiReportValidator().ValidateAsync(mohiReport);
                var msg = new MohiReportValidationResultFormatter(Vodamep.Mohi.Validation.ResultFormatterTemplate.Text, false).Format(mohiReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            if (report is StatLpReport statLpReport)
            {
                var validationResult = await new StatLpReportValidator().ValidateAsync(statLpReport);
                var msg = new StatLpReportValidationResultFormatter(Vodamep.StatLp.Validation.ResultFormatterTemplate.Text, false).Format(statLpReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            if (report is TbReport tbReport)
            {
                var validationResult = await new TbReportValidator().ValidateAsync(tbReport);
                var msg = new TbReportValidationResultFormatter(Vodamep.Tb.Validation.ResultFormatterTemplate.Text, false).Format(tbReport, validationResult);

                return new Tuple<bool, string>(validationResult.IsValid, msg);
            }

            return null;
        }
    }
}