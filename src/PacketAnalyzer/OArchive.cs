using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace PacketAnalyzer
{
    public class OArchive
    {
        private SQLiteConnection _dbConnection = null;
        private static string _archiveFolder = null;

        // static function
        public static string DBName()
        {
            return "packet.sqlite";
        }

        public static string GetArchiveFolder()
        {
            if (null == _archiveFolder)
            {
                _archiveFolder = Application.StartupPath;
                _archiveFolder += "\\packets";
            }
            return _archiveFolder;
        }

        ~OArchive()
        {
            UninitializeDB();
        }

        protected virtual bool InitializeDB(string strDBPath, bool bCreateAlways)
        {
            // if database is already initialized, return false
            if (null != _dbConnection)
            {
                return false;
            }

            string strConnectionString = "Data Source=";
            strConnectionString += strDBPath;
            strConnectionString += ";Version=3;";

            if (false == File.Exists(strDBPath))
            {
                // Create DB
                SQLiteConnection.CreateFile(strDBPath);
                _dbConnection = new SQLiteConnection(strConnectionString);
                _dbConnection.Open();

                // Make table
                string sqlCreateTable = "CREATE TABLE packets (collected_time LONG, src_ip TEXT, dest_ip TEXT, dest_port INTEGER, length INTEGER, packet_path TEXT)";
                SQLiteCommand cmd = new SQLiteCommand(sqlCreateTable, _dbConnection);
                cmd.ExecuteNonQuery();
                return true;
            }
            // at this time, we have already db, so just open database
            _dbConnection = new SQLiteConnection(strConnectionString);
            _dbConnection.Open();
            return true;
        }

        protected virtual void UninitializeDB()
        {
            // close database
        }

        private string _GetTempName()
        {
            string guid = System.Guid.NewGuid().ToString();
            return guid;
        }

        private bool _SavePacketAsFile(string path, byte[] rawdata)
        {
            FileStream f = new FileStream(path, FileMode.Create);
            // Write rawDaeta into the file
            f.Write(rawdata, 0, rawdata.Length);
            // close file handle
            f.Close();
            return true;
        }

        private string BuildSQLStatement(ulong from, ulong to, List<int> protocols)
        {
            string sql = "Select collected_time, src_ip, dest_ip, dest_port, length, packet_path from packets where ";
            sql += "(collected_time >= ";
            sql += from.ToString();
            sql += " and collected_time <= ";
            sql += to.ToString();
            sql += ")";

            // Find if there is all protocols
            bool bAllProtocol = false;
            for (int i=0; i<protocols.Count; i++)
            {
                if (0 == protocols[i])
                {
                    bAllProtocol = true;
                    break;
                }
            }

            // If we have to find all protocols, don't make
            // more condition statement for protocols
            if (true == bAllProtocol)
            {
                return sql;
            }
            
            string sqlProtocol = "(";
            for (int i=0; i<protocols.Count; i++)
            {
                sqlProtocol += "dest_port = ";
                sqlProtocol += protocols[i].ToString();
                // if it is not the last item
                if (i < (protocols.Count - 1))
                {
                    sqlProtocol += " or ";
                }
            }
            sqlProtocol += ")";

            if (protocols.Count > 0)
            {
                sql += " and ";
                sql += sqlProtocol;
            }

            return sql;
        }

        public bool OpenDatabase(String strDBPath, bool bCreateAlways)
        {
            return InitializeDB(strDBPath, bCreateAlways);
        }

        public void CloseDatabase()
        {
            UninitializeDB();
        }

        public void SavePacket(OPacket packet, byte[] rawData)
        {
            if (null == _dbConnection)
            {
                return;
            }

            // Save packet data into the file system
            string _path = OArchive.GetArchiveFolder();
            _path += "\\";
            _path += _GetTempName();

            // Save packet data
            if (false == _SavePacketAsFile(_path, rawData))
            {
                return;
            }

            packet.SetPacketStoredPath(_path);

            // Save into the Database
            string sql = "INSERT INTO packets (collected_time, src_ip, dest_ip, dest_port, length, packet_path) values ";
            string sqlValue = "(";
            sqlValue += packet.GetTimeCollected();
            sqlValue += ", ";
            sqlValue += "\"";
            sqlValue += packet.GetSourceIP();
            sqlValue += "\"";
            sqlValue += ", ";
            sqlValue += "\"";
            sqlValue += packet.GetDestinationIP();
            sqlValue += "\"";
            sqlValue += ", ";
            sqlValue += Convert.ToString(packet.GetProtocol());
            sqlValue += ", ";
            sqlValue += "\"";
            sqlValue += Convert.ToString(packet.GetLength());
            sqlValue += "\"";
            sqlValue += ", ";
            sqlValue += "\"";
            sqlValue += packet.GetPacketStoredPath();
            sqlValue += "\"";
            sqlValue += ")";
            sql += sqlValue;
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
        }
        public bool QueryPackets(ulong startTime, ulong endTime, List<int> protocolList, OCurrentPacketList curPackets)
        {
            if (null == _dbConnection)
            {
                return false;
            }

            // Build Query statement
            string sql = BuildSQLStatement(startTime, endTime, protocolList);
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                OPacket pac = new OPacket();
                pac.SetTimeCollected((ulong)(Int64)reader[0]);
                pac.SetSourceIP((string)reader[1]);
                pac.SetDestinationIP((string)reader[2]);
                pac.SetProtocol((int)(Int64)reader[3]);
                pac.SetLength((int)(Int64)reader[4]);
                pac.SetPacketStoredPath((string)reader[5]);

                curPackets.AddPacket(pac);
            }
            return true;
        }
    }
}
