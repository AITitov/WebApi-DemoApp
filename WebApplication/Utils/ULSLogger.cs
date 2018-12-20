using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Utils
{
    // Логирование в ULS
    public class ULSLogger
    {
        public static void WriteLog(string category, TraceSeverity traceSeverity, EventSeverity eventSeverity, string message, string stackTrace)
        {
            SPSecurity.RunWithElevatedPrivileges(() => SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(category, traceSeverity, EventSeverity.Error), traceSeverity, message, stackTrace));
        }

        public static void WriteLog(string category, TraceSeverity traceSeverity, EventSeverity eventSeverity, string message)
        {
            WriteLog(category, traceSeverity, eventSeverity, message, string.Empty);
        }
        public static void WriteLog(string message)
        {
            WriteLog("WestConcept", TraceSeverity.Unexpected, EventSeverity.ErrorCritical, message, string.Empty);
        }
    }
}