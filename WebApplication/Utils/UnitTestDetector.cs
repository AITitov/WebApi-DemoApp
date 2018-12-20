using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApplication.Utils
{
    public static class UnitTestDetector
    {
        private static bool _runningFromNUnit = false;
        static UnitTestDetector()
        {
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assem.FullName.ToLowerInvariant().Contains("test"))
                {
                    _runningFromNUnit = true;
                    break;
                }
            }
        }
        public static bool IsRunningFromNUnit
        {
            get { return _runningFromNUnit; }
        }
    }
}