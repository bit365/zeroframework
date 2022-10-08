namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    public static class TypeExtensions
    {
        /// <summary>
        /// How To Detect If Type is Another Generic Type
        /// </summary>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            return givenType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericType) || givenType.BaseType != null && (givenType.BaseType.IsGenericType && givenType.BaseType.GetGenericTypeDefinition() == genericType || givenType.BaseType.IsAssignableToGenericType(genericType));
        }

        /// <summary>
        /// How To Detect If Type is Another Generic Type
        /// </summary>
        public static bool IsAssignableFromGenericType(this Type givenType, Type genericType) => IsAssignableToGenericType(genericType, givenType);

        /// <summary>
        /// Checks whether this type is a closed type of a given generic type.
        /// </summary>
        public static bool IsClosedTypeOf(this Type @this, Type openGeneric) => @this.GetInterfaces().Any(t => t.IsGenericType && !@this.ContainsGenericParameters && t.GetGenericTypeDefinition() == openGeneric);
    }
}