using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Example.Core.Extension
{
    public static class ListExtension
    {
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable table = new DataTable();
            if (list.Count > 0)
            {
                var properties = list[0].GetType().GetProperties();
                foreach (var pi in properties)
                {
                    Type pt = pi.PropertyType;
                    if ((pt.IsGenericType) && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        pt = pt.GetGenericArguments()[0];
                    }
                    table.Columns.Add(new DataColumn(pi.Name, pt));
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var tempList = new System.Collections.ArrayList();
                    foreach (var pi in properties)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }
            return table;
        }
    }
}
