using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Vodamep.ValidationBase
{
    internal class CultureCheck
    {
        public static void Check()
        {
            bool isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (!isGerman)
            {
                throw new Exception("CurrentThread Culture must be set to 'de'");
            }
        }
    }
}
