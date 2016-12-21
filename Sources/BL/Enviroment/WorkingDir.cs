using System;
using BlueBit.HR.Docs.BL.Diagnostics;

namespace BlueBit.HR.Docs.BL.Enviroment
{
    public class WorkingDir : 
        IDisposable
    {
        private static readonly log4net.ILog log = Log.GetInstance();

        private readonly string _path;
        public string Path { get { return _path; } }

        public WorkingDir()
        {
            _path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(_path);
        }

        public void Dispose()
        {
            try
            {
                System.IO.Directory.Delete(_path);
            }
            catch (Exception e)
            {
                log._TraceWarn(e);
            }
        }
    }
}
