﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections;

namespace MikLib.Util
{
	public static class Debugger
	{
		/// <summary>
		/// Auther: Ruud
		/// </summary>
		public static string var_dump(object target, int recursion = 0)
		{
			StringBuilder result = new StringBuilder();

			// Protect the method against endless recursion
			if (recursion < 5)
			{
				// Determine object type
				Type targetType = target.GetType();

				// Get array with properties for this object
				PropertyInfo[] properties = targetType.GetProperties();

				foreach (PropertyInfo pi in properties)
				{
					try
					{
						// Get the property value
						object value = pi.GetValue(target, null);

						// Create indenting string to put in front of properties of a deeper level
						// We'll need this when we display the property name and value
						string indent = string.Empty;
						string spaces = "|   ";
						string trail = "|...";

						if (recursion > 0)
						{
							indent = new StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
						}

						if (value != null)
						{
							// If the value is a string, add quotation marks
							string displayValue = value.ToString();
							if (value is string)
							{
								displayValue = string.Concat('"', displayValue, '"');
							}

							// Add property name and value to return string
							result.AppendFormat("{0}{1} = {2}\n", indent, pi.Name, displayValue);

							try
							{
								if (!(value is ICollection))
								{
									// Call var_dump() again to list child properties
									// This throws an exception if the current property value
									// is of an unsupported type (eg. it has not properties)
									result.Append(var_dump(value, recursion + 1));
								}
								else
								{
									// 2009-07-29: added support for collections
									// The value is a collection (eg. it's an arraylist or generic list)
									// so loop through its elements and dump their properties
									int elementCount = 0;
									foreach (object element in ((ICollection)value))
									{
										string elementName = string.Format("{0}[{1}]", pi.Name, elementCount);
										indent = new StringBuilder(trail).Insert(0, spaces, recursion).ToString();

										// Display the collection element name and type
										result.AppendFormat("{0}{1} = {2}\n", indent, elementName, element.ToString());

										// Display the child properties
										result.Append(var_dump(element, recursion + 2));
										elementCount++;
									}

									result.Append(var_dump(value, recursion + 1));
								}
							}
							catch { }
						}
						else
						{
							// Add empty (null) property to return string
							result.AppendFormat("{0}{1} = {2}\n", indent, pi.Name, "null");
						}
					}
					catch
					{
						// Some properties will throw an exception on property.GetValue()
						// I don't know exactly why this happens, so for now i will ignore them...
					}
				}
			}

			return result.ToString();
		}
	}
}
