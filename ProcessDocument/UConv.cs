using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDocument
{
    public static class UConv
    {

        internal static object ConvertTo(object inputOb, Type t)
        {
            if (t == null)
                return inputOb;

            if (inputOb == null)
            {
                var underlyingType = Nullable.GetUnderlyingType(t);
                if (underlyingType != null)
                {
                    return Convert.ChangeType(inputOb, underlyingType);
                }
                else
                {
                    return Convert.ChangeType(inputOb, t);
                }
            }

            //if (t == typeof(int))
            //    return UConv.ConvertTo<int>(inputOb);
            //if (t == typeof(decimal))
            //    return UConv.ConvertTo<decimal>(inputOb);
            //if (t == typeof(double))
            //    return UConv.ConvertTo<double>(inputOb);
            //if (t == typeof(short))
            //    return UConv.ConvertTo<short>(inputOb);
            //if (t == typeof(long))
            //    return UConv.ConvertTo<long>(inputOb);
            //if (t == typeof(float))
            //    return UConv.ConvertTo<float>(inputOb);
            //if (t == typeof(string))
            //    return UConv.ConvertTo<string>(inputOb);
            if (inputOb == System.DBNull.Value)
                return null;

            var res = Convert.ChangeType(inputOb, t);

            return res;

        }

        /// <summary>
        /// Funkcja konwertuje dowolne typy danych
        /// </summary>
        /// <typeparam name="T">typ typu docelowego</typeparam>
        /// <param name="o">obiekt źródłowy</param>
        /// <returns></returns>
        internal static T ConvertTo<T>(object o, string format = "")
        {
            T res = default(T);
            if (o == null || o == DBNull.Value)
            {
                if (Nullable.GetUnderlyingType(typeof(T)) == null)
                    res = default(T);
                else
                    res = (T)(object)null;
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(typeof(T));
                if (underlyingType == null)
                    res = (T)Convert.ChangeType(o, typeof(T));
                else
                    res = (T)Convert.ChangeType(o, underlyingType);
            }
            return res;
        }

        public static bool TryCast<T>(this object obj, out T result)
        {
            result = default(T);
            if (obj is T)
            {
                result = (T)obj;
                return true;
            }

            try
            {
                result = ConvertTo<T>(obj);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
    }
}
