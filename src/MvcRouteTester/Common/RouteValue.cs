namespace MvcRouteTester.Common
{
	public class RouteValue
	{
		public string Name { get; set; }
		public bool FromBody { get; set; }
		public object Value { get; set; }

		public string ValueAsString
		{
			get
			{
				return Value == null ? string.Empty : Value.ToString();
			}
		}
	}
}