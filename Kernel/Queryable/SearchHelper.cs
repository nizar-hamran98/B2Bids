using System.Collections;
using System.Text.Json;

namespace SharedKernel
{
    internal static class SearchHelper
    {
        public static object? ConvertObjectToType(this object? TargetObject, Type TargetType)
        {
            if (TargetObject == null)
                return null;

            var _ObjType = TargetObject.GetType();

            switch (true)
            {
                case object _ when _ObjType == TargetType:
                    {
                        return TargetObject;
                    }

                case object _ when TargetType == typeof(bool):
                case object _ when TargetType == typeof(bool?):
                    {
                        return Convert.ToBoolean(TargetObject);
                    }

                case object _ when TargetType == typeof(long):
                case object _ when TargetType == typeof(long?):
                    {
                        return Convert.ToInt64(TargetObject);
                    }

                case object _ when TargetType == typeof(int):
                case object _ when TargetType == typeof(int?):
                    {
                        return Convert.ToInt32(TargetObject);
                    }

                case object _ when TargetType == typeof(double):
                case object _ when TargetType == typeof(double?):
                    {
                        return Convert.ToDouble(TargetObject);
                    }

                case object _ when TargetType == typeof(short):
                case object _ when TargetType == typeof(short?):
                    {
                        return Convert.ToInt16(TargetObject);
                    }

                case object _ when TargetType == typeof(byte):
                case object _ when TargetType == typeof(byte?):
                    {
                        return Convert.ToByte(TargetObject);
                    }

                case object _ when TargetType == typeof(DateTime):
                case object _ when TargetType == typeof(DateTime?):
                    {
                        //here we go
                        return DateTime.SpecifyKind(DateTime.Parse(TargetObject.ToString()), DateTimeKind.Utc);
                        //return DateTime.Parse(TargetObject.ToString());
                    }

                case object _ when TargetType == typeof(DateTimeOffset):
                case object _ when TargetType == typeof(DateTimeOffset?):
                    {
                        var utcTime = DateTime.SpecifyKind(DateTime.Parse(TargetObject.ToString()), DateTimeKind.Utc);
                        return new DateTimeOffset(utcTime);
                    }

                case object _ when TargetType == typeof(decimal):
                case object _ when TargetType == typeof(decimal?):
                    {
                        return Convert.ToDecimal(TargetObject);
                    }

                case object _ when TargetType == typeof(Guid):
                case object _ when TargetType == typeof(Guid?):
                    {
                        if (TargetObject.GetType() == typeof(string))
                            return new Guid(Convert.ToString(TargetObject));
                        else if (TargetObject.GetType() == typeof(Guid))
                            return (Guid)TargetObject;
                        break;
                    }
                case object when TargetType == typeof(string):
                    {
                        return Convert.ToString(TargetObject);
                    }

                default:
                    {
                        throw new Exception("Unsupported type conversion.");
                        break;
                    }
            }
            return null;
        }
        public static object? ConvertObjectToType(this JsonElement TargetObject, Type TargetType)
        {
            switch (true)
            {

                case object _ when TargetType == typeof(bool):
                case object _ when TargetType == typeof(bool?):
                    {
                        return TargetObject.GetBoolean();
                    }

                case object _ when TargetType == typeof(long):
                case object _ when TargetType == typeof(long?):
                    {
                        return TargetObject.GetInt64();
                    }

                case object _ when TargetType == typeof(int):
                case object _ when TargetType == typeof(int?):
                    {
                        return TargetObject.GetInt32();
                    }

                case object _ when TargetType == typeof(double):
                case object _ when TargetType == typeof(double?):
                    {
                        return TargetObject.GetDouble();
                    }

                case object _ when TargetType == typeof(short):
                case object _ when TargetType == typeof(short?):
                    {
                        return TargetObject.GetInt16();

                    }

                case object _ when TargetType == typeof(byte):
                case object _ when TargetType == typeof(byte?):
                    {
                        return TargetObject.GetByte();
                    }

                case object _ when TargetType == typeof(DateTime):
                case object _ when TargetType == typeof(DateTime?):
                    {
                        //here we go
                        return TargetObject.GetDateTime();
                    }

                case object _ when TargetType == typeof(DateTimeOffset):
                case object _ when TargetType == typeof(DateTimeOffset?):
                    {
                        return TargetObject.GetDateTimeOffset();
                    }

                case object _ when TargetType == typeof(decimal):
                case object _ when TargetType == typeof(decimal?):
                    {
                        return TargetObject.GetDecimal();
                    }

                case object _ when TargetType == typeof(Guid):
                case object _ when TargetType == typeof(Guid?):
                    {
                        return TargetObject.GetGuid();
                    }
                case object when TargetType == typeof(string):
                    {
                        return TargetObject.GetString();
                    }

                default:
                    {
                        throw new Exception("Unsupported type conversion.");
                        break;
                    }
            }
        }
        public static ICollection? ConvertToICollection(this object? input, Type TargetType)
        {
            if (input == null)
            {
                return null;
            }

            if (input is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
            {
                var list = new List<object>();
                foreach (JsonElement element in jsonElement.EnumerateArray())
                {
                    list.Add(element.ConvertObjectToType(TargetType));
                }
                return list;
            }

            if (input.GetType().IsArray)
            {
                var m = new ArrayList((Array)input);
                return m;
            }
           
            else 
            {
                return input as ICollection;
            }
        }
    }
}
