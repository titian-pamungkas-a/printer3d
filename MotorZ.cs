using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace printer3d
{
    public class MotorZ : Motor, IMotorUpDown
    {
        public MotorZ(int axis, double velocity) : base(axis, velocity)
        {
        }

        public async Task<double> RiseUpMotor(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            return await MoveAsync(startPoint, 12, currentPoint, cancellationToken);
        }

        public override async Task<double> RiseDownAsync(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            return await MoveAsync(startPoint, 0, currentPoint, cancellationToken);
        }
    }
}
