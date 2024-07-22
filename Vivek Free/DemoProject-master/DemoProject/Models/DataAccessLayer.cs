using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace DemoProject.Models
{
    public class DataAccessLayer
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataAccessLayer(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connectionString");
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> lstEmployee = new List<Employee>();
            string query = "SELECT * FROM EMPLOYEE";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee();

                                employee.Id = Convert.ToInt32(reader["EmpId"]);
                                employee.Name = Convert.ToString(reader["EmpName"]);
                                employee.JoiningDate = Convert.ToDateTime(reader["EmpJoiningDate"]);
                                employee.District = Convert.ToString(reader["EmpDistrict"]);
                                employee.CSGhead = Convert.ToString(reader["CSGhead"]);
                                employee.PU = Convert.ToString(reader["PU"]); // Ensure correct case
                                employee.PUMapped = Convert.ToString(reader["PUMapped"]); // Ensure correct case
                                employee.DM = Convert.ToString(reader["DM"]); // Ensure correct case
                                employee.CSG = Convert.ToString(reader["CSG"]); // Ensure correct case
                                employee.State = Convert.ToString(reader["State"]); // Ensure correct case
                                //employee.RevVar = Convert.ToDouble(reader["RevVar"]);
                                //employee.VolVar = Convert.ToDouble(reader["VolVar"]);

                                lstEmployee.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Consider logging the exception
            }

            return lstEmployee;
        }



        public IEnumerable<Employee> GetEmployees(DateTime startDate, DateTime endDate)
        {
            List<Employee> lstEmployee = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_employee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@StartDate", startDate));
                        command.Parameters.Add(new SqlParameter("@EndDate", endDate));

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee();

                                employee.Id = Convert.ToInt32(reader["EmpId"]);
                                employee.Name = Convert.ToString(reader["EmpName"]);
                                employee.JoiningDate = Convert.ToDateTime(reader["EmpJoiningDate"]);
                                employee.District = Convert.ToString(reader["EmpDistrict"]);
                                employee.Language = Convert.ToString(reader["Emplanguage"]);
                                employee.PU = Convert.ToString(reader["PU"]);
                                employee.PUMapped = Convert.ToString(reader["PUMapped"]);
                                employee.DM = Convert.ToString(reader["DM"]);
                                employee.CSG = Convert.ToString(reader["CSG"]);
                                employee.CSGhead = Convert.ToString(reader["CSGhead"]);
                                employee.State = Convert.ToString(reader["State"]);
                                employee.RevVar = Convert.ToDouble(reader["RevVar"]);
                                employee.RevVar = Convert.ToDouble(reader["RevVar1"]) + Convert.ToDouble(reader["RevVar2"]);
                                employee.RevVar1 = Convert.ToDouble(reader["RevVar1"]);
                                employee.RevVar2 = Convert.ToDouble(reader["RevVar2"]);
                                employee.VolVar1 = Convert.ToDouble(reader["VolVar1"]);
                                employee.VolVar2 = Convert.ToDouble(reader["VolVar2"]);
                                employee.VolVar = Convert.ToDouble(reader["VolVar"]);


                                lstEmployee.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Consider logging the exception
            }

            return lstEmployee;
        }
        public IEnumerable<string> GetJoiningDate()
        {
            List<string> JoiningDate = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT DISTINCT EmpJoiningDate FROM Employee";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JoiningDate.Add(Convert.ToString(reader["EmpJoiningDate"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return JoiningDate;
        }

        public IEnumerable<string> GetCSGhead()
        {
            List<string> CSGhead = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT DISTINCT CSGhead FROM Employee";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CSGhead.Add(Convert.ToString(reader["CSGhead"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CSGhead;
        }

        public IEnumerable<string> GetCSG()
        {
            List<string> CSG = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT DISTINCT CSG FROM Employee";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CSG.Add(Convert.ToString(reader["CSG"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CSG;
        }

        public List<string> GetDistrictsByState(string state)
        {
            List<string> districts = new List<string>();
            string query = "SELECT DISTINCT EmpDistrict FROM Employee WHERE State = @State";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@State", state);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string district = Convert.ToString(reader["EmpDistrict"]);
                            districts.Add(district);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return districts;
        }

        public List<string> GetCSGbyCSGhead(string CSGhead)
        {
            List<string> CSGs = new List<string>();
            string query = "SELECT DISTINCT CSG FROM Employee WHERE CSGhead = @CSGhead";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CSGhead", CSGhead);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string CSG = Convert.ToString(reader["CSG"]);
                            CSGs.Add(CSG);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CSGs;
        }

        public List<string> GetPUMappedbyPU(string PU)
        {
            List<string> PUmapped = new List<string>();
            string query = "SELECT DISTINCT PUMapped FROM Employee WHERE PU = @PU";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PU", PU);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string PUMapped = Convert.ToString(reader["PU"]);
                            PUmapped.Add(PUMapped);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PUmapped;
        }

        //public List<string> GetItemsByCriteria(string criteria, string value)
        //{
        //    List<string> items = new List<string>();
        //    string query = "";

        //    switch (criteria)
        //    {
        //        case "State":
        //            query = "SELECT DISTINCT EmpDistrict FROM Employee WHERE State = @Value";
        //            break;
        //        case "CSGhead":
        //            query = "SELECT DISTINCT CSG FROM Employee WHERE CSGhead = @Value";
        //            break;
        //        default:
        //            throw new ArgumentException("Invalid criteria provided.");
        //    }

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@Value", value);
        //            connection.Open();

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string item = Convert.ToString(reader[criteria == "State" ? "EmpDistrict" : "CSG"]);
        //                    items.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return items;
        //}


        //public IEnumerable<Employee> GetGraphDataForChart(List<DateTime> selectedDates)
        //{
        public List<Employee> GetGraphDataForChart(List<DateTime> selectedDates)
        {
            List<Employee> graphData = new List<Employee>();
            try
            {
                string formattedString = "";
                foreach (var date in selectedDates)
                {
                    // Format each date to "MM/dd/yyyy hh:mm:ss tt" format
                    formattedString += date.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture) + ",";
                }
                // Remove the trailing comma
                formattedString = formattedString.TrimEnd(',');
                var Dates = String.Join(',', formattedString);
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("sp_employee_4", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DateA", formattedString);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //if sql side se data milega to while loop ke andar jayega
                        while (reader.Read())
                        {
                            Employee data = new Employee();
                            data.Date = reader.GetDateTime(reader.GetOrdinal("EmpJoiningDate")).ToString("yyyy-MM-dd");
                            if (!reader.IsDBNull(reader.GetOrdinal("RevVar")))
                            {
                                // If not null, try to parse it to double //parse mean try karna check karne ki ki usi formate hai ya nahi
                                if (double.TryParse(reader["RevVar"].ToString(), out double revVar))
                                {
                                    data.RevVar = revVar;
                                }
                                else
                                {
                                    
                                    data.RevVar = 0.0;
                                }
                            }
                            else
                            {
                               
                                data.RevVar = 0.0;
                            }

                            // Repeat the same process for VolVar1
                            if (!reader.IsDBNull(reader.GetOrdinal("VolVar")))
                            {
                                if (double.TryParse(reader["VolVar"].ToString(), out double volVar))
                                {
                                    data.VolVar = volVar;
                                }
                                else
                                {
                                    // Handle parsing failure if required
                                    data.VolVar = 0.0;
                                }
                            }
                            else
                            {
                                // Handle DBNull if required
                                data.VolVar = 0.0;
                            }

                            graphData.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return graphData;
        }


        public IEnumerable<Employee> ShowSummary(DateTime startDate, DateTime endDate)
        {
            List<Employee> employeeSummaries = new List<Employee>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("sp_employee_summary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
                            Employee summary = new Employee();
                            summary.State = reader["State"].ToString();
                            summary.RevVar = reader.GetDouble(reader.GetOrdinal("RevVar"));
                            summary.VolVar = reader.GetDouble(reader.GetOrdinal("VolVar"));
                            employeeSummaries.Add(summary);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return employeeSummaries;
        }


    }
}
