using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Server=LAPTOP-H0J6VR6K\\SQLEXPRESS;Database=ktdh;TrustServerCertificate=True;Integrated Security=True;Trusted_Connection=True;";

        // Simulate multiple users uploading multiple Excel files
        List<string> excelFiles = new List<string>
        {
            @"C:\Users\welli\Downloads\OCE-20240715\Datapacks\MasterData (3) (1).xlsx",
            @"C:\Users\welli\Downloads\OCE-20240715\Datapacks\MasterData (3) (2).xlsx",
            @"C:\Users\welli\Downloads\OCE-20240715\Datapacks\MasterData (3) (3).xlsx",
            // Add more file paths here
        };

        // Run concurrent uploads
        Task.Run(() => SimulateConcurrentUploads(excelFiles, connectionString)).Wait();
    }

    static async Task SimulateConcurrentUploads(List<string> excelFiles, string connectionString)
    {
        List<Task> tasks = new List<Task>();

        foreach (string file in excelFiles)
        {
            tasks.Add(Task.Run(() => ProcessExcelAndUpload(file, connectionString)));
        }

        await Task.WhenAll(tasks);
    }

    static void ProcessExcelAndUpload(string filePath, string connectionString)
    {
        try
        {
            Console.WriteLine($"Processing file: {filePath}");

            // Parse Excel file into a DataTable
            DataTable dataTable = ParseExcelToDataTable(filePath);

            // Bulk insert into SQL
            BulkInsertToSql(connectionString, dataTable);

            Console.WriteLine($"File processed successfully: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
        }
    }

    static DataTable ParseExcelToDataTable(string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        DataTable dataTable = new DataTable();

        // Define columns in the DataTable
        dataTable.Columns.Add("TypeOfLaw", typeof(string));
        dataTable.Columns.Add("Name", typeof(string));
        dataTable.Columns.Add("Status", typeof(string));
        dataTable.Columns.Add("CreatedBy", typeof(string));
        dataTable.Columns.Add("ModifiedDate", typeof(DateTime));
        dataTable.Columns.Add("CreatedDate", typeof(DateTime));

        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[1]; // Assuming the first sheet

        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["TypeOfLaw"] = worksheet.Cells[row, 1].Text;
            dataRow["Name"] = worksheet.Cells[row, 2].Text;
            dataRow["Status"] = worksheet.Cells[row, 3].Text;
            dataRow["CreatedBy"] = worksheet.Cells[row, 4].Text;
            dataRow["ModifiedDate"] = DateTime.TryParse(worksheet.Cells[row, 5].Text, out var modifiedDate) ? modifiedDate : DBNull.Value;
            dataRow["CreatedDate"] = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var createdDate) ? createdDate : DBNull.Value;

            dataTable.Rows.Add(dataRow);
        }

        return dataTable;
    }

    static void BulkInsertToSql(string connectionString, DataTable dataTable)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        using SqlBulkCopy bulkCopy = new SqlBulkCopy(connection)
        {
            DestinationTableName = "NatureOfProceedings"
        };

        // Map DataTable columns to SQL table columns (excluding Id as it is auto-increment)
        bulkCopy.ColumnMappings.Add("TypeOfLaw", "TypeOfLaw");
        bulkCopy.ColumnMappings.Add("Name", "Name");
        bulkCopy.ColumnMappings.Add("Status", "Status");
        bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
        bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
        bulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");

        // Perform the bulk copy
        bulkCopy.WriteToServer(dataTable);
    }
}
