using System;

namespace Tilegram.Feature
{
	public class RequestAttribute : Attribute
	{
		public string FieldName { get; set; }

		public RequestAttribute(string fieldName)
		{
			FieldName = fieldName;
		}
	}
}
