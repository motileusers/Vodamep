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
        private static DisplayNameResolver displayNameResolver = new DisplayNameResolver();

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
                            case AttributeType.CareAllowance:
                                break;
                            case AttributeType.CareAllowanceArge:
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

        private void CheckAdmissionType(Attribute attribute, List<StatLpReport> newerReports, CustomContext ctx)
        {
            var admissionType = (AdmissionType)Enum.Parse(typeof(AdmissionType), attribute.Value);

            switch (admissionType)
            {
                case AdmissionType.HolidayAt:
                    this.CheckMaxDurationPerType(attribute, newerReports, 42, AdmissionType.HolidayAt.ToString(), ctx);
                    break;

                case AdmissionType.TransitionalAt:
                    this.CheckMaxDurationPerType(attribute, newerReports, 365, AdmissionType.TransitionalAt.ToString(), ctx);
                    break;
            }
        }

        private void CheckMaxDurationPerType(Attribute attribute, List<StatLpReport> newerReports, int maxDays, string value, CustomContext ctx)
        {
            var startDate = attribute.FromD;

            for (var i = 0; i < newerReports.Count(); i++)
            {
                var report = newerReports[i];

                var admissionAttribute = report.Attributes.FirstOrDefault(x => x.PersonId == attribute.PersonId && x.AttributeType == AttributeType.AdmissionType);
                var stay = report.Stays.FirstOrDefault(x => x.PersonId == attribute.PersonId);
                var leaving = report.Leavings.FirstOrDefault(x => x.PersonId == attribute.PersonId);

                if (admissionAttribute?.Value != value || i == newerReports.Count - 1)
                {
                    DateTime stopDate = DateTime.MinValue;

                    if (admissionAttribute != null && admissionAttribute.Value != value)
                    {
                        stopDate = admissionAttribute.FromD;
                    }
                    else if (i == newerReports.Count - 1)
                    {
                        if (stay != null)
                        {
                            stopDate = stay.To.AsDate();
                        }
                        else if (leaving != null)
                        {
                            stopDate = leaving.Valid.AsDate();
                        }
                    }

                    if (stopDate == DateTime.MinValue)
                    {
                        continue;
                    }

                    var timeSpan = stopDate - startDate;

                    if (timeSpan.TotalDays > maxDays)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonPeriodForAdmissionTooLong(attribute.PersonId,
                                displayNameResolver.GetDisplayName(attribute.Value),
                                $"mehr als {maxDays} Tage")));

                        break;
                    }
                }
            }
        }

    }
}