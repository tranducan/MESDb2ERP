using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MesDb2ERPDb
{
  public  class LocalPLCSql 
    {

     //   public Model.SettingClass settingClass = (Model.SettingClass) Algorithm.SaveObject.Load_data(Algorithm.SaveObject.Pathsave);
        public SqlConnection conn = new SqlConnection(); //get from user database

        public string sqlExecuteScalarString(string sql, Model.SettingClass settingClass)
        {

            String outstring;
            try
            {
                conn = DBUtils.GetLocalPLCConnection(settingClass.OfflineServer,"ERPSOFT",settingClass.userOffline,settingClass.password);
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                outstring = cmd.ExecuteScalar().ToString();
                conn.Close();
                return outstring;
            }
            catch (Exception ex)
            {

                
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "Database Responce", ex.Message);
                return String.Empty;
            }


        }
      
        public void sqlDataAdapterFillDatatable(string sql, ref DataTable dt, Model.SettingClass settingClass)
        {
            try
            {
                conn = DBUtils.GetLocalPLCConnection(settingClass.OfflineServer, "ERPSOFT", settingClass.userOffline, settingClass.password);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                {
                    cmd.CommandText = sql;
                    cmd.Connection = conn;
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "Database Responce", ex.Message);
              

            }
        }
        public bool sqlExecuteNonQuery(string sql, bool result_message_show, Model.SettingClass settingClass)
        {
            try
            {
                conn = DBUtils.GetLocalPLCConnection(settingClass.OfflineServer, "ERPSOFT", settingClass.userOffline, settingClass.password);
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);

                int response = cmd.ExecuteNonQuery();
                if (response >= 1)
                {
                    if (result_message_show) { SystemLog.Output(SystemLog.MSG_TYPE.Err,"Successful!", "Database Responce"); }
                    conn.Close();
                    return true;
                }
                else
                {
                    SystemLog.Output(SystemLog.MSG_TYPE.Err, "Database Responce", "");
                    
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "Database Responce", ex.Message);
                conn.Close();
                return false;
            }
        }
    }
}
