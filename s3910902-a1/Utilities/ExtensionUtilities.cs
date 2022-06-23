using System.Data;
using Microsoft.Data.SqlClient;

namespace s3910902_a1.Utilities;

public static class ExtensionUtilities
{
    public static DataTable GetDataTable(this SqlCommand command)
    {
        using var adapter = new SqlDataAdapter(command);

        var table = new DataTable();
        adapter.Fill(table);

        return table;
    }
    
    public static object GetObjectOrDbNull(this object value) => value ?? DBNull.Value;
    
    // Read user input as character
    public static ConsoleKeyInfo ReadInput() => Console.ReadKey(true);
}