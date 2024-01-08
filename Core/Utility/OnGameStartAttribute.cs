using System.Reflection;

namespace HopeEngine;

/// <summary>
/// All static methods with this attribute will be executed before the first frame.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class OnGameStartAttribute : Attribute
{
    /// <summary>
    /// Invoke all static methods with the attribute.
    /// </summary>
    internal static void InvokeMethodsOnAllInstances()
    {
        // Get all loaded assemblies
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Iterate through all types in all assemblies
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                // Get all methods of the type
                MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                // Filter methods with the specified attribute
                var methodsWithAttribute = methods
                    .Where(m => m.GetCustomAttributes(typeof(OnGameStartAttribute), false).Length > 0);

                // Invoke static methods with the attribute
                foreach (var method in methodsWithAttribute)
                {
                    if (!type.IsAbstract && !type.IsInterface)
                    {
                        var instance = Activator.CreateInstance(type);
                        method.Invoke(instance, null);
                    }
                }
            }
        }
    }
}