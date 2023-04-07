using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Moq.AutoMock;

public class AutoMocker
{
    private static readonly Dictionary<Type, Mock> Map;

    static AutoMocker()
    {
        Map = new Dictionary<Type, Mock>();
    }

    public static Mock<T> GetMock<T>() where T : class
    {
        if (!Map.ContainsKey(typeof(T)))
        {
            Map.Add(typeof(T), new Mock<T>());
        }

        return (Mock<T>)Map[typeof(T)];
    }

    public static T GetInstance<T>()
    {
        var repository = new MockRepository(MockBehavior.Loose);
        Type t = typeof(T);
        var constructor = ((TypeInfo)t).DeclaredConstructors.ToList()[0];
        var parameters = constructor.GetParameters();
        var list = new List<Mock>();
        foreach (var parameterInfo in parameters)
        {
            if (!Map.ContainsKey(parameterInfo.ParameterType))
            {
                MethodInfo? method = typeof(MockRepository).GetMethod(nameof(MockRepository.Create), Type.EmptyTypes);
                MethodInfo? generic = method?.MakeGenericMethod(parameterInfo.ParameterType);
                if (generic?.Invoke(repository, null) is Mock data)
                {
                    Map.Add(parameterInfo.ParameterType, data);
                }
            }

            list.Add(Map[parameterInfo.ParameterType]);
        }

        object[] parameter = new object[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            parameter[i] = list[i].Object;
        }
        
        return (T)Activator.CreateInstance(typeof(T), parameter, null)!;
    }
}