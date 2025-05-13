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
            await AccelerateMotor();
            double currentPosition = await CalculateDistance(currentPoint, startPoint, endPoint, cancellationToken);
            stopwatch.Stop();
            Log.Info($"Axis {GetAxisPoint(Axis)} " +
                $"selesai pada {stopwatch.Elapsed.TotalSeconds} " +
                $"jalur {startPoint}-{endPoint} " +
                $"posisi {GetAxisPoint(Axis)}_{currentPoint.CurrentPoint.GetAxisPoint(Axis)}");
            return startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition;
        }

        public async Task AccelerateMotor()
        {
            await Task.Delay(1000);
        }

        public override async Task<double> RiseDownAsync(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            return await MoveAsync(startPoint, 0, currentPoint, cancellationToken);
        }
    }
}
