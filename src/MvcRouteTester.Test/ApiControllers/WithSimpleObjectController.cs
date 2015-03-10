using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	[TypeConverter(typeof(SimpleInputModelConverter))]
	public class SimpleInputModel
	{
		public uint X { get; set; }
		public uint Y { get; set; }

		public override string ToString()
		{
			return X + "-" + Y;
		}

		public static SimpleInputModel Parse(string value)
		{
			var elements = value.Split('-');

			return new SimpleInputModel
			{
				X = UInt32.Parse(elements[0]),
				Y = UInt32.Parse(elements[1])
			};
		}
	}

	class SimpleInputModelConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var str = value as string;
			if (str != null)
			{
				return new InputModel { Name = str };
			}

			return base.ConvertFrom(context, culture, value);
		}
	}

	public class WithSimpleObjectController : ApiController
	{
		public HttpResponseMessage Get(SimpleInputModel data)
		{
			return new HttpResponseMessage();
		}
	}
}
