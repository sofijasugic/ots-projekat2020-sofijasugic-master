﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshoppy.FinanceModule.Models
{
    public class KuwaitiDinarCurrency : Currency
    {
        public KuwaitiDinarCurrency(double multiplyFactor)
        {
            MultiplyFactor = multiplyFactor;
        }
    }
}
