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
        private PointWrapper pointwrapper;

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

        public async Task Stop()
        {
            cancellationTokenSource.Cancel();
            await BackToIdle(pointwrapper);
        }

        public async Task Move(Point startPoint, List<Point> points, Motor[] motors)
        {
            this.currentPoint = startPoint;
            pointwrapper = new PointWrapper(currentPoint);
            double zInitialPosition = await ((MotorZ)motors[2]).RiseUpMotor(currentPoint.zAxis, pointwrapper, cancellationToken);
            for (int i = 0; i < points.Count; i++)
            {
                Log.Info($"Titik sekarang {currentPoint}");
                pointwrapper = new PointWrapper(currentPoint);
                double zPosition = await motors[2].MoveAsync(currentPoint.zAxis, points[i].zAxis, pointwrapper, cancellationToken);
                try
                {
                    Task.WaitAll(
                        motors[0].MoveAsync(currentPoint.xAxis, points[i].xAxis, pointwrapper, cancellationToken),
                        motors[1].MoveAsync(currentPoint.yAxis, points[i].yAxis, pointwrapper, cancellationToken)
                     );
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                    return;
                }
                Console.WriteLine($"Selesai di tujuan pada titik {pointwrapper.CurrentPoint.xAxis} {pointwrapper.CurrentPoint.yAxis} {pointwrapper.CurrentPoint.zAxis}");
            }
            await BackToIdle(pointwrapper);
        }

        private async Task BackToIdle(PointWrapper pointwrapper)
        {
            await ((MotorZ)motors[2]).RiseUpMotor(currentPoint.zAxis, pointwrapper, cancellationToken);
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
