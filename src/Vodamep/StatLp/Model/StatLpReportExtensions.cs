using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodamep.ReportBase;
using Vodamep.StatLp.Validation;

namespace Vodamep.StatLp.Model
{
    public static class StatLpReportExtensions
    {
        public static StatLpReport AddPerson(this StatLpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static StatLpReport AddPersons(this StatLpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));
        public static StatLpReport AddAdmission(this StatLpReport report, Admission admission) => report.InvokeAndReturn(m => m.Admissions.Add(admission));
        public static StatLpReport AddAdmissions(this StatLpReport report, IEnumerable<Admission> admissions) => report.InvokeAndReturn(m => m.Admissions.AddRange(admissions));
        public static StatLpReport AddAttribute(this StatLpReport report, Attribute attribute) => report.InvokeAndReturn(m => m.Attributes.Add(attribute));
        public static StatLpReport AddAttributes(this StatLpReport report, IEnumerable<Attribute> attributes) => report.InvokeAndReturn(m => m.Attributes.AddRange(attributes));
        public static StatLpReport AddStay(this StatLpReport report, Stay stay) => report.InvokeAndReturn(m => m.Stays.Add(stay));
        public static StatLpReport AddStays(this StatLpReport report, IEnumerable<Stay> stays) => report.InvokeAndReturn(m => m.Stays.AddRange(stays));
        public static StatLpReport AddLeaving(this StatLpReport report, Leaving leaving) => report.InvokeAndReturn(m => m.Leavings.Add(leaving));
        public static StatLpReport AddLeavings(this StatLpReport report, IEnumerable<Leaving> leavings) => report.InvokeAndReturn(m => m.Leavings.AddRange(leavings));

        private static StatLpReport InvokeAndReturn(this StatLpReport m, Action<StatLpReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this StatLpReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        public static StatLpReportValidationResult Validate(this StatLpReport report) => (StatLpReportValidationResult)new StatLpReportValidator().Validate(report);
      
        public static StatLpReportValidationResult ValidateHistory(this StatLpReport report, List<StatLpReport> existingReports) => (StatLpReportValidationResult)new StatLpReportHistoryValidator().Validate(new StatLpReportHistory {StatLpReport = report, StatLpReports = existingReports});

        public static string ValidateToText(this StatLpReport report, bool ignoreWarnings) => new StatLpReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this StatLpReport report, bool ignoreWarnings) => new StatLpReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static StatLpReport AsSorted(this StatLpReport report)
        {
            var result = new StatLpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            //result.Activities.AddRange(report.Activities.AsSorted());
            
            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));            
            //result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }

    }
}