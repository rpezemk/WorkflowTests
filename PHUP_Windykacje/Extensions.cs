using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHUP_Windykacje
{
    public static class Extensions
    {
        public static void TransferField<T>(ref T o, object v) where T: struct
        {
            var thisType = o.GetType();
            var inputType = v.GetType();
            
            if(inputType == typeof(DBNull))
            {
                o = default(T);
                return;
            }
            
            if(inputType == typeof(T))
            {
                o = (T)v;
                return;
            }

            try
            {
                var res = UConv.ConvertTo<T>(v);
                o = (T)res;
                return;
            }
            catch { }
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

        public static object[] ToItemArray(this object source)
        {
            var props = source.GetType().GetProperties();
            var fields = source.GetType().GetFields().Where(f => f.IsPrivate == false);
            var c = props.Count();
            var res = props.Select(p => p.GetValue(source)).ToList();
            var res2 = fields.Select(p => p.GetValue(source)).ToList();
            res.AddRange(res2);
            return res.ToArray();
        }

        public static int GetNoOfFieldsAndProps(this Type type)
        {
            var n = type.GetProperties().Count();
            n += type.GetFields().Where(f => f.IsPrivate == false).Count();
            return n;
        }

        public static T ToObjectInstance<T>(this DataRow source) where T : new()
        {
            var res = new T();
            var props = res.GetType().GetProperties();
            int c = 0;
            foreach (var prop in props)
            {
                if (!source[prop.Name].In(new[] { DBNull.Value, null }))
                {
                    var val = source[prop.Name];
                    if (prop.GetSetMethod() != null)
                        prop.SetValue(res, UConv.ConvertTo(val, val.GetType()));
                }
                c++;
            }

            return res;
        }


        public static bool NotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        public static T To<T>(this object source)
        {
            try
            {
                var val = UConv.ConvertTo<T>(source);
                return val;
            }
            catch
            {
                return default(T);
            }
        }



        

        public static bool IsNullOrEmpty<T>(this List<T> Source)
        {
            if (Source == null) return true;

            return !Source.Any();
        }
    }
}
