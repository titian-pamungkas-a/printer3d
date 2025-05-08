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

        public override async Task<double> MoveAsync(double startPoint, double endPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double currentPosition = await CalculateDistance(startPoint, endPoint, cancellationToken);
            stopwatch.Stop();
            Console.WriteLine($"Axis {this.Axis} sampai pada tujuan dalam waktu {stopwatch.Elapsed.TotalMilliseconds} {currentPosition} {startPoint} {endPoint}");
            currentPoint.CurrentPoint.SetAxisPoint(startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition, this.Axis);
            return startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition;
        }

        public async Task<double> RiseUpMotor(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            return await MoveAsync(startPoint, 18, currentPoint, cancellationToken);
        }

        public override async Task<double> RiseDownAsync(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            return await MoveAsync(startPoint, 0, currentPoint, cancellationToken);
        }
    }
}
