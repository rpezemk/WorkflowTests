using System;

namespace PHUP_Windykacje
{
    public static class UConv
    {
        public static void TransferField<T>(ref T o, object v)
        {
            if (o == null)
                o = default;

            var thisType = o.GetType();
            var inputType = v.GetType();

            if (inputType == typeof(DBNull))
            {
                o = default(T);
                return;
            }

            if (inputType == typeof(T))
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

        internal static T ConvertTo<T>(object o, string format = "")
        {
            if (o == null || o == DBNull.Value)
            {
                if (Nullable.GetUnderlyingType(typeof(T)) == null)
                    return default(T);
                else
                    return (T)(object)null;
            }
            else
            {
                var underlyingType = Nullable.GetUnderlyingType(typeof(T));
                if (underlyingType == null)
                {
                    try
                    {
                        return (T)Convert.ChangeType(o, typeof(T));
                    }
                    catch (Exception ex)
                    {
                        return default(T);
                    }
                }
                else
                    return (T)Convert.ChangeType(o, underlyingType);
            }
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

    }
}
