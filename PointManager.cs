using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public class PointFactory
    {
        public static List<Point> points { get; set; } = new List<Point>();
        public PointFactory()
        {
            
            
        }

        public static async Task<List<Point>> readPointData()
        {
            Log.Info("Mengambil data sequence dari ");
            try
            {
                await Task.Delay(10);
                for (int i = 1; i <= 4; i++)
                {
                    points.Add(new Point(6 * i, 6 * i, 6 * i));
                }
                points[1].xAxis = 18;
                points[1].yAxis = 18;
                points[1].zAxis = 18;
                points[2].xAxis = 12;
                points[2].yAxis = 12;
                points[2].zAxis = 12;
                Log.Info("Berhasil mengambil data sequence dari ");
                return points;
            }
            catch (Exception ex)
            {
                Log.Info(ex.Message);
                return new List<Point>();
            }
            
        }
    }
}
