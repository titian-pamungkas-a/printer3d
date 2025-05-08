using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public sealed class MotorManager
    {
        public Motor[] motors { get; set; }
        public Point currentPoint { get; set; }
        public Point currentPoint1 { get; set; } = new Point(0, 0, 0);
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private Task<double>[] tasks;

        public MotorManager(double xVelocity, double yVelocity, double zVelocity)
        {
            this.motors = new Motor[]
            {
                new MotorX((int)Axis.X, xVelocity, 2),
                new MotorY((int)Axis.Y, yVelocity, 2),
                new MotorZ((int)Axis.Z, zVelocity)
            };
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public async Task Move(Point startPoint, List<Point> points, Motor[] motors)
        {
            this.currentPoint = startPoint;
            PointWrapper pointwrapper = new PointWrapper(currentPoint);
            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine($"Titik sekarang {currentPoint}");
                pointwrapper = new PointWrapper(currentPoint);
                double zInitialPosition = await ((MotorZ)motors[2]).RiseUpMotor(currentPoint.zAxis, pointwrapper, cancellationToken);
                double zPosition = await motors[2].MoveAsync(currentPoint.zAxis, points[i].zAxis, pointwrapper, cancellationToken);
                tasks = new Task<double>[]
                {
                    motors[0].MoveAsync(currentPoint.xAxis, points[i].xAxis, pointwrapper, cancellationToken),
                    motors[1].MoveAsync(currentPoint.yAxis, points[i].yAxis, pointwrapper, cancellationToken)
                };
                if (tasks.Any(t => t.IsCanceled))
                {
                    return;
                }
                try
                {
                    Task.WaitAll(tasks);
                }
                catch (Exception ex)
                {
                    //return;
                }
                finally
                {

                }
                Console.WriteLine("INI POINT DI AWALLL:" + pointwrapper.CurrentPoint.xAxis);
                //currentPoint = new Point(tasks[0].Result, tasks[1].Result, tasks[2].Result);
                Console.WriteLine($"Selesai di tujuan pada titik {pointwrapper.CurrentPoint.xAxis} {pointwrapper.CurrentPoint.yAxis} {pointwrapper.CurrentPoint.zAxis}");
            }
            Task.WaitAll(
                motors[0].RiseDownAsync(currentPoint.xAxis, pointwrapper, cancellationToken),
                motors[1].RiseDownAsync(currentPoint.yAxis, pointwrapper, cancellationToken),
                motors[2].RiseDownAsync(currentPoint.zAxis, pointwrapper, cancellationToken)
            );

        }

        public void SetMotorsVelocity(double velocity)
        {
            foreach (Motor motor in motors)
            {
                motor.Velocity = velocity;
            }
        }

        public void SetXPoint(double xPoint)
        {
            //Console.WriteLine($"Check jumlah pont {xPoint}" );
            this.currentPoint.xAxis = xPoint;
        }
        public void SetYPoint(double yPoint) => this.currentPoint.yAxis = yPoint;
        public void SetZPoint(double zPoint) => this.currentPoint.zAxis = zPoint;
    }

    public class PointWrapper(Point point)
    {
        public Point CurrentPoint { get; set; } = point;
    }
}
