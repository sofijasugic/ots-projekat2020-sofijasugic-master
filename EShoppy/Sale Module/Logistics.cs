using Eshoppy.SalesModule.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshoppy.SalesModule
{
    public class Logistics
    {
        public List<IOffer> Offers { get; set; }

        public Logistics(List<IOffer> offers)
        {
            Offers = offers;
        }
    }
}
