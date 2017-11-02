using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LoggerDSCF
{
    class Logger
    {
        private static Logger instance_ = null;
        private readonly string fileName = "";
        private Logger()
        {
            fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (fileName[fileName.Length - 1] != '\\')
            {
                fileName += "\\";
            }

            fileName += "DebugLog.txt";
        }

        public static Logger Instance
        {
            get
            {
                if (instance_ == null)
                {
                    instance_ = new Logger();
                }

                return instance_;
            }
        }

        public void WriteLog(string message)
        {
            StreamWriter fileStream = new StreamWriter(fileName, true);
            fileStream.WriteLine(message);
            fileStream.Flush();
            fileStream.Close();
            fileStream = null;
        }
    }
}
