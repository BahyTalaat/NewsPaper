
using Newspaper.Core.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Newspaper.Core.Helper
{
    public static class Helper
    {
        public static string ToJsonNS(object obj, bool handleRefLoop = true)
        {
            try
            {
                if (handleRefLoop)
                    return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                else
                    return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public static string GetDisplayText<EnumType>(EnumType value) where EnumType : struct, IConvertible, IComparable, IFormattable
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					CustomEnumAttribute attr =
						   Attribute.GetCustomAttribute(field,
							 typeof(CustomEnumAttribute)) as CustomEnumAttribute;
					if (attr != null)
					{
						return attr.DisplayText;
					}
				}
			}
			return null;
		}
		public static string GetDescription<EnumType>(EnumType value) where EnumType : struct, IConvertible, IComparable, IFormattable
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					CustomEnumAttribute attr =
						   Attribute.GetCustomAttribute(field,
							 typeof(CustomEnumAttribute)) as CustomEnumAttribute;
					if (attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}
		private static string GetGroupBy<EnumType>(EnumType value) where EnumType : struct, IConvertible, IComparable, IFormattable
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					CustomEnumAttribute attr =
						   Attribute.GetCustomAttribute(field,
							 typeof(CustomEnumAttribute)) as CustomEnumAttribute;
					if (attr != null)
					{
						return attr.GroupBy;
					}
				}
			}
			return null;
		}
		public static string GetAbbreviation<EnumType>(EnumType value) where EnumType : struct, IConvertible, IComparable, IFormattable
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					CustomEnumAttribute attr =
						   Attribute.GetCustomAttribute(field,
							 typeof(CustomEnumAttribute)) as CustomEnumAttribute;
					if (attr != null)
					{
						return attr.Abbreviation;
					}
				}
			}
			return null;
		}
		public static IEnumerable<dynamic> ConvertCustomEnumToList<EnumType>(bool twoIDs = true, bool orderByID = false, params EnumType[] exclude)
where EnumType : struct, IConvertible, IComparable, IFormattable
		{
			if (!typeof(EnumType).IsEnum)
				throw new ArgumentException("EnumType must be an Enumeration type");

			var type = typeof(EnumType);
			var memInfo = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
			var attributes = memInfo[0].GetCustomAttributes(true);
			var description = ((CustomEnumAttribute)attributes[0]).Description;

			if (twoIDs)
			{
				var list = Enum.GetValues(typeof(EnumType)).Cast<EnumType>()
					.Where(x => !exclude.Contains(x))
					.Select(x => new
					{
						ID = Convert.ToInt32(x),
						id = Convert.ToInt32(x),
						EnumName = x.ToString(),
						Name = GetDisplayText(x),
						DisplayText = GetDisplayText(x),
						Description = GetDescription(x),
						GroupBy = GetGroupBy(x),
						Abbreviation = GetAbbreviation(x)
					}).Where(x => x.ID != 0);

				if (orderByID == false)
					list = list.OrderBy(x => x.Name);
				else
					list = list.OrderBy(x => x.ID);

				var items = list.ToList();

				var item = Enum.GetValues(typeof(EnumType)).Cast<EnumType>()
					.Where(x => !exclude.Contains(x))
					.Select(x => new
					{
						ID = Convert.ToInt32(x),
						id = Convert.ToInt32(x),
						EnumName = x.ToString(),
						Name = GetDisplayText(x),
						DisplayText = GetDisplayText(x),
						Description = GetDescription(x),
						GroupBy = GetGroupBy(x),
						Abbreviation = GetAbbreviation(x)
					}).Where(x => x.ID == 0).OrderBy(x => x.Name).FirstOrDefault();
				if (item != null)
					items.Add(item);
				return items;
			}
			else
			{
				var Itemlist = Enum.GetValues(typeof(EnumType)).Cast<EnumType>()
					.Where(x => !exclude.Contains(x))
							   .Select(x => new
							   {
								   ID = Convert.ToInt32(x),
								   EnumName = x.ToString(),
								   Name = GetDisplayText(x),
								   DisplayText = GetDisplayText(x),
								   Description = GetDescription(x),
								   GroupBy = GetGroupBy(x),
								   Abbreviation = GetAbbreviation(x)
							   }).Where(x => x.ID != 0);

				if (orderByID == false)
					Itemlist = Itemlist.OrderBy(x => x.Name);
				else
					Itemlist = Itemlist.OrderBy(x => x.ID);

				var items = Itemlist.ToList();

				var item = Enum.GetValues(typeof(EnumType)).Cast<EnumType>()
					.Where(x => !exclude.Contains(x))
							   .Select(x => new
							   {
								   ID = Convert.ToInt32(x),
								   EnumName = x.ToString(),
								   Name = GetDisplayText(x),
								   DisplayText = GetDisplayText(x),
								   Description = GetDescription(x),
								   GroupBy = GetGroupBy(x),
								   Abbreviation = GetAbbreviation(x)
							   }).Where(x => x.ID == 0).OrderBy(x => x.Name).FirstOrDefault();
				if (item != null)
					items.Add(item);
				return items;
			}

		}

	}
}
