// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using printer3d;
using System;
using System.Diagnostics;


Status currentStatus = Status.Idle;
PointManager pointManager = new PointManager();
MotorManager motorManager = MotorManager.Instance;
Point startPoint = new Point(0, 0, 0);
List<Point> points = await pointManager.readPointData();
//motorManager.Move(startPoint, pointManager.points, motorManager.motors);

while (true)
{
    /*if (Console.KeyAvailable)
    {
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            //Console.WriteLine("B");
            Task task = Task.Run(() => motorManager.Move(startPoint, pointManager.points, motorManager.motors)); motorManager.Move(startPoint, pointManager.points, motorManager.motors);
        }
        else if(Console.ReadKey(true).Key == ConsoleKey.Escape)
        {
            Console.WriteLine("SABAR WOIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
        }
    }*/
    if (currentStatus == Status.Idle)
    {
        currentStatus = Status.MoveIn;
        Task task = Task.Run(() => motorManager.Move(startPoint, pointManager.points, motorManager.motors));
    }
    //Console.WriteLine("Test thread");
    var key = Console.ReadKey(true);
    if (!Console.KeyAvailable) motorManager.Stop();
}

async Task tryprint(string str)
{
    await Task.Delay(500);
    Console.WriteLine(str);
    return;
}


















/*
(double, double, double)[] points =
{
    (10, 10, 10),
    (20, 20, 20),
    (30, 30, 30),
    (40, 40, 40),
    (50, 50, 50),
    (60, 60, 60),
    (70, 70, 70),
    (80, 80, 80),
    (90, 90, 90),
};*/
(double, double, double) startPosition = (0, 0, 0), speed = (1, 2, 3);
//MoveAllMotorAsync(speed, startPosition, pointManager.points);
Console.WriteLine("SELESAI SEMUA");



void MoveAllMotorAsync((double, double, double) speed, (double, double, double) startPosition, List<Point> points)
{
    (double, double, double) currentPosition = startPosition;
    for (int i = 0; i < points.Count; i++)
    {
        Console.WriteLine($"Titik sekarang {currentPosition}");
        Task<double> xMotorTask = Task.Run(() => MoveMotorAsync("X", speed.Item1, currentPosition.Item1, points[i].xAxis));
        Task<double> yMotorTask = Task.Run(() => MoveMotorAsync("Y", speed.Item2, currentPosition.Item2, points[i].yAxis));
        Task<double> zMotorTask = Task.Run(() => MoveMotorAsync("Z", speed.Item3, currentPosition.Item3, points[i].zAxis));
        Task.WaitAll(xMotorTask, yMotorTask, zMotorTask);
        currentPosition = (points[i].xAxis, points[i].yAxis, points[i].zAxis);
        Console.WriteLine($"Selesai di tujuan pada titik {points[i]}");
    }

}

async Task<double> MoveMotorAsync(string axisName, double speed, double xStartPosition, double xFinishPosition)
{
    Stopwatch stopwatch = Stopwatch.StartNew(); 
    await calculateDistance(axisName, speed, Math.Abs(xFinishPosition - xStartPosition));
    stopwatch.Stop();
    Console.WriteLine($"Axis {axisName} sampai pada tujuan dalam waktu {stopwatch.Elapsed.TotalMilliseconds}");
    return (stopwatch.Elapsed.TotalMilliseconds);
}

async Task calculateDistance(string axisName, double speed, double distance)
{
    double xAbsolutePosition = 0;
    while (true)
    {
        await Task.Delay(1000);
        xAbsolutePosition += (speed / 1);
        Console.WriteLine($"Axis {axisName} menempuh jarak {xAbsolutePosition}");
        if (xAbsolutePosition >= distance) break;
    }
    return;
}


