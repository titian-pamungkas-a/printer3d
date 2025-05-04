using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public sealed class MotorManager
    {
        private static MotorManager instance;
        private static object locker = new object();
        public Motor[] motors { get; set; }
        public Point currentPoint { get; set; }
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private Task<double>[] tasks;

        public static MotorManager Instance
        {
            get{
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new MotorManager(1, 2, 3);
                    }
                    return instance;
                }
            }
        }

        private MotorManager(double xVelocity, double yVelocity, double zVelocity)
        {
            this.motors = new Motor[]
            {
                new Motor((int)Axis.X, xVelocity),
                new Motor((int)Axis.Y, yVelocity),
                new Motor((int)Axis.Z, zVelocity)
            };
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public void Move(Point startPoint, List<Point> points, Motor[] motors)
        {
            this.currentPoint = startPoint;
            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine($"Titik sekarang {currentPoint}");
                tasks = new Task<double>[]
                {
                    Task.Run(() => motors[0].MoveAsync(this.currentPoint.xAxis, points[i].xAxis, cancellationToken), cancellationToken),
                    Task.Run(() => motors[1].MoveAsync(currentPoint.yAxis, points[i].yAxis, cancellationToken), cancellationToken),
                    Task.Run(() => motors[2].MoveAsync(currentPoint.zAxis, points[i].zAxis, cancellationToken), cancellationToken)
                };
                /*Task<double> xMotorTask = Task.Run(() => motors[0].MoveAsync(currentPoint.xAxis, points[i].xAxis, cancellationToken), cancellationToken);
                Task<double> yMotorTask = Task.Run(() => motors[1].MoveAsync(currentPoint.yAxis, points[i].yAxis, cancellationToken), cancellationToken);
                Task<double> zMotorTask = Task.Run(() => motors[2].MoveAsync(currentPoint.zAxis, points[i].zAxis, cancellationToken), cancellationToken);*/
                try
                {
                    Task.WaitAll(tasks);
                    currentPoint = currentPoint;
                }
                catch (Exception ex)
                {
                    foreach(var t in tasks)
                    {
                    if (t.IsCompletedSuccessfully)
                        Console.WriteLine($"Completed with result: {t.Result}");
                    else if (t.IsCanceled)
                        Console.WriteLine($"Task was canceled.");
                    else if (t.IsFaulted)
                        Console.WriteLine($"Task faulted: {t.Exception?.GetBaseException().Message}");
                    }
                    return;
                }

                //currentPoint = new Point(tasks[0].Result, tasks[1].Result, tasks[2].Result);
                Console.WriteLine($"Selesai di tujuan pada titik {tasks[0].Result} {tasks[1].Result} {tasks[2].Result}");
            }
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
            Console.WriteLine($"Check jumlah pont {xPoint}" );
            this.currentPoint.xAxis = xPoint;
        }
        public void SetYPoint(double yPoint) => this.currentPoint.yAxis = yPoint;
        public void SetZPoint(double zPoint) => this.currentPoint.zAxis = zPoint;
    }
}
