using System.Linq;

namespace DawWorkflowBase.Extensions
{
    public static class Extensions
    {
        public static bool In<T>(this T o, params T[] ps)
        {
            if (ps.Contains(o))
                return true;
            return false;
        }
    }



}
