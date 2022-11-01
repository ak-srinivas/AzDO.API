using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AzDO.API.Base.Common.Extensions
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Exports contents of any DataTable to a given file. 
        /// This is the most fastest way to export a data table of any size to a file. <br/><br/>
        /// <i>Tested with data tables having 5 million records and this function took 20 seconds to write to disk.</i>
        /// </summary>
        /// <param name="table">DataTable to fetch records.</param>
        /// <param name="targetFilePath">Destination file path.</param>
        /// <param name="delimiter">Default delimiter is comma(,).</param>
        public static void ConvertTableToFile(this DataTable table, string targetFilePath, char delimiter = ',')
        {
            File.Delete(targetFilePath); // This will not throw an excpetion if file is not found.

            var fileStream = new FileStream(targetFilePath, FileMode.Create);
            using var bufstream = new BufferedStream(fileStream, 4096);
            using var swriter = new StreamWriter(bufstream);
            swriter.WriteLine(string.Join(delimiter, table.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName)));
            foreach (DataRow dataRow in table.Rows)
            {
                string record = string.Join(delimiter, dataRow.ItemArray);
                swriter.WriteLine(record);
                bufstream.Flush();
            }
        }

        /// <summary>
        /// Imports a csv file as a data table. <br/><br/>
        /// <i>Tested with a csv file of 1.5 GB file size with 5 million records.<br/> 
        /// This function took 1 minute to read the entire csv into a data table. </i>
        /// </summary>
        /// <param name="targetCsvInfo">Destination file path.</param>
        /// <returns>A data table with </returns>
        public static DataTable GetDataTableFromCsvFile(FileInfo targetCsvInfo)
        {
            DataTable csvTable = new DataTable();
            bool isHeaderFound = false;

            List<string> headerCols = GetHeaderFromCSVFile(targetCsvInfo);

            for (int i = 0; i < headerCols.Count; i++)
            {
                csvTable.Columns.Add(headerCols[i]);
            }

            FileStream fileStream = new FileStream(targetCsvInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
            using (BufferedStream bufstream = new BufferedStream(fileStream))
            using (StreamReader sreader = new StreamReader(bufstream))
            {
                string line = sreader.ReadLine();

                if (!isHeaderFound)
                {
                    // Skip first iteration, because first record is header which we do not need.
                    isHeaderFound = true;
                    line = sreader.ReadLine();
                }

                while (line != null)
                {
                    List<string> cellValues = line.Split(',').Select(p => p.Trim()).ToList();
                    DataRow newRow = csvTable.NewRow();
                    for (int i = 0; i < cellValues.Count; i++)
                    {
                        newRow[i] = cellValues[i];
                    }

                    csvTable.Rows.Add(newRow);
                    line = sreader.ReadLine();
                }
            }

            return csvTable;
        }

        public static string GetColumnNumbers(FileInfo csvInfo, List<string> columnNames, char delimiter = ',', bool throwError = false)
        {
            if (columnNames == null || columnNames.Count == 0)
                return null;

            List<string> allColumns = GetHeaderFromCSVFile(csvInfo);
            string colNumbers = null;

            foreach (string column in columnNames)
            {
                int index = allColumns.IndexOf(column);

                if (index == -1)
                {
                    if (throwError)
                    {
                        string message = $"Column name '{column}' does not exist in csv file.";
                        throw new Exception(message);
                    }
                }
                else
                    colNumbers = colNumbers + (index + 1) + delimiter.ToString();
            }

            if (colNumbers != null && colNumbers.EndsWith(delimiter.ToString()))
                colNumbers = colNumbers.Remove(colNumbers.Length - 1);

            return colNumbers;
        }

        public static List<string> GetHeaderFromCSVFile(FileInfo csvInfo, bool sort = false)
        {
            string header = null;
            FileStream fileStream = new FileStream(csvInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
            using (BufferedStream bufstream = new BufferedStream(fileStream))
            using (StreamReader sreader = new StreamReader(bufstream))
            {
                header = sreader.ReadLine();
                if (header == null)
                    throw new NullReferenceException("Couldn't fetch header from csv file.");
            }
            List<string> list = header.Split(',').Select(p => p.Trim()).ToList();
            if (sort)
                list.Sort();

            return list;
        }

        /// <summary>
        /// Returns a specific column from a DataTable as a List<T> where T is a type.
        /// </summary>
        /// <typeparam name="Type">Return type</typeparam>
        /// <param name="table">A DataTable</param>
        /// <param name="columnName">A valid column name</param>
        public static List<Type> GetColumnFromDataTable<Type>(this DataTable table, string columnName)
        {
            return table.Rows.OfType<DataRow>().Select(dr => dr.Field<Type>(columnName)).ToList();
        }
    }
}
