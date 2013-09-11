namespace MvcRouteTester.Common
{
	public class RouteValue
	{
		public RouteValue(string name, object value, bool fromBody)
		{
			Name = name;
			Value = value;
			FromBody = fromBody;
		}

		public string Name { get; private set; }
		public object Value { get; private set; }
		public bool FromBody { get; private set; }

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