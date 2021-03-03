using System;
using Vodamep.ReportBase;

namespace Vodamep.Client
{
    public class HandlerFactory
    {
        public HandlerBase CreateFromType(ReportType type)
        {
            switch (type)
            {
                case ReportType.Agp:
                    return new AgpHandler();
                case ReportType.Cm:
                    return new CmHandler();
                case ReportType.Hkpv:
                    return new HkpvHandler();
                case ReportType.Mkkp:
                    return new Mkkpandler();
                case ReportType.Mohi:
                    return new MohiHandler();
                case ReportType.StatLp:
                    return new StatLpHandler();
                case ReportType.Tb:
                    return new TbHandler();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}