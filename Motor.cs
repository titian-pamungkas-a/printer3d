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
    public abstract class Motor(int axis, double velocity)
    {
        public int Axis { get; set; } = axis;
        public double Velocity { get; set; } = velocity;
        public abstract Task<double> RiseDownAsync(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken);

        protected char GetAxisPoint(int axis)
        {
            switch (axis)
            {
                case 0:
                    return 'X';
                case 1:
                    return 'Y';
                case 2:
                    return 'Z';
            }
            return '\0';
        }

        public virtual async Task<double> MoveAsync(double startPoint, double endPoint, PointWrapper currentPoint, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double currentPosition = await CalculateDistance(currentPoint, startPoint, endPoint, cancellationToken);
            stopwatch.Stop();
            Log.Info($"Axis {GetAxisPoint(Axis)} " +
                $"selesai pada {stopwatch.Elapsed.TotalSeconds} " +
                $"jalur {startPoint}-{endPoint} " +
                $"posisi {GetAxisPoint(Axis)}_{currentPoint.CurrentPoint.GetAxisPoint(Axis)}");
            return startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition;
        }

        public async Task<double> CalculateDistance(PointWrapper currentPoint, double startPoint, double endPoint, CancellationToken cancellationToken)
        {
            double currentDistance = 0;
            double distance = Math.Abs(endPoint - startPoint);
            while (!cancellationToken.IsCancellationRequested && currentDistance < distance)
            {
                await Task.Delay(1000);
                currentDistance += (this.Velocity / 1);
                currentPoint.CurrentPoint.SetAxisPoint(startPoint < endPoint ? currentDistance + startPoint : startPoint - currentDistance, Axis);
                Log.Info($"Axis {GetAxisPoint(Axis)} menempuh jarak {currentDistance} dan berada pada titik {currentPoint.CurrentPoint.GetAxisPoint(Axis)}");
            } 
            return (currentDistance);
        }
    }
}
