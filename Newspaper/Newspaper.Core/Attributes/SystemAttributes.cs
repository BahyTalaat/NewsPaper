
using Newspaper.Core.Helper;
using Newspaper.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CustomEnumAttribute : Attribute
	{
		public string DisplayText { set; get; }
		public string Description { set; get; }
		public string Abbreviation { set; get; }
		public string GroupBy { get; set; }
		public Type ResourceType { get; set; }

		public CustomEnumAttribute()
		{
		}
		public CustomEnumAttribute(Type ResourceType, string DisplayTextName = "", string DescriptionName = "", string AbbreviationName = "", string GroupByName = "")
		{
			if (!DisplayTextName.IsNullOrEmptyWithTrim())
				DisplayText = ResourceHelper.GetResourceLookup(ResourceType, DisplayTextName);

			if (!DescriptionName.IsNullOrEmptyWithTrim())
				Description = ResourceHelper.GetResourceLookup(ResourceType, DescriptionName);

			if (!AbbreviationName.IsNullOrEmptyWithTrim())
				Abbreviation = ResourceHelper.GetResourceLookup(ResourceType, AbbreviationName);

			if (!GroupByName.IsNullOrEmptyWithTrim())
				GroupBy = ResourceHelper.GetResourceLookup(ResourceType, GroupByName);
		}

	}
}
