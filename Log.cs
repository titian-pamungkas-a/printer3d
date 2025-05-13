using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace printer3d
{
    public static class Log
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger(); 

        public static void Info<T>(T text)
        {
            Logger.Info(text);
        }

        public static void Error<T>(T text)
        {
            Logger.Error(text);
        }

        public static void UserAction<T>(T text)
        {
            Logger.Info($"<User Action> {text}");
        }

    }
}
