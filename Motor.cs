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
    public class Motor(int axis, double velocity)
    {
        public int Axis { get; set; } = axis;
        public double Velocity { get; set; } = velocity;
        public double Acceleration { get; set; }

        public async Task<double> MoveAsync(double startPoint, double endPoint, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            double currentPosition = await CalculateDistance(Math.Abs(startPoint - endPoint), cancellationToken);
            stopwatch.Stop();
            Console.WriteLine($"Axis {this.Axis} sampai pada tujuan dalam waktu {stopwatch.Elapsed.TotalMilliseconds} {currentPosition} {startPoint} {endPoint}");
            SetPointAxis(startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition, this.Axis);
            return startPoint < endPoint ? currentPosition + startPoint : startPoint - currentPosition;
        }

        private async Task<double> CalculateDistance(double distance, CancellationToken cancellationToken)
        {
            double currentDistance = 0;
            while (!cancellationToken.IsCancellationRequested && currentDistance < distance)
            {
                await Task.Delay(1000);
                currentDistance += (this.Velocity / 1);
                Console.WriteLine($"Axis {this.Axis} menempuh jarak {currentDistance}");
                Console.WriteLine($"ke 2 {this.Axis} {this.Velocity} {distance} {currentDistance}");
            } 
            return (currentDistance);
        }

        private void SetPointAxis(double distance, double axis)
        {
            Console.WriteLine($"axisnta {axis}");
            switch(axis)
            {
                case 0:
                    MotorManager.Instance.SetXPoint(distance); 
                    break;
                case 1:
                    MotorManager.Instance.SetYPoint(distance); 
                    break;
                case 2:
                    MotorManager.Instance.SetZPoint(distance); 
                    break;  
            }
        }
        public double Stop() { return Acceleration; }
    }
}
