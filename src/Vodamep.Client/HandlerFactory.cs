using System;

namespace Vodamep.Client
{
    public class HandlerFactory
    {
        public HandlerBase CreateFromType(Type type)
        {
            switch (type)
            {
                case Type.Agp:
                    return new AgpHandler();
                case Type.Hkpv:
                    return new HkpvHandler();
                case Type.Mkkp:
                    return new Mkkpandler();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}