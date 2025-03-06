using System;

namespace Vodamep.StatLp.Model
{
    public partial class Attribute
    {
        /// <summary>
        /// Eindeutige Id, die verwendet werden kann um externe IDs zu speichern (wird nicht übermittelt)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Referenz Id, die verwendet werden kann um externe IDs zu speichern (wird nicht übermittelt)
        /// </summary>
        public string ExternalStayId { get; set; }

        /// <summary>
        /// Referenz Datum, das verwendet werden kann um externe Daten zu speichern (wird nicht übermittelt)
        /// </summary>
        public DateTime ReferenceDate { get; set; }

        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public void SetValue(string attributeType, string value)
        {
            if (Enum.TryParse<Attribute.ValueOneofCase>(attributeType, true, out Attribute.ValueOneofCase type))
            {
                switch (type)
                {
                    case Attribute.ValueOneofCase.CareAllowance:
                        {
                            this.CareAllowance = Enum.TryParse<CareAllowance>(value, out var v) ? v : CareAllowance.UndefinedAllowance;
                            break;
                        }
                    case Attribute.ValueOneofCase.CareAllowanceArge:
                        {
                            this.CareAllowanceArge = Enum.TryParse<CareAllowanceArge>(value, out var v) ? v : CareAllowanceArge.UndefinedAr;
                            break;
                        }
                    case Attribute.ValueOneofCase.Finance:
                        {
                            this.Finance = Enum.TryParse<Finance>(value, out var v) ? v : Finance.UndefinedFi;
                        }
                        break;
                }
            }
        }

    }
}