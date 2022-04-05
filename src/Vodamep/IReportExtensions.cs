using FluentValidation.Results;
using System;
using System.Linq;
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
using Vodamep.StatLp.Validation.Adjacent;
using Vodamep.StatLp.Validation.Update;
using Vodamep.Tb.Model;
using Vodamep.Tb.Validation;

namespace Vodamep
{
    public static class IReportExtensions
    {
        public static ValidationResult Validate(this IReport report)
        {
            switch (report)
            {
                case AgpReport agp: return new AgpReportValidator().Validate(agp);
                case CmReport Cm: return new CmReportValidator().Validate(Cm);
                case HkpvReport hkpv: return new HkpvReportValidator().Validate(hkpv);
                case MkkpReport mkkp: return new MkkpReportValidator().Validate(mkkp);
                case MohiReport mohi: return new MohiReportValidator().Validate(mohi);
                case StatLpReport statLp: return new StatLpReportValidator().Validate(statLp);
                case TbReport tb: return new TbReportValidator().Validate(tb);
                default: return default;
            }
        }

        public static ValidationResult Validate<T>(this T report, T other)
            where T : IReport
        {
            if (other == null)
                return default;

            if (report is StatLpReport r && other is StatLpReport rOther)
            {
                if (report.FromD.Date.AddDays(-1) == other.ToD.Date)
                {
                    var reportsWithoutDoubletes = new StatLpReport[] { rOther, r }.RemoveDoubletes();

                    return new StatLpAdjacentReportsValidator().Validate((reportsWithoutDoubletes[0], reportsWithoutDoubletes[1]));
                }

                if (report.FromD.Date == other.FromD.Date)
                {
                    return new StatLpUpdateReportValidator().Validate((rOther, r));
                }
            }

            return default;
        }

        public static (bool IsValid, string Message) ValidateToText(this IReport report, bool ignoreWarnings)
        {
            var vr = Validate(report);
            string msg = string.Empty;
            switch (report)
            {
                case AgpReport agp:
                    msg = new AgpReportValidationResultFormatter(Agp.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(agp, vr);
                    break;
                case CmReport cm:
                    msg = new CmReportValidationResultFormatter(Cm.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(cm, vr);
                    break;
                case HkpvReport hkpv:
                    msg = new HkpvReportValidationResultFormatter(Hkpv.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(hkpv, vr);
                    break;
                case MkkpReport mkkp:
                    msg = new MkkpReportValidationResultFormatter(Mkkp.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(mkkp, vr);
                    break;
                case MohiReport mohi:
                    msg = new MohiReportValidationResultFormatter(Mohi.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(mohi, vr);
                    break;
                case StatLpReport statLp:
                    msg = new StatLpReportValidationResultFormatter(StatLp.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(statLp, vr);
                    break;
                case TbReport tb:
                    msg = new TbReportValidationResultFormatter(Tb.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(tb, vr);
                    break;
            }

            return (vr.IsValid, msg);
        }

        [Obsolete("wird das noch benötigt?")]
        public static (bool IsValid, string[] messages) ValidateToEnumable(this IReport report, bool ignoreWarnings)
        {
            var vr = Validate(report);
            string[] msg = Array.Empty<string>();
            switch (report)
            {
                case AgpReport agp:
                    msg = new AgpReportValidationResultListFormatter(Agp.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(agp, vr).ToArray();
                    break;
                case CmReport cm:
                    msg = new CmReportValidationResultListFormatter(Cm.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(cm, vr).ToArray();
                    break;
                case HkpvReport hkpv:
                    msg = new HkpvReportValidationResultListFormatter(Hkpv.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(hkpv, vr).ToArray();
                    break;
                case MkkpReport mkkp:
                    msg = new MkkpReportValidationResultListFormatter(Mkkp.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(mkkp, vr).ToArray();
                    break;
                case MohiReport mohi:
                    msg = new MohiReportValidationResultListFormatter(Mohi.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(mohi, vr).ToArray();
                    break;
                case StatLpReport statLp:
                    msg = new StatLpReportValidationResultListFormatter(StatLp.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(statLp, vr).ToArray();
                    break;
                case TbReport tb:
                    msg = new TbReportValidationResultListFormatter(Tb.Validation.ResultFormatterTemplate.Text, ignoreWarnings).Format(tb, vr).ToArray();
                    break;
            }

            return (vr.IsValid, msg);
        }


        public static Task<SendResult> Send(this IReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        internal static T InvokeAndReturn<T>(this T m, Action<T> action)
            where T : IReport
        {
            action(m);
            return m;
        }

    }
}
