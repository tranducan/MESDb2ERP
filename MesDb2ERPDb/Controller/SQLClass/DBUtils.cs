﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MesDb2ERPDb
{
    class DBUtils
    {

        public static SqlConnection GetDBConnection()
        {
          

            string datasource = "172.16.0.12";
            string database = "ERPSOFT";
            string username = "ERPUSER";
            string password = "12345";



            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }


        public static SqlConnection GetERPDBConnection()
        {
            //Data Source = LONG; Initial Catalog = TEST; Integrated Security = True
            string datasource = "172.16.0.11";
            string database = "TLVN2";
            string username = "soft";
            string password = "techlink@!@#";


            return DBSQLServerUtils.GetERPDBConnection(datasource, database, username, password);
        }
        public static SqlConnection GetSFT_TLVN2Connection()
        {
            //Data Source = LONG; Initial Catalog = TEST; Integrated Security = True
            string datasource = "172.16.0.11";
            string database = "SFT_TLVN2";
            string username = "soft";
            string password = "techlink@!@#";

            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }
        public static SqlConnection GetSFTDBConnection()
        {
            //Data Source = LONG; Initial Catalog = TEST; Integrated Security = True
            string datasource = "172.16.0.11";
            string database = "SFT_TLVN2";
            string username = "soft";
            string password = "techlink@!@#";


            return DBSQLServerUtils.GetSFTDBConnection(datasource, database, username, password);
        }
        public static SqlConnection GetERPTargetBConnection()
        {
            //Data Source = LONG; Initial Catalog = TEST; Integrated Security = True
            string datasource = "172.16.0.11";
            string database = "SOT";
            string username = "sa";
            string password = "dsc@123";


            return DBSQLServerUtils.GetSFTDBConnection(datasource, database, username, password);
        }
        public static SqlConnection GetLocalPLCConnection(string _datasource, string  _database, string _user, string _pass)
        {
            //Data Source = LONG; Initial Catalog = TEST; Integrated Security = True
            string datasource = _datasource;
            string database =_database;
            string username = _user;
            string password = _pass;


            return DBSQLServerUtils.GetLocalPLCConnection(datasource, database, username, password);
        }
    }
}
