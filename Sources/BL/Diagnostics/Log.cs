using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace BlueBit.HR.Docs.BL.Diagnostics
{
    public static class Log
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static log4net.ILog GetInstance()
        {
            StackFrame frame = new StackFrame(2, false);
            return log4net.LogManager.GetLogger(frame.GetMethod().DeclaringType.FullName);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static log4net.ILog GetInstanceFor<T>()
        {
            StackFrame frame = new StackFrame(2, false);
            return log4net.LogManager.GetLogger(typeof(T).FullName);
        }
        public static void Configure(string fileName)
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            path = System.IO.Path.GetDirectoryName(path);
            path = System.IO.Path.Combine(path, fileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(path));
        }

        private class PropertyInfo
        {
            public Func<string> GetInfo;
            public override string ToString() { return GetInfo(); }
        }
        public static void AddPropertyInfo(string name, Func<string> getInfo)
        {
            Contract.Requires(getInfo != null);
            log4net.GlobalContext.Properties[name] = new PropertyInfo() { GetInfo = getInfo };
        }
    }


    public static class LogExtensions
    {
        private static readonly string _formatTraceIn = "» {0}({1})";
        private static readonly string _formatTraceOut = "« {0}({1})=>[{2}] ~T={3}";
        private static readonly string _formatTraceInfoStats = "@ {0}(…)" + Environment.NewLine + "{1}";
        private static readonly string _formatException = "! {0}(…)" + Environment.NewLine + "{1}";

        public interface ILogMethod :
            IDisposable
        {
            T ExecuteWithResult<T>(Func<T> action);
        }

        private class _LogMethod : 
            ILogMethod
        {
            private readonly Stopwatch sw = Stopwatch.StartNew();
            private readonly log4net.ILog log;
            private readonly string name;
            private readonly string parameters;
            private string result = string.Empty;

            public _LogMethod(log4net.ILog log, string name)
            {
                this.log = log;
                this.name = name;
                this.parameters = string.Empty;
                log.DebugFormat(_formatTraceIn, this.name, this.parameters);
            }

            public _LogMethod(log4net.ILog log, string name, string format, params object[] parameters)
            {
                this.log = log;
                this.name = name;
                this.parameters = string.Format(format, parameters);
                log.DebugFormat(_formatTraceIn, this.name, this.parameters);
            }

            public void Dispose()
            {
                log.DebugFormat(_formatTraceOut, name, parameters, result, sw.ElapsedMilliseconds);
            }

            public void Execute(Action action)
            {
                Contract.Requires(action != null);
                try { action(); }
                catch (Exception e) { log._TraceException(e); throw; }
            }

            public T ExecuteWithResult<T>(Func<T> action)
            {
                Contract.Requires(action != null);
                var result = action();
                this.result = string.Format("{0}", result);
                return result;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void _TraceEntry(this log4net.ILog log, Action action, int level = 0)
        {
            Contract.Requires(action != null);

            using (var _log = new _LogMethod(log, GetMethodName(level)))
                try { action(); }
                catch (Exception e) { log._TraceException(e); throw; }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T _TraceEntryWithResult<T>(this log4net.ILog log, Func<T> action, int level = 0)
        {
            Contract.Requires(action != null);

            using (var _log = new _LogMethod(log, GetMethodName(level)))
                try { return action(); }
                catch (Exception e) { log._TraceException(e); throw; }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ILogMethod _TraceMethod(this log4net.ILog log)
        {
            return new _LogMethod(log, GetMethodName());
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ILogMethod _TraceMethod(this log4net.ILog log, string format, params object[] parametry)
        {
            return new _LogMethod(log, GetMethodName(), format, parametry);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void _TraceInfo(this log4net.ILog log, string format, params object[] parameters)
        {
            format = string.Format(_formatTraceInfoStats, GetMethodName(), format);
            log.DebugFormat(format, parameters);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void _TraceWarn(this log4net.ILog log, Exception e)
        {
            var format = string.Format(_formatException, GetMethodName(), e.ToDebugString(true));
            log.WarnFormat(format, e);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void _TraceException(this log4net.ILog log, Exception e)
        {
            var format = string.Format(_formatException, GetMethodName(), e.ToDebugString(true));
            log.ErrorFormat(format, e);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetMethodName(int level = 0)
        {
            var st = new System.Diagnostics.StackTrace(2+level);
            var mt = st.GetFrame(0).GetMethod();
            return mt.Name;
        }
    }
}
