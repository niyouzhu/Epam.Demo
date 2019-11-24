using System.Reflection;

namespace Eric.DotNetCore.Interception
{
    internal static class MethodInfoExtensions
    {
        public static bool Matches(this MethodInfo method1, MethodInfo another)
        {
            if (method1.Name != another.Name)
            {
                return false;
            }

            var parameters1 = method1.GetParameters();
            var parameters2 = another.GetParameters();

            if (parameters1.Length != parameters2.Length)
            {
                return false;
            }
            for (int i = 0; i < parameters1.Length; i++)
            {
                if (parameters1[i].ParameterType != parameters2[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }
    }
}