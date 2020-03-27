using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesDb2ERPDb
{
   public class UploadMain
    {
        string strLot = "";
    public    void GetListLOT()
        {
            try
            {

                string sqlgetListLOT = "select distinct lot  from m_ERPMQC_REALTIME  where data not like '0' and status  like '' ";
                ComboBox cmb_ = new ComboBox();
                sqlCON data = new sqlCON();
                data.getComboBoxData(sqlgetListLOT, ref cmb_);
                string inspectdate = DateTime.Now.ToString("yyyy-MM-dd");
                string inspecttime = DateTime.Now.ToString("HH:mm:ss");
                string DateUp = DateTime.Now.ToString("yyyyMMdd");
                string TimeUp = DateTime.Now.ToString("HH:mm:ss");
                string TimeSerno = DateTime.Now.ToString("HHmmss");
                //   if (cmb_.Items.Count == 0) return;
                for (int cmbitem = 0; cmbitem < cmb_.Items.Count; cmbitem++)
                {
                    string serno = cmb_.Items[cmbitem].ToString().Split(';')[0] + "-" + DateUp + "_" + TimeSerno;
                    DataTable table = new DataTable();
                    StringBuilder sqlGetTable = new StringBuilder();
                    sqlGetTable.Append("select '" + serno + "' as serno,");
                    sqlGetTable.Append("lot, model, site, factory, line, process,item,");
                    sqlGetTable.Append("'" + inspectdate + "' as inspectdate,");
                    sqlGetTable.Append("'" + inspecttime + "' as inspecttime,");
                    sqlGetTable.Append("sum(cast(data as int)) as data,");
                    sqlGetTable.Append("'0' as judge , status, remark from m_ERPMQC_REALTIME  ");
                    sqlGetTable.Append("where data not like '0' and status  like '' and lot = '" + cmb_.Items[cmbitem].ToString() + "'");
                    sqlGetTable.Append("group by item,lot,model,site, factory, line, process, item,  status, remark");
                    data.sqlDataAdapterFillDatatable(sqlGetTable.ToString(), ref table);

                    int intCountOK = CounterỌKERP(ref table);
                    int intCountNG = CounterNGERP(ref table);
                    strLot = table.Rows[0]["lot"].ToString();
                    string code = table.Rows[0]["serno"].ToString().Split('-')[0];
                    string No = table.Rows[0]["serno"].ToString().Split('-')[1];
                    //string serno = table.Rows[0]["serno"].ToString();
                    string typeNG = "";
                    int checkdouble = 0;
                    //check file trung
                    string sqlERP = "select count(*) from m_ERPMQC_Error where serno = '" + serno + "'";
                    string sqlERPError = "select count(*) from m_ERPMQC_REALTIME where serno = '" + serno + "'";
                    checkdouble = int.Parse(data.sqlExecuteScalarString(sqlERP)) + int.Parse(data.sqlExecuteScalarString(sqlERPError));
                    //An Them Code Check Nguyen Vat Lieu ngay 10/05/2019
                    string MaLSX = code + "-" + No;
                    Material material = new Material();
                    bool IsDuSoLuong = false;
                    bool IsNVL = false;
                    List<string> _messages = new List<string>();
                    List<MaterialAdapt> listMaterial = new List<MaterialAdapt>();
                    double SL_UPload = intCountOK + intCountNG;
                    //Chua ma Lenh San xuat vao 2 truong dang dua vao
                    bool IsResultheck = material.KiemtraNguyenVatLieu(code, No, SL_UPload, out IsDuSoLuong, out IsNVL, out listMaterial, out _messages);
                    insertERP classinsert = new insertERP();
                    DefectClass defectClass = new DefectClass();
                    if ((intCountNG + intCountOK > 0) && checkdouble == 0)
                    {

                        //update to realtime

                        if (IsResultheck == true)
                        {
                            classinsert.InsertdataToERP(table.Rows[0]["lot"].ToString(), intCountOK.ToString(), intCountNG.ToString(), DateUp, TimeUp);
                            classinsert.updateERP(table.Rows[0]["lot"].ToString(), intCountOK.ToString(), intCountNG.ToString(), DateUp, TimeUp);
                            classinsert.InsertdataToSFT(table.Rows[0]["lot"].ToString(), intCountOK.ToString(), intCountNG.ToString(), DateUp, TimeUp);
                            classinsert.UpdatedataToSFT(table.Rows[0]["lot"].ToString(), intCountOK.ToString(), intCountNG.ToString(), DateUp, TimeUp);


                            for (int i = 0; i < table.Rows.Count; i++)
                            {
                                typeNG = table.Rows[i]["item"].ToString();
                                string SL = table.Rows[i]["data"].ToString();

                                if (int.Parse(SL) > 0 && typeNG.Contains("NG"))
                                {
                                    var insert = defectClass.InsertToSFT_OP_EXCEPT(MaLSX, typeNG, int.Parse(SL), classinsert.Sequence_OP_REAL_RUN);
                                }
                            }
                            classinsert.InsertToERPMQC(table);
                            classinsert.UpdateToERPMQC_Realtime("OK", strLot);
                            classinsert.updateERPMQC(table.Rows[0]["serno"].ToString());
                            classinsert.UpdateToERPMQC_Error("OK", table.Rows[0]["serno"].ToString());
                        }
                        else //insert to Temperate database
                        {

                            classinsert.InsertToERPMQC_Error(table);
                            classinsert.InsertToERPMQC(table);
                            classinsert.UpdateToERPMQC_Realtime("OK", strLot);
                        }
                    }
                }
                UpdateFromERPMQC_ErrorToSFT_ERP(); //waiting
            }

            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, " GetListLOT", ex.Message);
            }

        }
        public void UpdateFromERPMQC_ErrorToSFT_ERP()
        {
            try
            {


                StringBuilder sqlquery = new StringBuilder();
                sqlquery.Append(@"select distinct
serno,lot,model,site,factory,line,process,item,inspectdate,inspecttime,data,judge,status,remark
from m_ERPMQC_Error 
where status !='OK' ");
                sqlCON data = new sqlCON();
                DataTable dtTable = new DataTable();
                data.sqlDataAdapterFillDatatable(sqlquery.ToString(), ref dtTable);

                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    Material material = new Material();
                    bool IsDuSoLuong = false;
                    bool IsNVL = false;
                    List<string> _messages = new List<string>();
                    List<MaterialAdapt> listMaterial = new List<MaterialAdapt>();
                    //Chua ma Lenh San xuat vao 2 truong dang dua vao
                    string code = dtTable.Rows[i]["serno"].ToString().Split('-')[0];
                    string No = dtTable.Rows[i]["serno"].ToString().Split('-')[1];
                    double dataQTY = dtTable.Rows[i]["data"].ToString() != null ? double.Parse(dtTable.Rows[i]["data"].ToString()) : 0;

                    string DateUp = DateTime.Parse(dtTable.Rows[i]["inspectdate"].ToString()).ToString("yyyyMMdd");
                    string TimeUp = dtTable.Rows[i]["inspecttime"].ToString().Substring(0, 8);
                    string typeNG = "";
                    string MaLSX = code + "-" + No;
                    bool IsResultheck = material.KiemtraNguyenVatLieu(code, No, dataQTY, out IsDuSoLuong, out IsNVL, out listMaterial, out _messages);
                    DefectClass defectClass = new DefectClass();
                    if (IsNVL && (dtTable.Rows[i]["data"].ToString() != "0"))
                    {//Update Status
                        string test = dtTable.Rows[i]["remark"].ToString();
                        int countOK = dtTable.Rows[i]["remark"].ToString() == "OP" ? int.Parse(dtTable.Rows[i]["data"].ToString()) : 0;
                        int countNG = dtTable.Rows[i]["remark"].ToString() == "NG" ? int.Parse(dtTable.Rows[i]["data"].ToString()) : 0;
                        insertERP classinsert = new insertERP();
                        classinsert.InsertdataToERP(dtTable.Rows[i]["lot"].ToString(), countOK.ToString(), countNG.ToString(), DateUp, TimeUp);
                        classinsert.updateERP(dtTable.Rows[i]["lot"].ToString(), countOK.ToString(), countNG.ToString(), DateUp, TimeUp);
                        classinsert.InsertdataToSFT(dtTable.Rows[i]["lot"].ToString(), countOK.ToString(), countNG.ToString(), DateUp, TimeUp);
                        classinsert.UpdatedataToSFT(dtTable.Rows[i]["lot"].ToString(), countOK.ToString(), countNG.ToString(), DateUp, TimeUp);
                        classinsert.updateERPMQC(dtTable.Rows[i]["serno"].ToString());
                        classinsert.UpdateToERPMQC_Error("OK", dtTable.Rows[i]["serno"].ToString());
                        // for (int j = 0; j < dtTable.Rows.Count; j++)
                        {
                            typeNG = dtTable.Rows[i]["item"].ToString();
                            string SL = dtTable.Rows[i]["data"].ToString();

                            if (int.Parse(SL) > 0 && typeNG.Contains("NG"))
                            {
                                var insert = defectClass.InsertToSFT_OP_EXCEPT(MaLSX, typeNG, int.Parse(SL), classinsert.Sequence_OP_REAL_RUN);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "UpdateFromERPMQC_ErrorToSFT_ERP ()", ex.Message);
            }

        }
        public int CounterỌKERP(ref DataTable dt)
        {
            int OK = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["remark"].ToString() == "OP") OK += int.Parse(dt.Rows[i]["data"].ToString());
            }
            return OK;
        }
        //Counter NG < chưa dung để đó>
        public int CounterNGERP(ref DataTable dt)
        {
            int NG = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["remark"].ToString() == "NG") NG += int.Parse(dt.Rows[i]["data"].ToString());
            }
            return NG;
        }
    }
}
