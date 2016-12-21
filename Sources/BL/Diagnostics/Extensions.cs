using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueBit.HR.Docs.BL.Diagnostics
{
    public static class Extensions
    {
        public static string ToDebugString<T>(this T obj)
        {
            return string.Format("({0})", obj.GetType().Name);
        }
        public static string ToDebugString<T>(this T obj, string format, params object[] parametry)
        {
            return string.Format("({0}:{1})", obj.GetType().Name, string.Format(format, parametry));
        }
        public static string ToDebugString<TValue>(this ICollection<TValue> obj)
        {
            if (obj == null) return "null";
            return string.Join(";", obj.Select((i) => i.ToString()).ToArray());
        }
        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> obj)
        {
            if (obj == null) return "null";
            return string.Join(";", obj.Select((i) => i.Key.ToString() + "=>" + i.Value.ToString()).ToArray());
        }

        private static readonly string _formatWyjatekZeStosem = "{0}{1}{2}";
        private static readonly string _formatWyjatekBezStosu = "{0}";

        public static string ToDebugString(this Exception e, bool addStack)
        {
            var msg = new System.Text.StringBuilder();
            for (; e != null; e = e.InnerException)
            {
                if (msg.Length > 0)
                {
                    msg.AppendLine();
                    msg.AppendLine("-->");
                }
                msg.AppendFormat(addStack ? _formatWyjatekZeStosem : _formatWyjatekBezStosu, e.Message, Environment.NewLine, e.StackTrace);
            }
            return msg.ToString();
        }
    }
}
