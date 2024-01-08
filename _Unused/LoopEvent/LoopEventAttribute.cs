using System.Reflection;

namespace HopeEngine;

/// <summary>
/// Marks static methods to be executed when <see cref="Looper.Start"/> is called, before or after according to the provided <see cref="LoopEventType"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class LoopEventAttribute : Attribute
{
    public LoopEventType EventType { get; }

    /// <param name="eventType">The event type for relevant the loop event method.</param>
    public LoopEventAttribute(LoopEventType eventType)
    {
        EventType = eventType;
    }

    /// <summary>
    /// Invokes all static methods with the attribute for the specified event type.
    /// </summary>
    internal static void InvokeMethodsOnAllInstances(LoopEventType eventType)
    {
        // Get all loaded assemblies
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                // Get all methods of the type
                MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                // Filter methods with the specified attribute and event type
                var methodsWithAttribute = methods
                    .Where(m => m.GetCustomAttributes(typeof(LoopEventAttribute), false)
                        .OfType<LoopEventAttribute>()
                        .Any(attr => attr.EventType == eventType));

                // Invoke static methods with the attribute and matching event type
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
