using GameAnalyticsSDK.Wrapper;
using System.Collections.Generic;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Error
	{
		public static void NewEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			CreateNewEvent(severity, message, fields);
		}

		private static void CreateNewEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			GA_Wrapper.AddErrorEvent(severity, message, fields);
		}
	}
}
