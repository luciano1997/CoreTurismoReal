using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TurismoReal.Class
{
    public class Functions
    {

        public T ReaderToValue<T>(object o)
        {
            object value = null;

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.Boolean:
                    if (o == DBNull.Value) value = false;
                    else value = o;
                    break;
                case TypeCode.Char:
                    break;
                case TypeCode.SByte:
                    break;
                case TypeCode.Byte:
                    break;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    if (o == DBNull.Value) value = 0;
                    else value = o;
                    break;
                case TypeCode.DateTime:
                    if (o == DBNull.Value) value = new DateTime();
                    else value = o;
                    break;
                case TypeCode.String:
                    if (o == DBNull.Value) value = string.Empty;
                    else value = o;
                    break;
                default:
                    value = o;
                    break;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

    }

    
}
