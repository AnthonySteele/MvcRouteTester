namespace MvcRouteTester.Common
{
	public class RouteValue
	{
		public RouteValue(string name, object value, RouteValueOrigin origin)
		{
			Name = name;
			Value = value;
			Origin = origin;
		}

		public string Name { get; private set; }
		public object Value { get; private set; }
		public RouteValueOrigin Origin { get; private set; }

		public string ValueAsString
		{
			get
			{
				return Value == null ? string.Empty : Value.ToString();
			}
		}

		public override string ToString()
		{
			return Name + ": " + ValueAsString;
		}
	}
}