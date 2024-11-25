using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using OfficeOpenXml;

class Program2
{
    static void Main2(string[] args)
    {
        string excelFilePath = @"C:\Users\welli\Downloads\OCE-20240715\Datapacks\MasterData (3) (1).xlsx";
        string connectionString = "Server=LAPTOP-H0J6VR6K\\SQLEXPRESS;Database=ktdh;TrustServerCertificate=True;Integrated Security=True;Trusted_Connection=True;";

        DataTable dataTable = ParseExcelSheet(excelFilePath, "NatureOfProceedings");

        BulkInsertToSql(connectionString, dataTable);
    }

    static DataTable ParseExcelSheet(string filePath, string sheetName)
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new System.IO.FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[sheetName];

        DataTable dataTable = new DataTable();
        // Add columns based on Excel header
        dataTable.Columns.Add("TypeOfLaw", typeof(string));
        dataTable.Columns.Add("Name", typeof(string));
        dataTable.Columns.Add("Status", typeof(string));
        dataTable.Columns.Add("CreatedBy", typeof(string));
        dataTable.Columns.Add("ModifiedDate", typeof(DateTime));
        dataTable.Columns.Add("CreatedDate", typeof(DateTime));

        // Iterate through rows and populate the DataTable
        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["TypeOfLaw"] = worksheet.Cells[row, 1].Text;
            dataRow["Name"] = worksheet.Cells[row, 2].Text;
            dataRow["Status"] = worksheet.Cells[row, 3].Text;
            dataRow["CreatedBy"] = worksheet.Cells[row, 4].Text;
            dataRow["ModifiedDate"] = DateTime.TryParse(worksheet.Cells[row, 5].Text, out var modifiedDate) ? modifiedDate : (object)DBNull.Value;
            dataRow["CreatedDate"] = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var createdDate) ? createdDate : (object)DBNull.Value;
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
