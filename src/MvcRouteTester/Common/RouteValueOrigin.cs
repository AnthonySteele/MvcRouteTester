namespace MvcRouteTester.Common
{
	public enum RouteValueOrigin
	{
		Unknown = 0,
		Path,
		Params,
		Body
	}

	public static class RouteValueOriginHelpers
	{
		public static bool Matches(RouteValueOrigin expected, RouteValueOrigin actual)
		{
			if (expected == RouteValueOrigin.Unknown)
			{
				return true;
			}

			return (expected == actual);
		}
	}
}