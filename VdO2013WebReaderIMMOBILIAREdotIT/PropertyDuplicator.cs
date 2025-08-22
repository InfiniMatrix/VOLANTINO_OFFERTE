using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VdO2013Data;
using VdO2013DataCore;

namespace VdO2013WebReaderIMMOBILIAREdotIT
{
    /// <summary>
    /// Utility class for duplicating property records in the OfferteData table
    /// </summary>
    public static class PropertyDuplicator
    {
        /// <summary>
        /// Duplicates a property record by creating a new row with the same data
        /// </summary>
        /// <param name="sourceRow">The DataRow to duplicate</param>
        /// <param name="targetTable">The target DataTable</param>
        /// <param name="generateNewCode">Whether to generate a new unique code</param>
        /// <returns>The newly created DataRow</returns>
        public static DataRow DuplicateProperty(DataRow sourceRow, DataTable targetTable, bool generateNewCode = true)
        {
            if (sourceRow == null)
                throw new ArgumentNullException(nameof(sourceRow));
            
            if (targetTable == null)
                throw new ArgumentNullException(nameof(targetTable));

            // Create a new row in the target table
            DataRow newRow = targetTable.NewRow();

            // Copy all column values from source to new row
            foreach (DataColumn column in targetTable.Columns)
            {
                // Skip auto-increment and read-only columns
                if (column.AutoIncrement || column.ReadOnly)
                    continue;

                // Skip ID column if it exists
                if (column.ColumnName.Equals("ID", StringComparison.OrdinalIgnoreCase))
                    continue;

                // Handle special columns
                if (column.ColumnName.Equals("Codice", StringComparison.OrdinalIgnoreCase) && generateNewCode)
                {
                    // Generate new unique code based on existing code
                    newRow[column] = GenerateNewCode(sourceRow[column]?.ToString(), targetTable);
                }
                else if (column.ColumnName.Equals("InsertDate", StringComparison.OrdinalIgnoreCase))
                {
                    // Set current date for insert date
                    newRow[column] = DateTime.Now;
                }
                else if (column.ColumnName.Equals("UpdateDate", StringComparison.OrdinalIgnoreCase))
                {
                    // Set current date for update date
                    newRow[column] = DateTime.Now;
                }
                else if (column.ColumnName.Equals("Riferimento", StringComparison.OrdinalIgnoreCase))
                {
                    // Add "COPIA" prefix to reference
                    string originalRef = sourceRow[column]?.ToString() ?? "";
                    newRow[column] = "COPIA-" + originalRef;
                }
                else if (column.ColumnName.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    // Reset status to default (usually 0 or "Normal")
                    newRow[column] = 0;
                }
                else if (column.ColumnName.Equals("Stampa", StringComparison.OrdinalIgnoreCase) ||
                         column.ColumnName.Equals("Cartello", StringComparison.OrdinalIgnoreCase))
                {
                    // Reset print and sign flags
                    newRow[column] = false;
                }
                else if (sourceRow.Table.Columns.Contains(column.ColumnName))
                {
                    // Copy the value from source row
                    newRow[column] = sourceRow[column];
                }
            }

            // Add the new row to the table
            targetTable.Rows.Add(newRow);

            return newRow;
        }

        /// <summary>
        /// Duplicates multiple property records
        /// </summary>
        /// <param name="sourceRows">Collection of DataRows to duplicate</param>
        /// <param name="targetTable">The target DataTable</param>
        /// <returns>List of newly created DataRows</returns>
        public static List<DataRow> DuplicateProperties(DataRow[] sourceRows, DataTable targetTable)
        {
            if (sourceRows == null || sourceRows.Length == 0)
                throw new ArgumentException("No rows to duplicate", nameof(sourceRows));

            var newRows = new List<DataRow>();

            foreach (var sourceRow in sourceRows)
            {
                try
                {
                    var newRow = DuplicateProperty(sourceRow, targetTable);
                    newRows.Add(newRow);
                }
                catch (Exception ex)
                {
                    // Log error but continue with other rows
                    System.Diagnostics.Debug.WriteLine($"Error duplicating row: {ex.Message}");
                }
            }

            return newRows;
        }

        /// <summary>
        /// Generates a new unique code based on the original code
        /// </summary>
        /// <param name="originalCode">The original property code</param>
        /// <param name="table">The DataTable to check for existing codes</param>
        /// <returns>A new unique code</returns>
        private static string GenerateNewCode(string originalCode, DataTable table)
        {
            if (string.IsNullOrEmpty(originalCode))
                originalCode = "PROP";

            string baseCode = originalCode;
            int suffix = 1;

            // Extract any existing numeric suffix
            var match = System.Text.RegularExpressions.Regex.Match(originalCode, @"(.+?)(\d+)$");
            if (match.Success)
            {
                baseCode = match.Groups[1].Value;
                suffix = int.Parse(match.Groups[2].Value) + 1;
            }
            else
            {
                baseCode = originalCode + "_COPY";
            }

            // Generate unique code
            string newCode = baseCode + suffix;
            while (CodeExists(newCode, table))
            {
                suffix++;
                newCode = baseCode + suffix;
            }

            return newCode;
        }

        /// <summary>
        /// Checks if a code already exists in the table
        /// </summary>
        /// <param name="code">The code to check</param>
        /// <param name="table">The DataTable to search</param>
        /// <returns>True if the code exists, false otherwise</returns>
        private static bool CodeExists(string code, DataTable table)
        {
            if (table.Columns.Contains("Codice"))
            {
                return table.AsEnumerable().Any(row => 
                    string.Equals(row.Field<string>("Codice"), code, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }

        /// <summary>
        /// Creates a clone of property with specific modifications
        /// </summary>
        /// <param name="sourceRow">The source property row</param>
        /// <param name="targetTable">The target table</param>
        /// <param name="modifications">Dictionary of column names and new values</param>
        /// <returns>The newly created DataRow</returns>
        public static DataRow ClonePropertyWithModifications(DataRow sourceRow, DataTable targetTable, 
            Dictionary<string, object> modifications = null)
        {
            var newRow = DuplicateProperty(sourceRow, targetTable);

            if (modifications != null)
            {
                foreach (var mod in modifications)
                {
                    if (targetTable.Columns.Contains(mod.Key) && 
                        !targetTable.Columns[mod.Key].ReadOnly)
                    {
                        newRow[mod.Key] = mod.Value;
                    }
                }
            }

            return newRow;
        }
    }
}