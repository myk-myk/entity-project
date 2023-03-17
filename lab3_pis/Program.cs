using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3_pis
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string connectionString = "Data Source=DESKTOP-4NCMVR1; Initial Catalog = lab1_pis; Integrated Security = true";
            var connection = new SqlConnection();
            connection.ConnectionString = connectionString;*/

            var cnString = ConfigurationManager.ConnectionStrings["lab3_pis"];
            var connection = new SqlConnection(cnString.ConnectionString);

            var name = cnString.Name;
            var connectionString = cnString.ConnectionString;

            Console.WriteLine("Connection name: " + name);
            Console.WriteLine("From App.Config: " + connectionString);

            Console.WriteLine();

            var ProgrammerRead = connection.CreateCommand();
            ProgrammerRead.CommandType = CommandType.Text;
            ProgrammerRead.CommandText = "SELECT * FROM Programmer";

            var TechnologyRead = connection.CreateCommand();
            TechnologyRead.CommandType = CommandType.Text;
            TechnologyRead.CommandText = "SELECT * FROM Technology";

            var SkillRead = connection.CreateCommand();
            SkillRead.CommandType = CommandType.Text;
            SkillRead.CommandText = "SELECT * FROM Skill";

            ReadTable(connection, ProgrammerRead);
            ReadTable(connection, TechnologyRead);
            ReadTable(connection, SkillRead);

            var TechnologyAdd = connection.CreateCommand();
            TechnologyAdd.CommandType = CommandType.Text;
            TechnologyAdd.CommandText = "INSERT INTO Technology(Name, Field) VALUES ('Node.js', 'JS Framework');";

            var SkillAdd = connection.CreateCommand();
            SkillAdd.CommandType = CommandType.Text;
            SkillAdd.CommandText = "INSERT INTO Skill(Name, Type) VALUES('Time management', 'Soft skill');";

            var ProgrammerAdd = connection.CreateCommand();
            ProgrammerAdd.CommandType = CommandType.Text;
            ProgrammerAdd.CommandText = "INSERT INTO Programmer(Name, Surname, tech_id, skill_id) VALUES('Olha', 'Kychuk', 6, 6);";

            ExecuteDatabase(connection, TechnologyAdd);
            ExecuteDatabase(connection, SkillAdd);
            ExecuteDatabase(connection, ProgrammerAdd);

            Console.WriteLine("Database after adding a row:");
            ReadTable(connection, ProgrammerRead);
            ReadTable(connection, TechnologyRead);
            ReadTable(connection, SkillRead);

            var TechnologyEdit = connection.CreateCommand();
            TechnologyEdit.CommandType = CommandType.Text;
            TechnologyEdit.CommandText = "UPDATE Technology SET Name = 'Angular', Field = 'TypeScript Framework' WHERE Id = 6;";
            
            var SkillEdit = connection.CreateCommand();
            SkillEdit.CommandType = CommandType.Text;
            SkillEdit.CommandText = "UPDATE Skill SET Name = 'Front End', Type = 'Hard skill' WHERE Id = 6;";
            
            var ProgrammerEdit = connection.CreateCommand();
            ProgrammerEdit.CommandType = CommandType.Text;
            ProgrammerEdit.CommandText = "UPDATE Programmer SET Name = 'Yuliia', Surname = 'Tomkiv' WHERE Id = 6;";
            
            ExecuteDatabase(connection, TechnologyEdit);
            ExecuteDatabase(connection, SkillEdit);
            ExecuteDatabase(connection, ProgrammerEdit);
            
            Console.WriteLine("Database after editing a row:");
            ReadTable(connection, ProgrammerRead);
            ReadTable(connection, TechnologyRead);
            ReadTable(connection, SkillRead);
            
            var TechnologyDelete = connection.CreateCommand();
            TechnologyDelete.CommandType = CommandType.Text;
            TechnologyDelete.CommandText = "DELETE FROM Technology WHERE Id = 6;";
            
            var SkillDelete = connection.CreateCommand();
            SkillDelete.CommandType = CommandType.Text;
            SkillDelete.CommandText = "DELETE FROM Skill WHERE Id = 6;";
            
            var ProgrammerDelete = connection.CreateCommand();
            ProgrammerDelete.CommandType = CommandType.Text;
            ProgrammerDelete.CommandText = "DELETE FROM Programmer WHERE Id = 6;";
            
            ExecuteDatabase(connection, TechnologyDelete);
            ExecuteDatabase(connection, SkillDelete);
            ExecuteDatabase(connection, ProgrammerDelete);
            
            Console.WriteLine("Database after deleting a row:");
            ReadTable(connection, ProgrammerRead);
            ReadTable(connection, TechnologyRead);
            ReadTable(connection, SkillRead);
            
            var ProgrammerAdapter = new SqlDataAdapter(ProgrammerRead);
            var TechnologyAdapter = new SqlDataAdapter(TechnologyRead);
            var SkillAdapter = new SqlDataAdapter(SkillRead);
            
            DataSet Employers = new DataSet("Employers");
            
            TechnologyAdapter.Fill(Employers, "Technology");
            SkillAdapter.Fill(Employers, "Skill");
            ProgrammerAdapter.Fill(Employers, "Programmer");
            
            PrintDataSet(Employers);
            
            var TechnologyCommandBuilder = new SqlCommandBuilder(TechnologyAdapter);
            var SkillCommandBuilder = new SqlCommandBuilder(SkillAdapter);
            var ProgrammerCommandBuilder = new SqlCommandBuilder(ProgrammerAdapter);
            
            var TechnologyTable = Employers.Tables["Technology"];
            var TechnologyRow = TechnologyTable.Select("Id = 2")[0];
            TechnologyRow["Name"] = "C#";
            
            var SkillTable = Employers.Tables["Skill"];
            SkillTable.Rows.Add(7, "Time management", "Soft skill");
            
            var ProgrammerTable = Employers.Tables["Programmer"];
            var ProgrammerRow = ProgrammerTable.Select("Id = 3")[0];
            ProgrammerRow.Delete();
            
            TechnologyAdapter.Update(Employers, "Technology");
            SkillAdapter.Update(Employers, "Skill");
            ProgrammerAdapter.Update(Employers, "Programmer");
            
            Console.WriteLine("Dataset after editing:");
            PrintDataSet(Employers);
            Console.WriteLine("Database after editing a dataset:");
            ReadTable(connection, ProgrammerRead);
            ReadTable(connection, TechnologyRead);
            ReadTable(connection, SkillRead);
            Console.ReadLine();
        }
        
        static void ReadTable(SqlConnection connection, SqlCommand command)
        {
            connection.Open();
            DbDataReader dataReader = command.ExecuteReader();
            int columncount = dataReader.FieldCount;
            int i = 0;
            while (i < columncount)
            {
                string columnname = dataReader.GetName(i);
                Console.Write(string.Format($"{columnname,-30}"));
                i++;
            }
            Console.WriteLine();
            while (dataReader.Read())
            {
                i = 0;
                while (i < columncount)
                {
                    string columnname = dataReader.GetName(i);
                    Console.Write(string.Format($"{dataReader[columnname],-30}"));
                    i++;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            connection.Close();
        }
        
        static void ExecuteDatabase(SqlConnection connection, SqlCommand command)
        {
            connection.Open();
            var count = command.ExecuteNonQuery();
            Console.WriteLine($"{count} rows affected.");
            Console.WriteLine();
            connection.Close();
        }
        
        public static void PrintDataSet(DataSet database)
        {
            string datasetName = string.Format($"DataSet {database.DataSetName}:");
            Console.WriteLine(string.Format($"{datasetName,70}"));
            Console.WriteLine();
            foreach (DataTable table in database.Tables)
            {
                string datatableName = string.Format($"Table {table.TableName}:");
                Console.WriteLine(string.Format($"{datatableName,45}"));
                foreach (DataColumn column in table.Columns)
                {
                    Console.Write(string.Format($"{column.ColumnName,-30}"));
                }
                Console.WriteLine();
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (DataColumn column in table.Columns)
                        {
                            Console.Write(string.Format($"{row[column],-30}"));
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }
    }
}

