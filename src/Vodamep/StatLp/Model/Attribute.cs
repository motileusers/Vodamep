using System;

namespace Vodamep.StatLp.Model
{
    public partial class Attribute
    {
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