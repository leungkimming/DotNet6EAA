using System;
using System.Linq;
using System.Reflection;

namespace P6.StoryTest {

    public static class ReflectHelper {
        public static void SetValue(object instance, string propertyName, object value) {
            Type type = instance.GetType();
            PropertyInfo propInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            propInfo.SetValue(instance, value);
        }

        public static object GetValue(object instance, string propertyName) {
            Type type = instance.GetType();
            PropertyInfo propInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return propInfo.GetValue(instance);
        }


        // for static
        public static void SetValue(Type type, string propertyName, object value) {
            PropertyInfo propInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            propInfo.SetValue(null, value);
        }

        // for static
        public static object GetValue(Type type, string propertyName) {
            PropertyInfo propInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return propInfo.GetValue(null);
        }


        public static object MakeGenericMethodCall(object instance, MethodInfo methodInfo, Type genericType, params object[] parameters) {
            return methodInfo.MakeGenericMethod(genericType).Invoke(instance, parameters);

        }


        public static Type GetTypeFromAssembly(Assembly assembly, string typeName) {
            return assembly.GetTypes().FirstOrDefault(o => o.Name == typeName);
        }


        public static object MakeMethodCall(object instance, string propertyName, params object[] parameters) {
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(propertyName);
            return method.Invoke(instance, parameters);
        }


        public static object ConstructObject(Type type, params object[] parameters) {
            return Activator.CreateInstance(type, parameters);
        }

    }

}
