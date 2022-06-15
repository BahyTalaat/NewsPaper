using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Newspaper.Core.Helpers
{
	public class ResourceHelper
	{
		public static string GetResourceLookup(Type resourceType, string resourceName)
		{
			resourceName = resourceName.Trim();
			if ((resourceType != null) && (resourceName != null))
			{
				PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
				if (property == null)
				{
					return resourceName;  //throw new InvalidOperationException(string.Format("Resource Type Does Not Have Property"));
				}
				if (property.PropertyType != typeof(string))
				{
					return resourceName; ; // throw new InvalidOperationException(string.Format("Resource Property is Not String Type"));
				}
				return (string)property.GetValue(null, null);
			}
			return null;
		}
	}
}
