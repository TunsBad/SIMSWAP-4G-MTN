using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace sim_swap.Extensions
{

    public class Logger
    {
        public static bool VerboseLogging { get; set; }
        private static readonly object Locker = new object();
        private readonly string _logDirectory;
        private string _infoLogDirectoryStatic;
        private string _errorLogDirectoryStatic;
        private string _warningLogDirectoryStatic;
        public IConfiguration _configuration;


        public Logger(IConfiguration configuration)
        {
            _configuration = configuration;
            _logDirectory = configuration.GetValue<string>("ERROR_LOG_DIRECTORY");
            _errorLogDirectoryStatic = _configuration.GetValue<string>("ERROR_LOG_DIRECTORY");
            _infoLogDirectoryStatic = _configuration.GetValue<string>("INFO_LOG_DIRECTORY");
            _warningLogDirectoryStatic = _configuration.GetValue<string>("WARNING_LOG_DIRECTORY");
        }

        public void logError(Exception x)
        {
            try
            {
                var dir = Path.Combine(_errorLogDirectoryStatic, DateTime.Now.ToString("yyyy_MM_dd"));
                CheckDir(dir);
                dir = Path.Combine(dir, DateTime.Now.ToString("HH"));
                CheckDir(dir);
                var message = x.ToString() + "\r\n" + x.StackTrace + "\r\n----------END----------";
                using (var sw = new StreamWriter(Path.Combine(dir,
                    string.Format("SimReg_error_{0}_{1}.log", DateTime.Now.ToString("HHmmss"),
                    new Random().Next(100000, 999999))), true))
                {
                    sw.WriteLine(message);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void logError(string msisdn, Exception x)
        {
            try
            {
                var dir = Path.Combine(_errorLogDirectoryStatic, DateTime.Now.ToString("yyyy_MM_dd"));
                CheckDir(dir);
                dir = Path.Combine(dir, DateTime.Now.ToString("HH"));
                CheckDir(dir);
                var message = x.ToString() + "\r\n" + x.StackTrace + "\r\n----------END----------";
                using (var sw = new StreamWriter(Path.Combine(dir,
                    string.Format("{0}_SimReg_error_{1}_{2}.log", msisdn, DateTime.Now.ToString("HHmmss"),
                    new Random().Next(100000, 999999))), true))
                {
                    sw.WriteLine(message);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void logError(string message)
        {
            try
            {
                var dir = Path.Combine(_errorLogDirectoryStatic, DateTime.Now.ToString("yyyy_MM_dd"));
                CheckDir(dir);
                dir = Path.Combine(dir, DateTime.Now.ToString("HH"));
                CheckDir(dir);
                message += "\r\n--------------------";
                using (var sw = new StreamWriter(Path.Combine(dir,
                    string.Format("SimReg_Error_{0}_{1}.log", DateTime.Now.ToString("HHmmss"),
                    new Random().Next(100000, 999999))), true))
                {
                    sw.WriteLine(message);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void logInfo(string message)
        {
            try
            {
                var dir = Path.Combine(_infoLogDirectoryStatic, DateTime.Now.ToString("yyyy_MM_dd"));
                CheckDir(dir);
                dir = Path.Combine(dir, DateTime.Now.ToString("HH"));
                CheckDir(dir);
                message += "\r\n--------------------";
                using (var sw = new StreamWriter(Path.Combine(dir,
                    string.Format("SimReg_Info_{0}_{1}.log", DateTime.Now.ToString("HHmmss"),
                    new Random().Next(100000, 999999))), true))
                {
                    sw.WriteLine(message);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void logInfo(string msisdn, string message)
        {
            try
            {
                var dir = Path.Combine(_infoLogDirectoryStatic, DateTime.Now.ToString("yyyy_MM_dd"));
                CheckDir(dir);
                dir = Path.Combine(dir, DateTime.Now.ToString("HH"));
                CheckDir(dir);
                message += "\r\n--------------------";
                using (var sw = new StreamWriter(Path.Combine(dir,
                    string.Format("{0}_SimReg_Info_{1}_{2}.log", msisdn, DateTime.Now.ToString("HHmmss"),
                    new Random().Next(100000, 999999))), true))
                {
                    sw.WriteLine(message);
                }
            }
            catch
            {
                // ignored
            }
        }

        public void logWarning(string message)
        {
            lock (Locker)
            {
                try
                {
                    var dir = Path.Combine(_warningLogDirectoryStatic, DateTime.Now.ToString("yyyy_MM_dd"));
                    CheckDirStatic(dir);
                    dir = Path.Combine(dir, DateTime.Now.ToString("HH"));
                    CheckDirStatic(dir);
                    message += "\r\n--------------------";
                    using (var sw = new StreamWriter(Path.Combine(dir,
                    string.Format("{2}SimReg_Warning_{0}_{1}.log", DateTime.Now.ToString("HHmmssfff"),
                    new Random().Next(100000, 999999), DateTime.Now.ToString("HHmmssfff"))), true))
                    {
                        sw.WriteLineAsync(message);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void CheckDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void CheckDirStatic(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
