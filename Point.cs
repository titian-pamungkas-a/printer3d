using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public class Point(double x, double y, double z)
    {
        public double xAxis { get; set; } = x;
        public double yAxis { get; set; } = y;
        public double zAxis { get; set; } = z;
    }
}
