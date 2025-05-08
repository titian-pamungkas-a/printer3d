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

        public abstract Task<double> MoveAsync(double startPoint, double endPoint, PointWrapper currentPoint, CancellationToken cancellationToken);
        public abstract Task<double> RiseDownAsync(double startPoint, PointWrapper currentPoint, CancellationToken cancellationToken);


        public async Task<double> CalculateDistance(double startPoint, double endPoint, CancellationToken cancellationToken)
        {
            double currentDistance = 0;
            double distance = Math.Abs(endPoint - startPoint);
            while (!cancellationToken.IsCancellationRequested && currentDistance < distance)
            {
                await Task.Delay(1000);
                currentDistance += (this.Velocity / 1);
                Console.WriteLine($"Axis {this.Axis} menempuh jarak {currentDistance}");
                Console.WriteLine($"ke 2 {this.Axis} {this.Velocity} {distance} {currentDistance}");
            } 
            return (currentDistance);
        }

        internal double RiseDownAsync(double xAxis1, double xAxis2, object pointwrapper, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
