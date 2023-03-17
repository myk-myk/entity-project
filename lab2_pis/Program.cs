using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace lab2_pis
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable Programmer = new DataTable("Programmer");
            DataTable Technology = new DataTable("Technology");
            DataTable Skill = new DataTable("Skill");

            DataColumn ProgrammerID = new DataColumn("Id")
            {
                DataType = typeof(int),
                Unique = true,
                AllowDBNull = false,
                AutoIncrement = true,
                AutoIncrementSeed = -1,
                AutoIncrementStep = -1
            };
            DataColumn ProgrammerName = new DataColumn("Name")
            {
                MaxLength = 20,
                AllowDBNull = false
            };
            DataColumn ProgrammerSurname = new DataColumn("Surname")
            {
                MaxLength = 30
            };
            DataColumn ProgrammerTechID = new DataColumn("tech_id")
            {
                DataType = typeof(int),
                Unique = true,
                AllowDBNull = false
            };
            DataColumn ProgrammerSkillID = new DataColumn("skill_id")
            {
                DataType = typeof(int),
                Unique = true,
                AllowDBNull = false
            };

            Programmer.Columns.Add(ProgrammerID);
            Programmer.Columns.Add(ProgrammerName);
            Programmer.Columns.Add(ProgrammerSurname);
            Programmer.Columns.Add(ProgrammerTechID);
            Programmer.Columns.Add(ProgrammerSkillID);

            Programmer.PrimaryKey = new DataColumn[] { ProgrammerID };

            DataColumn TechnologyID = new DataColumn("Id")
            {
                DataType = typeof(int),
                Unique = true,
                AllowDBNull = false,
                AutoIncrement = true,
                AutoIncrementSeed = -1,
                AutoIncrementStep = -1
            };
            DataColumn TechnologyName = new DataColumn("Name")
            {
                MaxLength = 20,
                AllowDBNull = false
            };
            DataColumn TechnologyField = new DataColumn("Field")
            {
                MaxLength = 50
            };

            Technology.Columns.Add(TechnologyID);
            Technology.Columns.Add(TechnologyName);
            Technology.Columns.Add(TechnologyField);

            Technology.PrimaryKey = new DataColumn[] { TechnologyID };

            DataColumn SkillID = new DataColumn("Id")
            {
                DataType = typeof(int),
                Unique = true,
                AllowDBNull = false,
                AutoIncrement = true,
                AutoIncrementSeed = -1,
                AutoIncrementStep = -1
            };
            DataColumn SkillName = new DataColumn("Name")
            {
                MaxLength = 20,
                AllowDBNull = false
            };
            DataColumn SkillType = new DataColumn("Type")
            {
                MaxLength = 10
            };

            Skill.Columns.Add(SkillID);
            Skill.Columns.Add(SkillName);
            Skill.Columns.Add(SkillType);

            Skill.PrimaryKey = new DataColumn[] { SkillID };

            DataSet Employers = new DataSet("Employers");

            Employers.Tables.Add(Programmer);
            Employers.Tables.Add(Technology);
            Employers.Tables.Add(Skill);

            Employers.Relations.Add("Programmer_technology", Technology.Columns["Id"], Programmer.Columns["tech_id"]);
            Employers.Relations.Add("Programmer_skill", Skill.Columns["Id"], Programmer.Columns["skill_id"]);

            ForeignKeyConstraint tech_fk = (ForeignKeyConstraint)Programmer.Constraints["Programmer_technology"];
            tech_fk.DeleteRule = Rule.None;
            ForeignKeyConstraint skill_fk = (ForeignKeyConstraint)Programmer.Constraints["Programmer_skill"];
            skill_fk.DeleteRule = Rule.None;

            DataRow first_technology = Technology.NewRow();
            first_technology["Id"] = -1;
            first_technology["Name"] = ".NET";
            first_technology["Field"] = "Framework";
            Technology.Rows.Add(first_technology);
            Technology.Rows.Add(null, "Python", "Programming language");
            Technology.Rows.Add(null, "Scrum", "Product creation methodology");
            Technology.Rows.Add(null, "SQL", "Data base language");
            Technology.Rows.Add(null, "Git", "Version control system");

            DataRow first_skill = Skill.NewRow();
            first_skill["Id"] = -1;
            first_skill["Name"] = "Machine learning";
            first_skill["Type"] = "Hard skill";
            Skill.Rows.Add(first_skill);
            Skill.Rows.Add(null, "Public speaking", "Soft skill");
            Skill.Rows.Add(null, "Quick thinking", "Soft skill");
            Skill.Rows.Add(null, "CAD skills", "Hard skill");
            Skill.Rows.Add(null, "CRM platforms", "Hard skill");

            DataRow first_programmer = Programmer.NewRow();
            first_programmer["Id"] = -1;
            first_programmer["Name"] = "Mykola";
            first_programmer["Surname"] = "Sannikov";
            first_programmer["tech_id"] = -2;
            first_programmer["skill_id"] = -1;
            Programmer.Rows.Add(first_programmer);
            Programmer.Rows.Add(null, "Petro", "Solomchak", -1, -3);
            Programmer.Rows.Add(null, "Bohdan", "Nadeberny", -4, -2);
            Programmer.Rows.Add(null, "Mariya", "Fedak", -5, -4);
            Programmer.Rows.Add(null, "Yuliia", "Safonova", -3, -5);

            Employers.AcceptChanges();
            PrintDataSet(Employers);

            DataView sortview = new DataView(Technology);
            sortview.Sort = "Name ASC, Field DESC";
            Console.WriteLine("Sorted view:");
            PrintView(sortview);

            DataView filterview = new DataView(Skill);
            filterview.RowFilter = "Type like '%Soft%'";
            Console.WriteLine("Filtered view:");
            PrintView(filterview);

            Technology.Rows.Add(null, "Node.js", "JS Framework");
            Skill.Rows.Add(null, "Time management", "Soft skill");
            Programmer.Rows.Add(null, "Olha", "Kychuk", -6, -6);
            Technology.Rows[0]["Field"] = "C# Framework";
            Skill.Rows[2]["Name"] = "Teamwork";
            Programmer.Rows[1]["Surname"] = "Borsuk";
            Programmer.Rows[4].Delete();
            Technology.Rows[2].Delete();
            Skill.Rows[4].Delete();
            Console.WriteLine("Modified DataSet:");
            PrintDataSet(Employers);
            
            Console.WriteLine("Only changed rows data:");
            PrintChangedData(Employers);

            DataSet changeddata = new DataSet("Changed Data");
            foreach (DataTable table in Employers.Tables)
            {
                changeddata.Tables.Add(table.GetChanges());
            }
            PrintDataSet(changeddata);

            Console.ReadLine();
        }
        public static void PrintDataSet(DataSet database)
        {
            string datasetName = string.Format($"DataSet {database.DataSetName}:");
            Console.WriteLine(string.Format($"{datasetName, 70}"));
            Console.WriteLine();
            foreach (DataTable table in database.Tables)
            {
                string datatableName = string.Format($"Table {table.TableName}:");
                Console.WriteLine(string.Format($"{datatableName, 45}"));
                foreach(DataColumn column in table.Columns)
                {
                    Console.Write(string.Format($"{column.ColumnName, -30}"));
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
                            Console.Write(string.Format($"{row[column], -30}"));
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }
        }

        public static void PrintChangedData(DataSet database)
        {
            string datasetName = string.Format($"DataSet {database.DataSetName}:");
            Console.WriteLine(string.Format($"{datasetName, 70}"));
            Console.WriteLine();
            foreach (DataTable table in database.Tables)
            {
                string datatableName = string.Format($"Table {table.TableName}:");
                Console.WriteLine(string.Format($"{datatableName, 45}"));
                foreach (DataColumn column in table.Columns)
                {
                    Console.Write(string.Format($"{column.ColumnName, -30}"));
                }
                Console.Write(string.Format("State",-20));
                Console.WriteLine();
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState != DataRowState.Unchanged)
                    {
                        if (row.RowState != DataRowState.Deleted)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                Console.Write(string.Format($"{row[column], -30}"));
                            }
                            Console.Write(string.Format($"{row.RowState, -20}"));
                            Console.WriteLine();
                        }
                        else
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                Console.Write(string.Format($"{row[column, DataRowVersion.Original], -30}"));
                            }
                            Console.Write(string.Format($"{row.RowState, -20}"));
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                Console.WriteLine();
            }
        }

        public static void PrintView(DataView view)
        {
            string datatableName = string.Format($"Table {view.Table.TableName}:");
            Console.WriteLine(string.Format($"{datatableName, 45}"));
            foreach (DataColumn column in view.Table.Columns)
            {
                Console.Write(string.Format($"{column.ColumnName, -30}"));
            }
            Console.WriteLine();
            foreach (DataRowView row in view)
            {
                foreach (DataColumn column in view.Table.Columns)
                {
                    Console.Write(string.Format($"{row.Row[column],-30}"));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
