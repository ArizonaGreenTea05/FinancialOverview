using System;
using System.Data;
using System.Linq;

namespace FinancialOverview.Utils
{
    internal static class DataRowExtensions
    {
        public static void CopyTo(this DataRowCollection drc, DataRowCollection rows)
        {
            for (var i = 0; i < drc.Count; ++i)
            {
                var newRow = new object[drc[i].ItemArray.Length];
                for (var j = 0; j < drc[i].ItemArray.Length; ++j)
                    newRow[j] = Convert.ToString(drc[i].ItemArray[j]);
                rows.Add(newRow);
            }
        }
    }
}
