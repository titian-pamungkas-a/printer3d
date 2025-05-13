// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using NLog;
using printer3d;
using System;
using System.Diagnostics;


internal class Program
{
    private static async Task Main(string[] args)
    {
        Status currentStatus = Status.Idle;
        MotorManager motorManager = new MotorManager(1, 2, 3);
        Point startPoint = new Point(0, 0, 0);
        List<Point> points = await PointFactory.readPointData();

        while (true)
        {
            var userInput = Console.ReadKey(true);
            if (userInput.Key == ConsoleKey.Escape && currentStatus == Status.Idle)
            {
                Log.UserAction("User telah menghentikan Program");
                break;
            }
            if (userInput.Key == ConsoleKey.Enter && currentStatus == Status.MoveIn)
            {
                Log.UserAction("User menghentikan motor. Tunggu proses berhenti");
                await motorManager.Stop();
                currentStatus = Status.Idle;
            }
            if (userInput.Key == ConsoleKey.Enter && currentStatus == Status.Idle)
            {
                Log.UserAction("User menjalankan motor");
                currentStatus = Status.MoveIn;
                Task task = Task.Run(() => motorManager.Move(startPoint, PointFactory.points, motorManager.motors));
            }
        }
    }
}