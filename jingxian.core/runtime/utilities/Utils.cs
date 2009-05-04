
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;
using Microsoft.Win32;
using System.Text;

namespace jingxian.core.runtime
{
	public static class Utils
	{
        public static string Join<T>(string separator, IEnumerable<T> objects)
        {
            StringBuilder sb = new StringBuilder();
            Join<T>(sb, separator, objects);
            return sb.ToString();
        }

        public static void Join<T>( StringBuilder sb, string separator, IEnumerable<T> objects)
        {
            bool isFirst = true;

            foreach (T obj in objects)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    sb.Append(separator);
                }

                sb.Append(obj);
            }
        }


        //public static string Join<T>(string separator, IEnumerable<T> objects, Action<T> )
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //    bool isFirst = true;

        //    foreach (T obj in objects)
        //    {
        //        if (isFirst)
        //        {
        //            isFirst = false;
        //        }
        //        else
        //        {
        //            sb.Append(separator);
        //        }

        //        sb.Append(obj);
        //    }
        //    return sb.ToString();
        //}

        public static object invokeGetter(object instance, string key)
        {
            Type instanceType = instance.GetType();
            PropertyInfo propertyInfo = instanceType.GetProperty(key);
            if (null != propertyInfo)
            {
                if (!propertyInfo.CanRead)
                    return null;

                return propertyInfo.GetValue(instance, null);
            }
            else
            {
                FieldInfo fieldInfo = instanceType.GetField(key);
                if (null != fieldInfo)
                    return propertyInfo.GetValue(instance, null);

                return null;
            }
        }

        public static object invokeSetter(object instance, string key, object value, IConvertSystem converter)
        {
            Type instanceType = instance.GetType();

            PropertyInfo propertyInfo = instanceType.GetProperty(key);
            if (null != propertyInfo)
            {
                if (!propertyInfo.CanWrite)
                    return null;

                object oldValue = (propertyInfo.CanRead) ? (propertyInfo.GetValue(instance, null) ?? value) : value;

                propertyInfo.SetValue(instance, (null == converter) ? value : converter.ConvertTo(value, propertyInfo.PropertyType, null), null);

                return oldValue;
            }
            else
            {

                FieldInfo fieldInfo = instanceType.GetField(key);
                if (null == fieldInfo)
                    return null;

                object oldValue = propertyInfo.GetValue(instance, null) ?? value;

                fieldInfo.SetValue(instance, (null == converter) ? value : converter.ConvertTo(value, propertyInfo.PropertyType, null));
                return oldValue;
            }
        }

        public static string GetAsString(IProperties properties, string key)
        {
            object r = properties[key];
            return (null == r) ? string.Empty : r.ToString();
        }

        public static int GetAsInteger(IProperties properties, string key, int defaultValue)
        {
            object r = properties[key];
            if (null == r)
                return defaultValue;

            if (r.GetType() == typeof(int))
                return (int)r;

            int value = 0;
            if (int.TryParse(r.ToString(), out value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 创建type实例，将properties中的值注入到实例中
        /// </summary>
        /// <returns>创建的实例</returns>
        public static object CreateObjectFromProperites(IProperties properties, Type type, IConvertSystem convertSystem)
        {
            if (null == type)
                return null;

            if (null == properties)
                return null;

            if (!type.IsClass && !type.IsValueType)
                return null;

            if (type.IsAbstract)
                return null;

            ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (null == constructorInfo)
                return null;

            object instance = constructorInfo.Invoke(new object[] { });
            foreach (KeyValuePair<string, object> kp in properties)
            {
                invokeSetter(instance, kp.Key, kp.Value, convertSystem);
            }

            return instance;
        }

		public static string CreateCompositeId(string head, string tail)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", head, tail);
		}

		public static string CreateCompositeId(string head, int instanceCounter)
		{
			return CreateCompositeId(head, instanceCounter.ToString());
		}

		public static string[] GetAllTypeNames(object obj)
		{
			return GetAllTypeNames(obj.GetType());
		}

		public static string[] GetAllTypeNames(Type type)
		{
			List<string> names = new List<string>();

			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				names.Add(GetImplementationName(interfaces[i]));
			}

            if (type.IsInterface)
			{
				names.Add(GetImplementationName(type));
			}
			else
			{
				AddClassNames(type, names);
			}
			SimplifyGenericTypeNames(names);

			string typeName = GetImplementationName(type);
			if (!names.Contains(typeName))
			{
				names.Add(typeName);
			}

			return names.ToArray();
		}

		private static void AddClassNames(Type type, List<string> names)
		{
			if (type != typeof(object))
			{
				string implementation = GetImplementationName(type);
				names.Add(implementation);

				AddClassNames(type.BaseType, names);
			}
		}

		public static void SimplifyGenericTypeNames(List<string> names)
		{
			for (int i = 0; i < names.Count; i++)
			{
				names[i] = SimplifyGenericTypeName(names[i]);
			}
		}

		public static string SimplifyGenericTypeName(string name)
		{
			Regex regex = new Regex(@"(?<Version>, Version=.*?[^\]])\]", RegexOptions.Singleline | RegexOptions.Compiled);
			MatchCollection matches = regex.Matches(name);
			foreach (Match match in matches)
			{
				string s = match.Result("${Version}");
				name = name.Replace(s, string.Empty);
			}
			return name;
		}

		public static string GetImplementationName(Type type)
		{
            Enforce.ArgumentNotNull<Type>( type, "type"); 

			string name = string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
			if (type.IsGenericType)
			{
				name = SimplifyGenericTypeName(name);
			}
			return name;
		}

		public static string GetImplementationName<T>()
		{
			return GetImplementationName(typeof(T));
		}

		public static string GetAssemblyFileNameFromLocation(string assemblyLocation)
		{
			FileInfo fileInfo = new FileInfo(assemblyLocation);
			return fileInfo.Name;
		}

		public static string GetAssemblySimpleNameFromLocation(string assemblyLocation)
		{
			string fileName = GetAssemblyFileNameFromLocation(assemblyLocation);
			Regex regex =
				new Regex(@".(dll|exe)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

			if (regex.Match(fileName).Success)
			{
				return regex.Replace(fileName, "");
			}
			else
			{
				return string.Empty;
			}
		}

		public static Attribute[] GetCustomAttributes(Assembly asm, bool inherit)
		{
			return asm == null ? new Attribute[0] : ConvertToAttributeArray(asm.GetCustomAttributes(inherit));
		}

		public static TAttribute[] GetCustomAttributes<TAttribute>(Assembly asm, bool inherit) where TAttribute: Attribute
		{
			return asm == null ? new TAttribute[0] : ConvertToAttributeArray<TAttribute>(asm.GetCustomAttributes(inherit));
		}

		public static Attribute[] GetCustomAttributes(object obj, bool inherit)
		{
			return obj == null ? new Attribute[0] : GetCustomAttributes(obj.GetType(), inherit);
		}

		public static TAttribute[] GetCustomAttributes<TAttribute>(object obj, bool inherit) where TAttribute: Attribute
		{
			return obj == null ? new TAttribute[0] : GetCustomAttributes<TAttribute>(obj.GetType(), inherit);
		}

		public static Attribute[] GetCustomAttributes(Type type, bool inherit)
		{
			return type == null ? new Attribute[0] : ConvertToAttributeArray(type.GetCustomAttributes(inherit));
		}

		public static TAttribute[] GetCustomAttributes<TAttribute>(Type type, bool inherit) where TAttribute: Attribute
		{
			return type == null ? new TAttribute[0] : ConvertToAttributeArray<TAttribute>(type.GetCustomAttributes(inherit));
		}

		public static Attribute[] GetCustomAttributes(MemberInfo info, bool inherit)
		{
			return info == null ? new Attribute[0] : ConvertToAttributeArray(info.GetCustomAttributes(inherit));
		}

		public static TAttribute[] GetCustomAttributes<TAttribute>(MemberInfo info, bool inherit) where TAttribute: Attribute
		{
			return info == null ? new TAttribute[0] : ConvertToAttributeArray<TAttribute>(info.GetCustomAttributes(inherit));
		}
		private static Attribute[] ConvertToAttributeArray(object[] attribs)
		{
			Attribute[] attributes = new Attribute[attribs.Length];
			for (int i = 0; i < attribs.Length; i++)
			{
				attributes[i] = (Attribute) attribs[i];
			}
			return attributes;
		}

		private static TAttribute[] ConvertToAttributeArray<TAttribute>(object[] attribs) where TAttribute: Attribute
		{
			List<TAttribute> attributes = new List<TAttribute>();
			for (int i = 0; i < attribs.Length; i++)
			{
				TAttribute attr = attribs[i] as TAttribute;
				if (attr != null)
				{
					attributes.Add(attr);
				}
			}
			return attributes.ToArray();
		}


		public static TAttribute GetCustomAttribute<TAttribute>(Assembly asm, bool inherit)
	        where TAttribute: Attribute
		{
			TAttribute attr;
			if (!TryGetCustomAttribute(asm, out attr, inherit))
				throw new ArgumentException(string.Format( "在程序集 '{0}' 中没有找到属性'{1}'.", asm, typeof(TAttribute)));
			return attr;
		}

		public static TAttribute GetCustomAttribute<TAttribute>(Type type, bool inherit)
			where TAttribute: Attribute
		{
			TAttribute attr;
			if (!TryGetCustomAttribute(type, out attr, inherit))			
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "在类型 '{0}' 中没有找到属性'{1}'.", type, typeof(TAttribute)));
			return attr;
        }

		public static TAttribute GetCustomAttribute<TAttribute>(MemberInfo member, bool inherit)
			where TAttribute: Attribute
		{
			TAttribute attr;
			if (!TryGetCustomAttribute(member, out attr, inherit))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "在成员 '{0}' 中没有找到属性'{1}'.", member, typeof(TAttribute)));

				return attr;
		}

		public static bool TryGetCustomAttribute<TAttribute, TTarget>(out TAttribute attr, bool inherit)
			where TAttribute: Attribute
		{
			return TryGetCustomAttribute(typeof(TTarget), out attr, inherit);
		}

		public static bool TryGetCustomAttribute<TAttribute>(Assembly asm, out TAttribute attr, bool inherit)
			where TAttribute: Attribute
		{
			return IsAttributeTypeDefined(GetCustomAttributes(asm, inherit), out attr);
		}

		public static bool TryGetCustomAttribute<TAttribute>(Type type, out TAttribute attr, bool inherit)
			where TAttribute: Attribute
		{
			return IsAttributeTypeDefined(GetCustomAttributes(type, inherit), out attr);
		}

		public static bool TryGetCustomAttribute<TAttribute>(MemberInfo member, out TAttribute attr, bool inherit)
			where TAttribute: Attribute
		{
			return IsAttributeTypeDefined(GetCustomAttributes(member, inherit), out attr);
		}

		private static bool IsAttributeTypeDefined<TAttribute>(Attribute[] attributes, out TAttribute attr)
			where TAttribute: Attribute
		{
			attr = null;
			for (int i = 0; i < attributes.Length; i++)
			{
				if (typeof(TAttribute).IsAssignableFrom(attributes[i].GetType()))
				{
					attr = attributes[i] as TAttribute;
					return true;
				}
			}
			return false;
		}


		public static bool EnsureDirectoryExists(string directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			return Directory.Exists(directory);
		}

		public static Type GetGreatestCommonType(IEnumerable objects)
		{
			Type commonType = default(Type);
			foreach (object o in objects)
			{
				if (commonType == default(Type))
				{
					commonType = o.GetType();
				}
				else
				{
					Type type = o.GetType();
					commonType = FindGreatestCommonType(type, commonType);
					continue;
				}
			}
			return commonType;
		}

		public static Type FindGreatestCommonType(Type type, Type commonType)
		{
            Enforce.ArgumentNotNull<Type>(type, "type");
            Enforce.ArgumentNotNull<Type>(commonType, "commonType"); 

			if (type == typeof(object) || commonType == typeof(object))
				return typeof(object);

			if (type == commonType || commonType.IsAssignableFrom(type))
			{
				return commonType;
			}
			else if (type.IsAssignableFrom(commonType))
			{
				return type;
			}
			else
			{
				Type candidate = FindGreatestCommonTypeInHierarchyUpwards(type, commonType);
				if (candidate != typeof(object) && candidate.IsAssignableFrom(commonType))
				{
					return candidate;
				}
				else
				{
					Type[] commonTypeInterfaces = commonType.GetInterfaces();
					foreach (Type typeInterface in commonTypeInterfaces)
					{
						if (typeInterface.IsAssignableFrom(type))
						{
							candidate = typeInterface;
						}
					}
					return candidate;
				}
			}
		}

		public static Type FindGreatestCommonTypeInHierarchyUpwards(Type type, Type commonType)
		{
			if (type.BaseType != typeof(object))
			{
				if (type.BaseType.IsAssignableFrom(commonType))
				{
					return type.BaseType;
				}
				else
				{
					return FindGreatestCommonTypeInHierarchyUpwards(type.BaseType, commonType);
				}
			}
			return typeof(object);
		}

		public static int GetCount(IEnumerable enumerable)
		{
			ICollection collection = enumerable as ICollection;
			if (collection != null)
			{
				return collection.Count;
			}
			else
			{
				IEnumerator enumerator = enumerable.GetEnumerator();
				int count = 0;
				while (enumerator.MoveNext())
				{
					count++;
				}
				return count;
			}
		}

		public static bool Contains(IEnumerable enumerable, object element)
		{
			IList list = enumerable as IList;
			if (list != null)
			{
				return list.Contains(element);
			}
			else
			{
				foreach (object o in enumerable)
				{
					if (Equals(o, element))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void ReadAppSettings<T>(string key, SettingsBase settingsBase, T defaultValue)
		{
			if (typeof(T) == typeof(string))
			{
				string suppliedDefaultValue = defaultValue as string;
				string settingsDefaultValue = settingsBase[key] as string;
				if (string.IsNullOrEmpty(settingsDefaultValue))
				{
					string appSettingsValue = ConfigurationManager.AppSettings[key];
					settingsBase[key] = !string.IsNullOrEmpty(appSettingsValue)
						? appSettingsValue
						: suppliedDefaultValue;
				}
			}
			else if (typeof(T) == typeof(bool))
			{
				bool parsedValue;
				string appSettingsValue = ConfigurationManager.AppSettings[key];
				if (!string.IsNullOrEmpty(appSettingsValue)
					&& bool.TryParse(appSettingsValue, out parsedValue))
				{
					settingsBase[key] = parsedValue;
				}
				else
				{
					settingsBase[key] = defaultValue;
				}
			}
			else
			{
				throw new ArgumentException("不能识别的参数类型!.");
            }
		}

	}
}
