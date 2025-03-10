﻿using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonStayValidator : AbstractValidator<StatLpReport>
    {
        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();
        public PersonStayValidator()
        {
            #region Documentation
            // AreaDef: STAT
            // OrderDef: 01
            // SectionDef: Person
            // StrengthDef: Fehler

            // CheckDef: Erlaubte Werte
            // Fields: Aufenthalt, Remark: Nur Personen mit gültigem Aufenthalt, Group: Inhaltlich
            #endregion

            // Zu jeder Person muss es mindestens einen Aufenthalt geben
            this.RuleFor(x => new { x.Persons, x.Stays })
                .Custom((a, ctx) =>
                {
                    StatLpReport report = ctx.InstanceToValidate as StatLpReport;

                    var idPersons = a.Persons.Select(x => x.Id).Distinct().ToArray();
                    var personIdsStays = (a.Stays.Select(x => x.PersonId)).Distinct().ToArray();

                    foreach (var id in idPersons.Except(personIdsStays))
                    {
                        var item = a.Persons.First(x => x.Id == id);
                        var index = a.Persons.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                            Validationmessages.StatLpStayEveryPersonMustBeInAStay(report.GetPersonName(id))));

                    }
                });

            // Zu jedem Aufenthalt muss es die Person geben
            this.RuleFor(x => new { x.Persons, x.Stays })
                .Custom((a, ctx) =>
                {
                    StatLpReport report = ctx.InstanceToValidate as StatLpReport;

                    var idPersons = a.Persons.Select(x => x.Id).ToArray();

                    foreach (var stay in a.Stays.Where(x => !idPersons.Contains(x.PersonId)))
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}", "Für einen Aufenthalt wurde keine Person gemeldet!"));
                    }
                });


            // der Zeiträume der Aufenthalte müssen passen
            this.RuleFor(x => new { x.Persons, x.Stays })
                .Custom((a, ctx) =>
                {
                    var report = ctx.InstanceToValidate as StatLpReport;


                    foreach (var person in a.Persons)
                    {
                        GroupedStay[] groupedStays;
                        try
                        {
                            // mitunter geht das gar nicht, dann wird ein Fehler geworfen
                            groupedStays = report.GetGroupedStays(person.Id, GroupedStay.SameTypeGroupMode.NotAllowed).ToArray();
                        }
                        catch (Exception e)
                        {
                            var index = a.Persons.IndexOf(person);
                            ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]", $"{report.GetPersonName(person.Id)}: {e.Message}"));
                            return;
                        }

                        foreach (var groupedStay in groupedStays)
                        {
                            // Aufenthalte vor Meldezeitraum
                            if (groupedStay.To < report.FromD)
                            {
                                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                                    Validationmessages.StatLpStayAheadOfPeriod(report.GetPersonName(person.Id), groupedStay.From.ToShortDateString(), groupedStay.To?.ToShortDateString())));
                            }

                            // Aufenthalte nach Meldezeitraum
                            if (groupedStay.To > report.ToD)
                            {
                                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                                    Validationmessages.StatLpStayAfterPeriod(report.GetPersonName(person.Id), groupedStay.From.ToShortDateString(), groupedStay.To?.ToShortDateString())));
                            }

                            StayLengthValidation(ctx, report, groupedStay, AdmissionType.HolidayAt, 42);

                            StayLengthValidation(ctx, report, groupedStay, AdmissionType.TransitionalAt, 365);

                            AdmissionTypeChangeValidation(ctx, report, groupedStay, AdmissionType.ContinuousAt, AdmissionType.HolidayAt);

                            AdmissionTypeChangeValidation(ctx, report, groupedStay, AdmissionType.ContinuousAt, AdmissionType.TransitionalAt);

                            AttributeValidation(ctx, report, groupedStay);

                            AdmissionValidation(ctx, report, person, groupedStay);

                            LeavingValidation(ctx, report, person, groupedStay);
                        }
                    }
                });
        }

        private void AttributeValidation(ValidationContext<StatLpReport> ctx, StatLpReport report, GroupedStay s)
        {
            #region Documentation
            // AreaDef: STAT
            // OrderDef: 03
            // SectionDef: Aufenthalt
            // StrengthDef: Fehler

            // CheckDef: Angaben bei Aufnahme
            // Fields: Hauptmerkmale, Remark: Meldung der 3 Hauptmerkmale bei Aufnahme, Group: Inhaltlich
            #endregion

            var attributTypes = new[]
{
                Model.Attribute.ValueOneofCase.Finance,
                Model.Attribute.ValueOneofCase.CareAllowance,
                Model.Attribute.ValueOneofCase.CareAllowanceArge
            };

            var attributesPerson = report.Attributes
                .Where(x => x.PersonId == s.Stays[0].PersonId);

            // Wenn der Aufenthalt im gleichen Jahr gestartet hat, dann müssen die Attribute zum Aufenthaltsstart gemeldet worden sein
            // Ansonsten zum 1.1. (Start des Berichts)
            var from = s.From > report.FromD ? s.From : report.FromD;

            foreach (var type in attributTypes)
            {
                Model.Attribute attribute = attributesPerson.Where(x => x.ValueCase == type && x.FromD == from).FirstOrDefault();

                if (attribute == null)
                {
                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                        Validationmessages.StatLpAdmissionAttributeMissing(report.GetPersonName(s.Stays[0].PersonId), from.ToShortDateString(), DisplayNameResolver.GetDisplayName(type.ToString()))));
                }
            }
        }


        #region Documentation
        // AreaDef: STAT
        // OrderDef: 03
        // SectionDef: Aufenthalt
        // StrengthDef: Fehler

        // CheckDef: Aufenthaltswechsel
        // Fields: Aufenthaltsart, Remark: Daueraufnahme auf Urlaubspflege nicht erlaubt, Group: Inhaltlich
        // Fields: Aufenthaltsart, Remark: Daueraufnahme auf Übergangspflege nicht erlaubt, Group: Inhaltlich
        #endregion

        private void AdmissionTypeChangeValidation(ValidationContext<StatLpReport> ctx, StatLpReport report, GroupedStay s, AdmissionType from, AdmissionType to)
        {
            for (var i = 1; i < s.Stays.Length; i++)
            {
                if (s.Stays[i].Type == to && s.Stays[i - 1].Type == from)
                {
                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                        Validationmessages.StatLpInvalidAdmissionTypeChange(DisplayNameResolver.GetDisplayName(from.ToString()), DisplayNameResolver.GetDisplayName(to.ToString()), report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To?.ToShortDateString())));
                }
            }
        }

        #region Documentation
        // AreaDef: STAT
        // OrderDef: 03
        // SectionDef: Aufenthalt
        // StrengthDef: Fehler

        // CheckDef: Aufenthaltsdauer Urlaub
        // Fields: Von/Bis/Aufnahmeart, Remark: Urlaub von der Pflege, 42 Tage, Group: Inhaltlich

        // CheckDef: Aufenthaltsdauer Übergang
        // Fields: Von/Bis/Aufnahmeart, Remark: Übergangspflege, 365 Tage, Group: Inhaltlich
        #endregion

        private void StayLengthValidation(ValidationContext<StatLpReport> ctx, StatLpReport report, GroupedStay s, AdmissionType admissionType, int days)
        {
            // Länge Urlaubsbetreuung
            var holidayToLong = s.Stays.Where(x => x.Type == admissionType)
                .Where(x => (x.ToD ?? report.ToD).Subtract(x.FromD).TotalDays > days)
                .FirstOrDefault();

            if (holidayToLong != null)
            {
                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                    Validationmessages.StatLpStayToLong(DisplayNameResolver.GetDisplayName(holidayToLong.Type.ToString()), report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To?.ToShortDateString(), days))
                {
                    Severity = Severity.Warning
                });
            }
        }

        #region Documentation
        // AreaDef: STAT
        // OrderDef: 04
        // SectionDef: Aufenthalt
        // StrengthDef: Fehler

        // CheckDef: Angaben bei Aufnahme
        // Fields: Aufnahmedaten, Remark: Eine Meldung der Aufnahmedaten bei Aufenthaltsstart, Group: Inhaltlich
        #endregion

        private void AdmissionValidation(ValidationContext<StatLpReport> ctx, StatLpReport report, Person person, GroupedStay s)
        {
            var admission = report.Admissions.Where(x => x.PersonId == person.Id && x.AdmissionDateD == s.From).ToArray();
            var hasMissingAdmission = !admission.Any();

            //Wenn der Beginn des Aufenthaltes zum Begin des Meldezeitraums ist, könnte es sich auch um einen Wechsel der Aufenthaltsart handeln
            if (hasMissingAdmission && s.From == report.FromD)
            {
                //überlegen: man könnte hier die Aufnahmearten einschränken, für die ein Wechsel zum Jahresende in Frage kommen?

                hasMissingAdmission = false;
            }


            if (hasMissingAdmission)
            {
                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                    Validationmessages.StatLpMissingAdmission(report.GetPersonName(person.Id), s.From.ToShortDateString())));
            }

            var hasMultipleAdmissions = admission.Length > 1 || report.Admissions
                .Where(x => x.PersonId == person.Id)
                .Where(x => x.AdmissionDateD > s.From && (!s.To.HasValue || x.AdmissionDateD <= s.To.Value))
                .Any();

            if (hasMultipleAdmissions)
            {
                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                    Validationmessages.StatLpMultipleAdmission(report.GetPersonName(person.Id), s.From.ToShortDateString())));
            }
        }

        private void LeavingValidation(ValidationContext<StatLpReport> ctx, StatLpReport report, Person person, GroupedStay s)
        {
            #region Documentation
            // AreaDef: STAT
            // OrderDef: 04
            // SectionDef: Aufenthalt
            // StrengthDef: Fehler

            // CheckDef: Angaben bei Abgang
            // Fields: Abgangsdaten, Remark: Eine Meldung der Abgangsdaten bei Aufenthaltsende, Group: Inhaltlich
            #endregion

            if (s.To.HasValue && s.To < DateTime.Today)
            {
                var leaving = report.Leavings.Where(x => x.PersonId == person.Id && x.LeavingDateD == s.To).ToArray();
                var hasMissingLeaving = !leaving.Any();

                //Wenn das Ende des Aufenthaltes am Ende Meldezeitraums ist, könnte es sich auch um einen Wechsel der Aufenthaltsart handeln
                if (hasMissingLeaving && s.To.Value == report.ToD)
                {
                    //überlegen: man könnte hier die Aufnahmearten einschränken, für die ein Wechsel zum Jahresende in Frage kommen?

                    hasMissingLeaving = false;
                }

                if (hasMissingLeaving)
                {
                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                        Validationmessages.StatLpMissingLeaving(report.GetPersonName(person.Id), s.To?.ToShortDateString())));
                }

                var hasMultipleLeavings = leaving.Length > 1 || report.Leavings
                    .Where(x => x.PersonId == person.Id)
                    .Where(x => x.LeavingDateD >= s.From && x.LeavingDateD < s.To.Value)
                    .Any();

                if (hasMultipleLeavings)
                {
                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}",
                        Validationmessages.StatLpMultipleLeavings(report.GetPersonName(person.Id), s.From.ToShortDateString())));
                }
            }
        }
    }
}