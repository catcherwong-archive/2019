namespace ExportExcelDemo
{
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    public static class ExcelExportHelper
    {
        public static string ExcelContentType
        {
            get
            {
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
        }

        private static DataTable ListToDataTable<T>(List<T> data, string[] columnsToTake = null)
        {            
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            DataTable dataTable = new DataTable();
            List<int> list = new List<int>();

            for (int i = 0; i < properties.Length; i++)
            {
                var item = properties[i];

                if (columnsToTake != null)
                {
                    if (columnsToTake.Any(x => x.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        var name = item.GetCustomAttribute<DisplayNameAttribute>();
                        dataTable.Columns.Add(name != null ? name.DisplayName : item.Name, Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType);
                        list.Add(i);
                    }
                }
                else
                {
                    dataTable.Columns.Add(item.Name, Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType);
                    list.Add(i);
                }
            }

            foreach (T item in data)
            {
                var objs = new List<object>();

                foreach (var i in list)
                {
                    objs.Add(properties[i].GetValue(item));
                }

                dataTable.Rows.Add(objs.ToArray());
            }

            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, string sheetName = "Sheet1", bool freeze = false, bool autoFit = false)
        {
            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(sheetName);

                // Add Content Into the Excel File
                workSheet.Cells["A1"].LoadFromDataTable(dataTable, true);                
                               
                // first row 
                workSheet.Cells[1, 1, 1, workSheet.Cells.Columns].Style.Font.Bold = true;

                if (autoFit)
                {
                    workSheet.Cells.AutoFitColumns();
                }

                if (freeze)
                {
                    // freeze first row
                    workSheet.View.FreezePanes(2, 1);
                }                

                result = package.GetAsByteArray();
            }
            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, string sheetName = "Sheet1", string[] columnsToTake = null, bool freeze = false, bool autoFit = false)
        {
            return ExportExcel(ListToDataTable(data, columnsToTake), sheetName, freeze, autoFit);
        }
    }
}
