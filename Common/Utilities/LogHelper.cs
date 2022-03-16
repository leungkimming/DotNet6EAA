using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities {
    public static class LogHelper {
        public const string MessageSeparator = "||";

        public static string EventLogCR {
            get {
                return char.ConvertFromUtf32(13);
            }
        }

        private static void WriteToEventLog(string message, string stackTrace, string userId, int eventId) {
            Dictionary<string, object> extendedProperties = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(stackTrace)) {
                extendedProperties.Add("StackTrace", EventLogCR + stackTrace.Replace(MessageSeparator, EventLogCR + EventLogCR));
            }

            extendedProperties.Add("User ID", string.IsNullOrEmpty(userId) ? "UNKNOWN" : userId);

            string fullMessage = string.Format("{0}:{1}{2}", ServerSettings.SystemCode, EventLogCR, FormatEventLogMessage(message));

            foreach (KeyValuePair<string, object> entry in extendedProperties) {
                fullMessage += EventLogCR;
                fullMessage += string.Format("{0}:{1}", entry.Key, entry.Value.ToString());
            }

            EventLog.WriteEntry(ServerSettings.EventLogSource, fullMessage, EventLogEntryType.Error, eventId);
        }

        private static string FormatEventLogMessage(string message) {
            int maxMessageLength = 1500;
            string[] elements = message.Split(new string[] { MessageSeparator }, StringSplitOptions.None);

            for (int i = 0; i < elements.Length; i++) {
                if (elements[i].Length > maxMessageLength) {
                    elements[i] = elements[i].Substring(0, maxMessageLength);
                }
            }
            return string.Join(EventLogCR + EventLogCR, elements);
        }

        public static void WriteErrorLog(CustomException ex, string userId) {
            WriteErrorLog(ex.CustomError.Code, ex.CustomError.Message, ex.StackTrace, userId);
        }

        public static void WriteErrorLog(string code, string message, string stackTrace, string userId) {

            int EventId = 0;
            if (!string.IsNullOrEmpty(code) && int.TryParse(code, out EventId)) {
                EventId = int.Parse(code);
            } else {
                EventId = int.Parse(ErrorRegistry.X2301.Code);
            }

            WriteToEventLog(message, stackTrace, userId, EventId);
        }

    }
}
