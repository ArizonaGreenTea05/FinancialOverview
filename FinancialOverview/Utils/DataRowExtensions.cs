using System;
using System.Data;

namespace BusinessLogic.Utils
{
    internal static class DataRowExtensions
    {
        public static void CopyValuesTo(this DataRowCollection drc, DataRow[] rows)
        {
            rows = new DataRow[drc.Count];
            drc.CopyTo(rows, 0);
            for (var i = 0; i < drc.Count; ++i)
            for (var j = 0; j < drc[i].ItemArray.Length; ++j)
                    rows[i].ItemArray[j] = Convert.ToString(drc[i].ItemArray[j]);
        }
    }
}
