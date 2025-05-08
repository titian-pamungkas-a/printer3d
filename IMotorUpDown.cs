using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public interface IMotorUpDown
    {
        public Task<double> RiseUpMotor(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken);
        
    }
}
