using System;

namespace BlueBit.HR.Docs.BL.BusinessLayer
{
    public class BusinessException : Exception
    {
    }

    public class LogonBusinessException : BusinessException
    {
        public static void Throw() { throw new LogonBusinessException(); }
    }
}
