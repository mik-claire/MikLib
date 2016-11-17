using System;
using System.Collections.Generic;
using System.Reflection;

namespace MikLib.Model
{
	public class ComparableModel
	{
		public bool IsSameData(ComparableModel target, params string[] ignoreProperyNames)
		{
			Type type = this.GetType();
			PropertyInfo[] properties = type.GetProperties();
			Console.WriteLine("type: {0}", type.Name);

			foreach (PropertyInfo pi in properties)
			{
				Console.WriteLine("  property: {0}", pi.Name);

				bool hit = false;
				foreach (string name in ignoreProperyNames)
				{
					if (name != pi.Name)
					{
						continue;
					}

					hit = true;
					break;
				}
				if (hit)
				{
					Console.WriteLine("    continue.");
					continue;
				}

				object currentVaule = pi.GetValue(this, null);
				object targetValue = pi.GetValue(target, null);

				Console.WriteLine("    current: {0}", currentVaule);
				Console.WriteLine("    target : {0}", targetValue);

				if (currentVaule.Equals(targetValue))
				{
					continue;
				}

				Console.WriteLine();
				return false;
			}

			Console.WriteLine();
			return true;
		}
	}
}
