using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;
using Attribute = Vodamep.StatLp.Model.Attribute;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryTimeValidator : AbstractValidator<StatLpReportHistory>
    {
        private const string HolidayOfCare = "Urlaub von der Pflege";
        private const string TemporaryCare = "Übergangspflege";

        public PersonHistoryTimeValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sendMessage = a.StatLpReport;

                var allMessages = new List<StatLpReport>();
                allMessages.AddRange(a.StatLpReports);
                allMessages.Add(a.StatLpReport);
                allMessages.OrderBy(b => b.FromD);

                for (var i = 0; i < allMessages.Count; i++)
                {
                    var message = allMessages[i];

                    foreach (var attribute in message.Attributes)
                    {
                        switch (attribute.AttributeType)
                        {
                            case AttributeType.UndefinedAttribute:
                                throw new ArgumentOutOfRangeException();
                            case AttributeType.AdmissionType:
                                this.CheckAdmissionType(attribute, allMessages.GetRange(i + 1, allMessages.Count - i - 1), ctx);
                                break;
                            case AttributeType.Careallowance:
                                break;
                            case AttributeType.Careallowancearge:
                                break;
                            case AttributeType.Finance:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }



            });
        }

        private void CheckAdmissionType(Attribute attribute, IEnumerable<StatLpReport> newerReports, CustomContext ctx)
        {
            switch (attribute.Value)
            {
                case HolidayOfCare:
                    this.CheckHolidayOfCare(attribute, newerReports, 42, HolidayOfCare, ctx);
                    break;

                case TemporaryCare:
                    this.CheckHolidayOfCare(attribute, newerReports, 365, TemporaryCare, ctx);
                    break;
            }
        }

        private void CheckHolidayOfCare(Attribute attribute, IEnumerable<StatLpReport> newerReports, int maxDays, string value, CustomContext ctx)
        {
            var startDate = attribute.FromD;

            foreach (var report in newerReports)
            {
                var admissionAttribute = report.Attributes.FirstOrDefault(x => x.PersonId == attribute.PersonId && x.AttributeType == AttributeType.AdmissionType);

                if (admissionAttribute != null && admissionAttribute.Value != value)
                {
                    var stopDate = admissionAttribute.FromD;

                    var timeSpan = stopDate - startDate;

                    if (timeSpan.TotalDays > maxDays)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonPeriodForAdmissionTooLong(attribute.PersonId,
                                attribute.Value, $"mehr als {maxDays} Tage")));

                        break;
                    }
                }
            }
        }

    }
}