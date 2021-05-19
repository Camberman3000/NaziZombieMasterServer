using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WorldAtWarMasterServer.Models
{
    public class DatabaseInterface
    {
        private MySqlConnection SQLConnection;
       public DatabaseInterface()
        {
            string ConnectionString = "server=127.0.0.1;uid=mark;pwd=Pebbles918;database=world_at_war_data";
            SQLConnection = new MySqlConnection(ConnectionString);
        }

        public string GetUserIPAddress()
        {
            string IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (IP == "::1")
            {
                IP = "127.0.0.1";
            }
            return IP;
        }
        public int PostData(ServerData Data)
        {
            try
            {

                SQLConnection.Open();

                // REMOVES ANY EXISTING SERVER FOR THE USERS IP ADDRESS IN THE DATABASE
                MySqlCommand Command = new MySqlCommand("DeleteServerEntry", SQLConnection);
                Command.CommandType = System.Data.CommandType.StoredProcedure;

                Command.Parameters.AddWithValue("_IPAddress", GetUserIPAddress());

                Command.ExecuteNonQuery();

                // ADDS A NEW ENTRY IN THE DB FOR THE USER (IP ADDRESS)
                Command = new MySqlCommand("AddServerEntry", SQLConnection);
                Command.CommandType = System.Data.CommandType.StoredProcedure;

                Random rNum = new Random();
                int ServerID = rNum.Next(1, 2000000000);

                Command.Parameters.AddWithValue("_ServerID", ServerID);
                Command.Parameters.AddWithValue("_IPAddress", GetUserIPAddress());
                Command.Parameters.AddWithValue("_ServerName", Data.ServerName);
                Command.Parameters.AddWithValue("_MapName", Data.MapName);
                Command.Parameters.AddWithValue("_CurrentPlayers", Data.CurrentPlayers);
                Command.Parameters.AddWithValue("_MaxPlayers", Data.MaxPlayers);

                Command.ExecuteNonQuery();
                SQLConnection.Close();
                return ServerID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public void DeleteData()
        {
            try
            {
                SQLConnection.Open();

                MySqlCommand Command = new MySqlCommand("DeleteServerEntry", SQLConnection);
                Command.CommandType = System.Data.CommandType.StoredProcedure;                

                Command.Parameters.AddWithValue("_IPAddress", GetUserIPAddress());               

                Command.ExecuteNonQuery();
                SQLConnection.Close();                 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public DataTable GetAllServers()
        {
            try
            {
                SQLConnection.Open();

                MySqlCommand Command = new MySqlCommand("GetAllServerEntries", SQLConnection);
                Command.CommandType = System.Data.CommandType.StoredProcedure;

                DataTable DT = new DataTable();
                DT.Load(Command.ExecuteReader());

                Command.ExecuteNonQuery();
                SQLConnection.Close();

                return DT;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new DataTable();
            }
        }

        public void UpdateServerEntry(ServerData Data)
        {
            try
            {
                SQLConnection.Open();

                // UPDATES AN ENTRY IN THE DB FOR THE USER (IP ADDRESS)
                MySqlCommand Command = new MySqlCommand("UpdateServerEntry", SQLConnection);
                Command.CommandType = System.Data.CommandType.StoredProcedure;

                Random rNum = new Random();
                int ServerID = rNum.Next(1, 2000000000);
                
                Command.Parameters.AddWithValue("_IPAddress", GetUserIPAddress());
                Command.Parameters.AddWithValue("_ServerName", Data.ServerName);
                Command.Parameters.AddWithValue("_MapName", Data.MapName);
                Command.Parameters.AddWithValue("_CurrentPlayers", Data.CurrentPlayers);
                Command.Parameters.AddWithValue("_MaxPlayers", Data.MaxPlayers);

                Command.ExecuteNonQuery();
                SQLConnection.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}