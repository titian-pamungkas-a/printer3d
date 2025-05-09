using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public class MotorX : Motor, IMotorAcceleration
    {
        private double accelerataion;
        public MotorX(int axis, double velocity, double acceleration) : base(axis, velocity)
        {
            this.accelerataion = acceleration;
        }

        public override async Task<double> MoveAsync(double startPoint, double endPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double currentPosition = await CalculateDistance(startPoint, endPoint, cancellationToken);
            stopwatch.Stop();
            Log.Info($"Axis {this.Axis} sampai pada tujuan dalam waktu {stopwatch.Elapsed.TotalMilliseconds} {currentPosition} {startPoint} {endPoint}");
            currentPoint.CurrentPoint.SetAxisPoint(startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition, this.Axis);
            return startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition;
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
