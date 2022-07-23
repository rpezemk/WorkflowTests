using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDocument
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool NotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool In<T, T2>(this T source, IEnumerable<T2> iEnumerable)
        {
            T2 srcRes;
            if (source.TryCast<T2>(out srcRes))
            {
                if (iEnumerable.Contains(srcRes))
                {
                    return true;
                }
                else
                    return false;
            }
            //if (list.ToList().Contains()
            return false;
        }


        public static bool NotIn<T>(this T source, IEnumerable<T> iEnumerable)
        {
            T srcRes;
            if (source.TryCast<T>(out srcRes))
            {
                if (iEnumerable.Contains(srcRes))
                {
                    return false;
                }
                else
                    return true;
            }
            //if (list.ToList().Contains()
            return true;
        }

        public static bool IsNullOrEmpty<T>(this List<T> Source)
        {
            if (Source == null) return true;

            return !Source.Any();
        }

        public static bool In<T1>(this T1 src, ICollection<T1> list)
        {
            if (list.Contains(src))
                return true;
            return false;
        }


        public static bool StartsWithAnyOf(this string src, ICollection<string> list)
        {
            if (list.Where(s => src.StartsWith(s)).Any())
                return true;
            return false;
        }


        public static bool In(this int src, params G[] gs)
        {
            if (gs.Where(g => src == (int)g).Any())
                return true;
            return false;
        }


        public static bool In<T>(this T o, params T[] os)
        {
            if (os.Contains(o))
                return true;

            return false;
        }


    }
}
