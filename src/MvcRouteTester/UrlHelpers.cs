using System;

namespace MvcRouteTester
{
	public class UrlHelpers
	{
		private const string DummySitePrefix = "http://site.com";

		public static string MakeAbsolute(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return String.Empty;
			}

			if (url.StartsWith("~"))
			{
				return DummySitePrefix + url.Substring(1);
			}

			if (url.StartsWith("/"))
			{
				return DummySitePrefix + url;
			}

			return url;
		}

		public static string PrependTilde(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return String.Empty;
			}

			if (url.StartsWith("/"))
			{
				return "~" + url;
			}

			return url;
		}
	}
}
