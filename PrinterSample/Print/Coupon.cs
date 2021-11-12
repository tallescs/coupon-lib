using System;
using System.Collections.Generic;

namespace PrinterSample.Print
{
    public class Coupon
    {
        public IEnumerable<Line> Lines { get; set; } = new List<Line>();
    }
}