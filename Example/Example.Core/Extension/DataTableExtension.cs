using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Example.Core.Extension
{
    public static class DataTableExtension
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            if (table == null)
                return null;
            var properties = typeof(T).GetProperties();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in properties)
                {
                    if (item.CanWrite && table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                    && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                        }
                    }
                }
                list.Add(entity);
            }
            return list;
        }

        public static T ToEntity<T>(this DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            if (table == null || table.Rows.Count == 0)
                return default(T);
            var properties = typeof(T).GetProperties();

            DataRow row = table.Rows[0];
            T entity = new T();
            foreach (var item in properties)
            {
                if (item.CanWrite && table.Columns.Contains(item.Name))
                {
                    if (DBNull.Value != row[item.Name])
                    {
                        Type newType = item.PropertyType;
                        //判断type类型是否为泛型，因为nullable是泛型类,
                        if (newType.IsGenericType
                                && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                        {
                            //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                            //将type转换为nullable对的基础基元类型
                            newType = nullableConverter.UnderlyingType;
                        }
                        item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                    }
                }
            }

            return entity;
        }
    }
}
