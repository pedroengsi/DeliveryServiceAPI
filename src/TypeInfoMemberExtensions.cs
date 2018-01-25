namespace DeliveryServiceAPI
{
	#region Using Directives

	using System;
	using System.Collections.Generic;
	using System.Reflection;

	#endregion

	/// <summary>
	/// </summary>
	public static class TypeInfoMemberExtensions
	{
		/// <summary>
		/// </summary>
		public static IEnumerable<ConstructorInfo> GetAllConstructors(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredConstructors);

		/// <summary>
		/// </summary>
		public static IEnumerable<EventInfo> GetAllEvents(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredEvents);

		/// <summary>
		/// </summary>
		public static IEnumerable<FieldInfo> GetAllFields(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredFields);

		/// <summary>
		/// </summary>
		public static IEnumerable<MemberInfo> GetAllMembers(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredMembers);

		/// <summary>
		/// </summary>
		public static IEnumerable<MethodInfo> GetAllMethods(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredMethods);

		/// <summary>
		/// </summary>
		public static IEnumerable<TypeInfo> GetAllNestedTypes(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredNestedTypes);

		/// <summary>
		/// </summary>
		public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
			=> typeInfo.GetAll(ti => ti.DeclaredProperties);

		/// <summary>
		/// </summary>
		private static IEnumerable<T> GetAll<T>(this TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> accessor)
		{
			while (typeInfo != null)
			{
				foreach (var t in accessor(typeInfo))
				{
					yield return t;
				}

				typeInfo = typeInfo.BaseType?.GetTypeInfo();
			}
		}
	}
}