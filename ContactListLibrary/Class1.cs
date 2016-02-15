using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.IO;

namespace ContactListLibrary
{
    public static class Connection
    {
        const string CON_STR = "Data Source=ACADEMY009-VM;Initial Catalog=Contacts;Integrated Security=SSPI";

        public static void EditContact(string id, string firstname, string lastname, string ssn)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR:
            
            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;


                myCommand.CommandText = $"UPDATE Contact set firstname = '{firstname}', lastname = '{lastname}', ssn = '{ssn}' where ID = {id}";
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void DeleteContact(string id)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;

            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;


                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "spDeleteContact";
                SqlParameter paraID = new SqlParameter("@ID", SqlDbType.Int);
                paraID.Value = id;
                myCommand.Parameters.Add(paraID);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void AddContact(string firstname, string lastname, string ssn)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;

            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;

                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "spAddContact";

                SqlParameter paramFirstname = new SqlParameter("@firstname", SqlDbType.VarChar);

                paramFirstname.Value = firstname;
                myCommand.Parameters.Add(paramFirstname);

                SqlParameter paramLastname = new SqlParameter("@lastname", SqlDbType.VarChar);
                paramLastname.Value = lastname;
                myCommand.Parameters.Add(paramLastname);

                SqlParameter paramSSN = new SqlParameter("@SSN", SqlDbType.VarChar);
                paramSSN.Value = ssn;
                myCommand.Parameters.Add(paramSSN);

                SqlParameter paramID = new SqlParameter("@new_id", SqlDbType.Int);
                paramID.Direction = ParameterDirection.Output;
                myCommand.Parameters.Add(paramID);

                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void AddAdress(string id, string type, string street, string city)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;

            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;

                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "spAddAdress";

                SqlParameter paraType = new SqlParameter("@type", SqlDbType.VarChar);

                paraType.Value = type;
                myCommand.Parameters.Add(paraType);

                SqlParameter paraStreet = new SqlParameter("@street", SqlDbType.VarChar);
                paraStreet.Value = street;
                myCommand.Parameters.Add(paraStreet);

                SqlParameter paraCity = new SqlParameter("@city", SqlDbType.VarChar);
                paraCity.Value = city;
                myCommand.Parameters.Add(paraCity);

                SqlParameter paramID = new SqlParameter("@new_cid", SqlDbType.Int);
                paramID.Value = id;
                myCommand.Parameters.Add(paramID);

                SqlParameter paramAID = new SqlParameter("@new_id", SqlDbType.Int);
                paramAID.Direction = ParameterDirection.Output;
                myCommand.Parameters.Add(paramAID);

                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void EditAdress(string id, string type, string street, string city)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;
            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;

                myCommand.CommandText = $"UPDATE Adresses set type = '{type}', street = '{street}', city = '{city}' where ID = '{id}'";
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static void DeleteAdress(string id)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;

            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                myCommand.CommandText = $"DELETE from Adresses where id = {id}";
                myCommand.ExecuteNonQuery();
                myCommand.CommandText = $"DELETE from Midtabel where AID = {id}";
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
        }
        public static string Show()
        {
            string info = "";

            info = $"<div class=\"container\">";
            info += $"<div class=\"table - responsive\">";
            info += $"<table class=\"table\">";
            info += $"<thead>";
            info += $"<tr>";
            info += $"<th>#</th>";
            info += $"<th>Förnamn</th>";
            info += $"<th>Efternamn</th>";
            info += $"</tr>";
            info += $"</thead>";

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;

            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;

                myCommand.CommandText = $"SELECT * from Contact";
                SqlDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    info += $"<tr>";
                    info += $"<td>{myReader["id"]}</td>";
                    info += $"<td>{myReader["firstname"]}</td>";
                    info += $"<td>{myReader["lastname"]}</td>";
                    info += $"<td></td>";
                    info += $"<td><a href=\".//mainEditContact.aspx?id={myReader["id"]}\">Editera</a></td><td><a href=\".//mainViewContactAdress.aspx?id={myReader["id"]}\">Titta</a></td><td><a href=\".//main.aspx?delete={myReader["id"]}\">Terminera</a></td>";
                }
                myReader.Close();
                myCommand.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
            info += $"</tr>";
            info += $"</tbody>";
            info += $"</table>";
            info += $"</div>";
            info += $"</div>";
            return info;
        }
        public static string ShowAdresses(string AID)
        {
            string info = "";
            info = $"<div class=\"container\">";
            info += $"<div class=\"table - responsive\">";
            info += $"<table class=\"table\">";
            info += $"<thead>";
            info += $"<tr>";
            info += $"<th>#</th>";
            info += $"<th>Typ av adress</th>";
            info += $"<th>Gata</th>";
            info += $"<th>City</th>";
            info += $"</tr>";
            info += $"</thead>";


            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = CON_STR;


            try
            {
                myConnection.Open();

                SqlCommand myCommand = new SqlCommand();
                myCommand.Connection = myConnection;
                SqlParameter paramID = new SqlParameter("@ID", SqlDbType.Int);
                paramID.Value = AID;

                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add(paramID);
                myCommand.CommandText = "spPrintAdress";
                SqlDataReader myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    info += $"<tr>";
                    info += $"<td>{myReader["id"]}</td>";
                    info += $"<td>{myReader["type"]}</td>";
                    info += $"<td>{myReader["street"]}</td>";
                    info += $"<td>{myReader["city"]}</td>";
                    info += $"<td></td>";
                    info += $"<td><a href=\"#\" onClick=\"showModal('{myReader["id"]}','{myReader["type"]}','{myReader["street"]}','{myReader["city"]}');\">Editera</a></td><td><a href=\"./mainViewContactAdress.aspx?DELETE={myReader["id"]}&id={AID}\">Terminera</a></td>";
                }
                myReader.Close();
                myCommand.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                myConnection.Close();
            }
            info += $"</tr>";
            info += $"</tbody>";
            info += $"</table>";
            info += $"</div>";
            info += $"</div>";

            return info;
        }
    }
}

