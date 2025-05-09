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
                Log.Info($"Titik sekarang {currentPoint}");
                pointwrapper = new PointWrapper(currentPoint);
                double zInitialPosition = await ((MotorZ)motors[2]).RiseUpMotor(currentPoint.zAxis, pointwrapper, cancellationToken);
                double zPosition = await motors[2].MoveAsync(currentPoint.zAxis, points[i].zAxis, pointwrapper, cancellationToken);
                tasks = new Task<double>[]
                {
                    motors[0].MoveAsync(currentPoint.xAxis, points[i].xAxis, pointwrapper, cancellationToken),
                    motors[1].MoveAsync(currentPoint.yAxis, points[i].yAxis, pointwrapper, cancellationToken)
                };
                try
                {
                    Task.WaitAll(tasks);
                }
                catch (Exception ex)
                {
                    Log.Info(ex.Message);
                    await BackToIdle(pointwrapper);
                }
                finally
                {
                    Console.WriteLine($"Selesai di tujuan pada titik {pointwrapper.CurrentPoint.xAxis} {pointwrapper.CurrentPoint.yAxis} {pointwrapper.CurrentPoint.zAxis}");
                }
                
            }
            /*Task.WaitAll(
                motors[0].RiseDownAsync(currentPoint.xAxis, pointwrapper, cancellationToken),
                motors[1].RiseDownAsync(currentPoint.yAxis, pointwrapper, cancellationToken),
                motors[2].RiseDownAsync(currentPoint.zAxis, pointwrapper, cancellationToken)
            );*/
            await BackToIdle(pointwrapper);
        }

        private async Task BackToIdle(PointWrapper pointwrapper)
        {
            Task.WaitAll(
                motors[0].RiseDownAsync(currentPoint.xAxis, pointwrapper, CancellationToken.None),
                motors[1].RiseDownAsync(currentPoint.yAxis, pointwrapper, CancellationToken.None),
                motors[2].RiseDownAsync(currentPoint.zAxis, pointwrapper, CancellationToken.None)
            );
        }
    }

    public class PointWrapper(Point point)
    {
        public Point CurrentPoint { get; set; } = point;
    }
}
