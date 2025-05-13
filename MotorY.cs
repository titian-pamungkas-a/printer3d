using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    internal class MotorY : Motor, IMotorAcceleration
    {
        private double acceleration;
        public MotorY(int axis, double velocity, double acceleration) : base(axis, velocity)
        {
            this.acceleration = acceleration;
        }

        public void AccelerateMotor(double acc)
        {
            throw new NotImplementedException();
        }

        public void DeccelerateMotor(double acc)
        {
            throw new NotImplementedException();
        }

        public override async Task<double> RiseDownAsync(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            return await MoveAsync(startPoint, 0, currentPoint, cancellationToken);
        }
    }
}
