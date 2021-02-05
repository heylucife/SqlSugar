﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace SqlSugar
{
    public static class DbExtensions
    {
        public static string ToJoinSqlInVals<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                return ToSqlValue(string.Empty);
            }
            else
            {
                return string.Join(",", array.Where(c => c != null).Select(it => it.ToSqlValue()));
            }
        }

        /// <summary>
        /// 数值类型
        /// </summary>
        static Type[] NumericalTypes = new Type[]
        {
            typeof(int),
            typeof(uint),
            typeof(byte),
            typeof(sbyte),
            typeof(long),
            typeof(ulong),
            typeof(short),
            typeof(ushort),
        };

        public static string ToSqlValue(this object value)
        {
            if (NumericalTypes.Contains(value.GetType()))
                return value.ToString();

            var str = value + "";
            return str.ToSqlValue();
        }

        public static string ToSqlValue(this string value)
        {
            return string.Format("'{0}'", value.ToSqlFilter());
        }

        /// <summary>
        ///Sql Filter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSqlFilter(this string value)
        {
            if (!value.IsNullOrEmpty())
            {
                value = value.Replace("'", "''");
            }
            return value;
        }

        internal static string ToLower(this string value ,bool isAutoToLower)
        {
            if (value == null) return null;
            if (isAutoToLower == false) return value;
            return value.ToLower();
        }
    }
}
