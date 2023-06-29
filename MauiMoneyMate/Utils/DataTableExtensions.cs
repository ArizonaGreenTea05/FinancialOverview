using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMoneyMate.Utils
{
    public static class DataTableExtensions
    {
        public static bool ContainsSimilarRow(this DataTable table, DataRow row)
        {
            return table.Rows.Cast<DataRow>().Any(dr => dr.IsSimilarTo(row));
        }

        public static bool ContainsSimilarRow(this DataTable table, object[] items)
        {
            return table.Rows.Cast<DataRow>().Any(dr => dr.IsSimilarTo(items));
        }

        public static bool IsSimilarTo(this DataRow row, object[] items)
        {
            if (row == null &&  items == null) return false;
            if (row == null ||  items == null) return false;
            if (row.Table.Columns.Count != items.Length) return false;
            for (var i = 0; i < row.Table.Columns.Count; ++i)
                if (Convert.ToString(row[i]) != Convert.ToString(items[i]))
                    return false;
            return true;
        }

        public static bool IsSimilarTo(this DataRow row1, DataRow row2)
        {
            if (row1 == row2) return true;
            if (row1 == null ||  row2 == null) return false;
            if (row1.Table.Columns.Count != row2.Table.Columns.Count) return false;
            for (var i = 0; i < row1.Table.Columns.Count; ++i)
                if (Convert.ToString(row1[i]) != Convert.ToString(row2[i]))
                    return false;
            return true;
        }
    }
}
