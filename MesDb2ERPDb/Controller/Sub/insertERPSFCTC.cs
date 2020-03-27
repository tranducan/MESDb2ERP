using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;


namespace MesDb2ERPDb
{
    public class insertERP
    {
        string TOID = "";
        double LOTSIZE_B02 = 0;
        string ITEMID_TC047 = "";
        string ITEMNAME_TC048 = "";
        string ITEMDESCRIPTION = "";
        string TRANSNO = "";
        string TO008 = "";
        public int Sequence_OP_REAL_RUN = 0;
        public void InsertdataToERP(string barcode, string output, string NG, string date, string time)
        {
            try
            {
                string[] QR = barcode.Split(';');
                string[] ML = QR[0].Split('-');
                //string date = DateTime.Now.ToString("yyyyMMdd");
                //string time = DateTime.Now.ToString("HH:mm:ss");
                // string month = DateTime.Now.ToString("yyMM");
                string month = date.Substring(2, 6);
                sqlERPCON sqlERPCON = new sqlERPCON();
                // An chinh sua
                string TC002 = GetTC002();
                TO008 = TC002;
                TOID = "D201-" + TC002; //chay thuc te
                //
                string TC047 = sqlERPCON.sqlExecuteScalarString("select distinct TA006 from MOCTA where TA001 = '" + ML[0] + "' and TA002 = '" + ML[1] + "'");
                ITEMID_TC047 = TC047;
                string TC048 = sqlERPCON.sqlExecuteScalarString("select distinct TA034 from MOCTA where TA001 = '" + ML[0] + "' and TA002 = '" + ML[1] + "'");
                ITEMNAME_TC048 = TC048;
                SQLSFTTLVN2 sQLSFTTLVN2 = new SQLSFTTLVN2();
                ITEMDESCRIPTION = sQLSFTTLVN2.sqlExecuteScalarString("select distinct  ITEMDESCRIPTION from  SFT_TRANSORDER_LINE where TRANSORDERTYPE = 'D101' and KEYID = '" + QR[0] + "'");


                string TA007 = sqlERPCON.sqlExecuteScalarString("select distinct TA007 from SFCTA where TA004 ='B01' and  TA001 = '" + ML[0] + "' and TA002 = '" + ML[1] + "'");
                int TC036 = int.Parse(output) + int.Parse(NG);
                // AN chinh sua
                TRANSNO = GetTB039(date); //update to transfer table

                sqlERPCON sqlInsert = new sqlERPCON();//
                StringBuilder sqlInsertSFCTC = new StringBuilder();
                sqlInsertSFCTC.Append("insert into SFCTC ");
                sqlInsertSFCTC.Append(@"(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,MODIFIER,MODI_DATE,FLAG,CREATE_TIME,CREATE_AP,CREATE_PRID,MODI_TIME,MODI_AP,MODI_PRID,");
                sqlInsertSFCTC.Append(@"TC001,TC002,TC003,TC004,TC005,TC006,TC007,TC008,TC009,TC010,TC011,TC012,TC013,TC014,TC015,TC016,TC017,TC018,TC019,TC020,");
                sqlInsertSFCTC.Append(@"TC021,TC022,TC023,TC024,TC025,TC026,TC027,TC033,TC034,TC035,TC036,TC037,TC038,TC039,TC040,");
                sqlInsertSFCTC.Append(@"TC041,TC042,TC043,TC044,TC045,TC046,TC047,TC048,TC049,TC050,TC051,TC055)");
                sqlInsertSFCTC.Append(" values ( ");
                sqlInsertSFCTC.Append("'TECHLINK','BQC01','JG01','" + date + "','BQC01','" + date + "',2,'" + time + "','SFT','Sftb03','" + time + "','SFT','',");
                sqlInsertSFCTC.Append("'D201','" + TC002 + "','0001','" + ML[0] + "','" + ML[1] + "','" + QR[1] + "','" + QR[2] + "','0020','B02','PCS','','','1'," + output + ",0," + NG + ",0,0,0,0,");
                sqlInsertSFCTC.Append("0,'Y','B01','',0,'N','N','" + date + "','" + date + "','N'," + TC036 + ",0,'" + date + "','0','',");
                sqlInsertSFCTC.Append("'B01',0,0,0,0,0,'" + TC047 + "','" + TC048 + "','" + ITEMDESCRIPTION + "','KG','0','N'");  //update new 21-oct
                sqlInsertSFCTC.Append(")");
                sqlInsert.sqlExecuteNonQuery(sqlInsertSFCTC.ToString(),false);


                StringBuilder sqlInsertSFCTB = new StringBuilder();
                sqlInsertSFCTB.Append("insert into SFCTB ");
                sqlInsertSFCTB.Append("(COMPANY,CREATOR,USR_GROUP,CREATE_DATE,MODIFIER,MODI_DATE,FLAG,CREATE_TIME,CREATE_AP,CREATE_PRID,MODI_TIME,MODI_AP,MODI_PRID,");
                sqlInsertSFCTB.Append("TB001,TB002,TB003,TB004,TB005,TB006,TB007,TB008,TB009,TB010,TB011,TB012,TB013,TB014,TB015,TB016,TB017,TB018,TB019,TB020,");
                sqlInsertSFCTB.Append(" TB021,TB022,TB023,TB024,TB025,TB026,TB031,TB036,TB037,TB038,TB039,");
                sqlInsertSFCTB.Append("TB200,TB201,TB202)");

                sqlInsertSFCTB.Append(" values ( ");
                sqlInsertSFCTB.Append("'TECHLINK','BQC01','JG01','" + date + "','BQC01','" + date + "',2,'" + time + "','SFT','Sftb03','" + time + "','SFT','',");
                sqlInsertSFCTB.Append("'D201','" + TC002 + "','" + date + "','1','B01','" + TA007 + "','1','B01','" + TA007 + "','TL',0,'N','Y','','" + date + "','ERP','N','','1','',");
                sqlInsertSFCTB.Append("'','1','1','','" + month + "',0.2,'0','VND',1,'D201','" + TRANSNO + "',");
                sqlInsertSFCTB.Append(int.Parse(output) + int.Parse(NG) + "," + output + "," + NG + " "); //ap them chi dieu yeu cau
                sqlInsertSFCTB.Append(")");
                sqlInsert.sqlExecuteNonQuery(sqlInsertSFCTB.ToString(),false);

            }
            catch (Exception ex)
            {

               SystemLog.Output(SystemLog.MSG_TYPE.Err, "InsertdataToERP(string barcode, string output, string NG)", ex.Message);
            }

        }
        public string GetTB039(string date_)
        {
            string _TB039 = "";
            // string dateFormat = DateTime.Now.ToString("yyyyMMdd").Substring(1);
            string dateFormat = date_.Substring(1);
            string countFormatReset = "0001";
            int countUp = 0;
            SQLSFTTLVN2 data = new SQLSFTTLVN2();
            StringBuilder sql = new StringBuilder();
            sql.Append(@" WITH A AS
(SELECT TRANSTYPE, TRANSDATE, TRANSNO FROM SFT_TRANSORDER  WHERE  TRANSTYPE = 'D201'  AND TRANSDATE = '" + date_ + "'   AND TRANSNO LIKE  '" + dateFormat + "%'" +
"UNION ALL SELECT TRANSTYPE, TRANSDATE, TRANSNO FROM SFT_TRANSORDER_FIN WHERE  TRANSTYPE = 'D201'  AND TRANSDATE = '" + date_ + "'   AND TRANSNO LIKE  '" + dateFormat + "%')SELECT ISNULL(MAX(TRANSNO),'') AS TRANSNO  FROM A");


            //string strData = data.sqlExecuteScalarString("select max(TRANSNO) from SFT_TRANSORDER where TRANSTYPE = 'D201'  ", SQLCommon.SFT_TL_V2);
            string strData = data.sqlExecuteScalarString(sql.ToString());
            if (strData != null && strData.Trim().Count() == 11)
            {
                string DateDatabase = strData.Trim().Substring(0, 7);
                string strCount = strData.Trim().Substring(7);

                countUp = int.Parse(strCount) + 1;
                string countFormatup = countUp.ToString("0000");
                _TB039 = dateFormat + countFormatup;

            }
            else
            {
                _TB039 = dateFormat + countFormatReset;
            }

            return _TB039;
        }
        public string GetTC002()
        {
            string _TC002 = "";
            string dateFormat = DateTime.Now.ToString("yyMM");
            string countFormatReset = "0001";
            int countUp = 0;
            sqlERPCON data = new sqlERPCON();
            string strData = data.sqlExecuteScalarString("select max(TB002) from SFCTB where TB001 = 'D201'  ");
            if (strData != null && strData.Trim().Count() == 8)
            {
                string DateDatabase = strData.Trim().Substring(0, 4);
                string strCount = strData.Trim().Substring(4);
                if (dateFormat == DateDatabase)
                {
                    countUp = int.Parse(strCount) + 1;
                    string countFormatup = countUp.ToString("0000");
                    _TC002 = dateFormat + countFormatup;
                }
                else
                {
                    _TC002 = dateFormat + countFormatReset;
                }
            }

            return _TC002;
        }
        public string GetTC002_Down()
        {
            string _TC002 = "";
            string dateFormat = DateTime.Now.ToString("yyMM");
            string firstvalue = dateFormat + "9999";
            sqlCON data = new sqlCON();
            string check9999 = data.sqlExecuteScalarString("select count(*) TB002 from m_SFTKEY where TB002 = '" + firstvalue + "'");
            if (int.Parse(check9999) != 0)
            {
                string minTB002 = data.sqlExecuteScalarString("select min(TB002) from m_SFTKEY where TB002 like '" + dateFormat + "%'");
                int valueset = int.Parse(minTB002.Trim().Substring(4, 4));
                valueset = valueset - 1;
                _TC002 = dateFormat + valueset.ToString("0000");
            }
            else
            {
                _TC002 = firstvalue;
            }
            //insertnextvalue
            string sqlinsert = "insert into m_SFTKEY(TB002, datetimeRST) values ('" + _TC002 + "', getdate()) ";
            data.sqlExecuteNonQuery(sqlinsert,false);
            return _TC002;
        }
        public string GetTB039_Down(string date_)
        {
            string _TC002 = "";
            string dateFormat = date_.Substring(1);
            string firstvalue = dateFormat + "9999";
            sqlCON data = new sqlCON();
            string check9999 = data.sqlExecuteScalarString("select count(*) TRANSNO from m_SFTKEY where TRANSNO = '" + firstvalue + "'" );
            if (int.Parse(check9999) != 0)
            {
                string minTB002 = data.sqlExecuteScalarString("select min(TRANSNO) from m_SFTKEY where TRANSNO like '" + dateFormat + "%'" );
                int valueset = int.Parse(minTB002.Trim().Substring(7, 4));
                valueset = valueset - 1;
                _TC002 = dateFormat + valueset.ToString("0000");
            }
            else
            {
                _TC002 = firstvalue;
            }
            //insertnextvalue
            string sqlinsert = "insert into m_SFTKEY(TRANSNO, datetimeRST) values ('" + _TC002 + "', getdate()) ";
            data.sqlExecuteNonQuery(sqlinsert,false);
            return _TC002;
        }
        public void updateERP(string barcode, string output, string NG, string date, string time)
        {
            try
            {
                string[] QR = barcode.Split(';');
                string[] ML = QR[0].Split('-');
                sqlERPCON data = new sqlERPCON();
                //string TA011 = data.sqlExecuteScalarString("select sum(TC014)  from SFCTC where TC004 = '" + ML[0] + "' and   TC005 = '" + ML[1] + "' and TC001 = 'D201'", SQLCommon.ERP_TL_V2);
                //string TA012 = data.sqlExecuteScalarString("select sum(TC016)  from SFCTC where TC004 = '" + ML[0] + "' and   TC005 = '" + ML[1] + "' and TC001 = 'D201'", SQLCommon.ERP_TL_V2);

                //update SFCTA row B01
                StringBuilder updateSFCTA = new StringBuilder();
                updateSFCTA.Append("update SFCTA set TA011 = TA011 +" + output + ", TA012 = TA012+ " + NG + ",TA031 ='" + date + "' where TA001 = '" + ML[0] + "' and TA002 = '" + ML[1] + "' and TA003 = '" + QR[1] + "' and TA004 = '" + QR[2] + "'");
                sqlERPCON sqlUpdate = new sqlERPCON ();
                sqlUpdate.sqlExecuteNonQuery(updateSFCTA.ToString(),false);

                //update SFCTA row B02
                StringBuilder updateSFCTAB02 = new StringBuilder();
                updateSFCTAB02.Append("update SFCTA set TA010 = TA010 + " + output + ", MODIFIER ='BQC01', MODI_DATE ='" + date + "',MODI_TIME ='" + time + "', MODI_AP ='SFT', MODI_PRID ='', TA030= '" + date + "' where TA001 = '" + ML[0] + "' and TA002 = '" + ML[1] + "' and TA003 ='0020' and TA004 = 'B02' ");
                sqlUpdate.sqlExecuteNonQuery(updateSFCTAB02.ToString(), false);
            }
            catch (Exception ex)
            {

               SystemLog.Output(SystemLog.MSG_TYPE.Err, "updateERP(string barcode)", ex.Message);
            }
        }
        public void updateERPMQC(string serno)
        {
            try
            {

                StringBuilder update = new StringBuilder();
                update.Append("update m_ERPMQC set status = 'OK' where serno = '" + serno + "'");
                sqlCON data = new sqlCON();
                data.sqlExecuteNonQuery(update.ToString(),false);
            }
            catch (Exception ex)
            {

                SystemLog.Output(SystemLog.MSG_TYPE.Err, "updateERPMQC(string serno)", ex.Message);
            }
        }
        public bool InsertToERPMQC_Error(DataTable table)
        {
            try
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["data"].ToString().Trim() != "0" && double.Parse(table.Rows[i]["data"].ToString()) > 0)
                    {
                        StringBuilder sqlInsert = new StringBuilder();
                        sqlInsert.Append("insert into m_ERPMQC_Error ");
                        sqlInsert.Append(@"(serno, lot, model, site, factory, line, process, item, inspectdate, inspecttime, data, judge, status, remark )");
                        sqlInsert.Append(" values ( ");
                        sqlInsert.Append("'" + table.Rows[i]["serno"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["lot"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["model"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["site"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["factory"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["line"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["process"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["item"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["inspectdate"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["inspecttime"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["data"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["judge"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["status"].ToString() + "', ");
                        sqlInsert.Append("'" + table.Rows[i]["remark"].ToString() + "' ) ");
                        sqlCON data = new sqlCON();
                        data.sqlExecuteNonQuery(sqlInsert.ToString(),false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "InsertToERPMQC_Error(DataTable table, string CountSummary,string Remark)", ex.Message);
                return false;
            }

        }
        public bool InsertToERPMQC(DataTable table)
        {
            try
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i]["data"].ToString().Trim() != "0" && double.Parse(table.Rows[i]["data"].ToString()) > 0)
                    {
                        StringBuilder sqlInsert = new StringBuilder();
                        sqlInsert.Append("insert into m_ERPMQC ");
                        sqlInsert.Append(@"(serno, lot, model, site, factory, line, process, item, inspectdate, inspecttime, data, judge, status, remark )");
                        sqlInsert.Append(" values ( ");
                        sqlInsert.Append("'" + table.Rows[i]["serno"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["lot"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["model"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["site"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["factory"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["line"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["process"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["item"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["inspectdate"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["inspecttime"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["data"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["judge"].ToString() + "' , ");
                        sqlInsert.Append("'" + table.Rows[i]["status"].ToString() + "', ");
                        sqlInsert.Append("'" + table.Rows[i]["remark"].ToString() + "' ) ");
                        sqlCON data = new sqlCON();
                        data.sqlExecuteNonQuery(sqlInsert.ToString(), false);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "InsertToERPMQC(DataTable table)", ex.Message);
                return false;
            }

        }
        public bool UpdateToERPMQC_Error(string Status, string serno)
        {
            try
            {
                StringBuilder sqlupdate = new StringBuilder();
                sqlupdate.Append("update m_ERPMQC_Error set status = '" + Status + "' where ");
                sqlupdate.Append("serno = '" + serno + "'");
               sqlCON data = new sqlCON();
                data.sqlExecuteNonQuery(sqlupdate.ToString(), false);
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, " UpdateToERPMQC_Error(string Status, string serno)", ex.Message);
                return false;
            }

        }
        public bool UpdateToERPMQC_Realtime(string Status, string lot)
        {
            try
            {
                StringBuilder sqlupdate = new StringBuilder();
                sqlupdate.Append("update m_ERPMQC_REALTIME set status = '" + Status + "' where ");
                sqlupdate.Append("lot = '" + lot + "' and data not like '0' and status  like ''");
                sqlCON data = new sqlCON();
                data.sqlExecuteNonQuery(sqlupdate.ToString(), false);
                return true;
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "  UpdateToERPMQC_Realtime(string Status, string lot)", ex.Message);
                return false;
            }

        }
        public void UpdatedataToSFT(string barcode, string output, string NG, string date, string time)
        {
            try
            {
                string datetime = date + " " + time;

                SQLSFTTLVN2 data = new SQLSFTTLVN2();
                string[] QR = barcode.Split(';');
                string[] ML = QR[0].Split('-');

                string datetimefrist = data.sqlExecuteScalarString("select distinct ARRIVETIME from SFT_OP_REALRUN where  ID = '" + QR[0] + "' and OPID = 'B01---B01' and SEQUENCE  = 0");
                StringBuilder update = new StringBuilder();
                update.Append("update SFT_OP_REALRUN set ALERADYDEFECTQTY = ALERADYDEFECTQTY+  " + NG + ",");
                update.Append("OUTQTY = OUTQTY+ " + output + ",");
                update.Append("DEFECTQTY = DEFECTQTY+ " + NG + ",");
                update.Append("ARRIVEQTY = ARRIVEQTY + (" + NG + "+" + output + "),");
                if (datetimefrist == "")
                { update.Append("ARRIVETIME = '" + datetime + "',"); }
                update.Append("OUTTIME ='" + datetime + "',");
                update.Append("OR019 =OR019 +" + NG + ",");
                update.Append("UNIT = 'PCS',");
                update.Append("OR012 = OR012 -" + NG + ",");  //a Long them vao
                update.Append("OR020 = OR020 - (" + NG + "+" + output + ")");
                update.Append("where ID = '" + QR[0] + "' and OPID = 'B01---B01' and SEQUENCE  = 0");
                data.sqlExecuteNonQuery(update.ToString(), false);

                //update toSFT_OP_REALRUN B02
                StringBuilder UpdateB02 = new StringBuilder();
                UpdateB02.Append("update SFT_OP_REALRUN set OR017 =  OR017 -" + NG + ",");
                UpdateB02.Append("OR012 =OR012- " + NG + ","); //a Long them vao
                UpdateB02.Append("OR020 = OR020 +  " + output + " ");
                UpdateB02.Append("where ID = '" + QR[0] + "' and OPID = 'B02---B01' and SEQUENCE = 0");
                data.sqlExecuteNonQuery(UpdateB02.ToString(), false);//chi dieu se kiem tra lai co up hay ko

                //update into LOT table B01
                string updateLOTB01 = "update LOT set TYPE = 0, LOTSIZE =  LOTSIZE -(" + output + "+" + NG + "),ISPLANNED = 0  where ID = '" + QR[0] + "' and ERP_OPSEQ = '0010' and ERP_OPID = 'B01' and STATUS !='50'";
                data.sqlExecuteNonQuery(updateLOTB01, false);


                //update to MODETAIL
                StringBuilder updateMODETAIL = new StringBuilder();
                updateMODETAIL.Append("update MODETAIL  set MO019 = MO019 - " + NG + ",TOTALEXCEPTQTY  = TOTALEXCEPTQTY - " + NG + ",LASTMAINTAINDATETIME = '" + datetime + "' where CMOID = '" + QR[0] + "'");
                data.sqlExecuteNonQuery(updateMODETAIL.ToString(), false);

            }
            catch (Exception ex)
            {

                SystemLog.Output(SystemLog.MSG_TYPE.Err, " UpdatedataToSFT(string barcode)", ex.Message);
            }

        }
        public void InsertdataToSFT(string barcode, string output, string NG, string date, string time)
        {
            //  TOID = "D201-190xxxxx"; //chay thu nghiem
            try
            {


                string[] QR = barcode.Split(';');
                string[] ML = QR[0].Split('-');

                //  string datetime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                string datetime = date + " " + time;
                SQLSFTTLVN2 data = new SQLSFTTLVN2();
                string str_OUTQTY = data.sqlExecuteScalarString("select sum(OUTQTY) as OUTQTY from SFT_OP_REALRUN where ID = '" + QR[0] + "' and OPID = 'B01---B01' and SEQUENCE not like 0");
                double OUTQTY = 0;
                if (str_OUTQTY == "" || str_OUTQTY == null)
                    OUTQTY = 0;
                else OUTQTY = double.Parse(str_OUTQTY);
                LOTSIZE_B02 = OUTQTY + int.Parse(output);
                int SEQUENCE = int.Parse(data.sqlExecuteScalarString("select max(SEQUENCE) from SFT_OP_REALRUN where ID = '" + QR[0] + "'")) + 1;
                //An them vao de lam bao phe
                Sequence_OP_REAL_RUN = SEQUENCE;
                /////////////////////
                int SEQUENCE_SFT_WS_RUN = int.Parse(data.sqlExecuteScalarString("select max(SEQUENCE) from SFT_WS_RUN where ID = '" + QR[0] + "'" )) + 1;
                int SEQUENCE_SFT_WS_RUN_OUT = SEQUENCE_SFT_WS_RUN + 1;
                int Total = int.Parse(output) + int.Parse(NG);
                // string date = DateTime.Now.ToString("yyyyMMdd");
                // string month = DateTime.Now.ToString("yyyyMM");
                string month = date.Substring(0, 6);


                //insert  into SFT_OP_REALRUN
                StringBuilder sqlInsertSFT_OP_REALRUN = new StringBuilder();
                sqlInsertSFT_OP_REALRUN.Append(@"insert into SFT_OP_REALRUN ");
                sqlInsertSFT_OP_REALRUN.Append(@"(ID,SEQUENCE,OPID,OUTQTY,OUTTIME,ALERADYDEFECTQTY,"); //1
                sqlInsertSFT_OP_REALRUN.Append(" DEFECTQTY,OPERID,OUTUNIT,ERP_OPSEQ,MANWORKTIME,COINSTYPE,");//2
                sqlInsertSFT_OP_REALRUN.Append("TOID,ERP_OPID,ERP_WSID,OR002,OR003,OR004,OR013,OR014,OR019,TOSN,");//3
                sqlInsertSFT_OP_REALRUN.Append("STARTQTY,ENDQTY,REWORKQTY,FINISHRATE,OPDESCRIPTION,QTYPER,");//4
                sqlInsertSFT_OP_REALRUN.Append("OUTQTYPER,COMPLEXION,TA007,TA034,TA046,TA047,TA048,TA049,");//5
                sqlInsertSFT_OP_REALRUN.Append("TB068,TB070,TB087,OR009,PKUNIT,OR018,OR031,OR036,OR037,OR045,OR046,OR047,OR044,");//6
                sqlInsertSFT_OP_REALRUN.Append("OR048,OR049,OR050,OR051,OR052,OR055,TB053,TB069,");//7
                sqlInsertSFT_OP_REALRUN.Append("TA050,TA051,PRODUCTION_REPORTID,PRODUCTION_REPORTSN,TB059,TB060,TB061,TB062,TB063,TB067)");
                sqlInsertSFT_OP_REALRUN.Append(" values ( ");

                sqlInsertSFT_OP_REALRUN.Append("'" + QR[0] + "'," + SEQUENCE + ",'B01---B01'," + output + ",'" + datetime + "'," + NG + ",");//1
                sqlInsertSFT_OP_REALRUN.Append(NG + ",'BQC01','PCS','0010',500,'VND',");//2
                sqlInsertSFT_OP_REALRUN.Append("'" + TOID + "','B01','B01','0020','B02','B01'," + SEQUENCE_SFT_WS_RUN_OUT + ",'BQC01'," + NG + ",'0001' ,");//3
                sqlInsertSFT_OP_REALRUN.Append("0, 0,0,0,'',0,");//4
                sqlInsertSFT_OP_REALRUN.Append("1,1,NULL,NULL,'',0,0,NULL,");//5
                sqlInsertSFT_OP_REALRUN.Append("0,0,'',0,'',0,1,0,0,'',0,'',0,");//6
                sqlInsertSFT_OP_REALRUN.Append("0,0,'','',0,'N',0,'', ");//7
                sqlInsertSFT_OP_REALRUN.Append("NULL, NULL,'','','',0,'N', 'N', '1', 'N'");
                sqlInsertSFT_OP_REALRUN.Append(")");
                data.sqlExecuteNonQuery(sqlInsertSFT_OP_REALRUN.ToString(), false);

                //check from LOt table
                string sqlcheckLOT = "select count(*) from LOT where ID = '" + QR[0] + "'";
                if (int.Parse(data.sqlExecuteScalarString(sqlcheckLOT)) == 1)

                {

                    //insert into LOT table B02
                    StringBuilder sqlinsertSFT_LoT = new StringBuilder();
                    sqlinsertSFT_LoT.Append(@"insert into LOT ");
                    sqlinsertSFT_LoT.Append(" ( ID,TYPE,MOID,ITEMID,LOTSIZE,STATUS,ISPLANNED,UNIT,");
                    sqlinsertSFT_LoT.Append("QTYPER,ERP_OPSEQ,ERP_OPID,ERP_WSID,LOT004,LOT005,");
                    sqlinsertSFT_LoT.Append("RELEASEDATETIME,DUEDATETIME,PRIORITY,ROUTESEQ,STEPSEQ,ALTID,OPERATIONSEQ,EQUIPMENTID,OPERATEDTIME,REMAINTIME,MFGFLAG,");
                    sqlinsertSFT_LoT.Append("QTIME1,QTIME2,QTIME3,QTIME4,QTIME5,QTIME6,QTIME7,QTIME8, ISBANKORDER,RECIPEID, ");
                    sqlinsertSFT_LoT.Append("OUTPUTQTY,REMAININGTIME,LOTGROUP,PARTSISSUEDSTATUS,KEYVALUE,LOCKVALUE,PASSVALUE,PKUNIT)");
                    sqlinsertSFT_LoT.Append(" values ( ");
                    sqlinsertSFT_LoT.Append("'" + QR[0] + "',0,'" + QR[0] + "','" + ITEMID_TC047 + "'," + LOTSIZE_B02 + ",0,1,'PCS',");
                    sqlinsertSFT_LoT.Append("1,'0020','B02','B01',1,1,");
                    sqlinsertSFT_LoT.Append("NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,");
                    sqlinsertSFT_LoT.Append("NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,");
                    sqlinsertSFT_LoT.Append("NULL,NULL,NULL,NULL,NULL,NULL,NULL,''");
                    sqlinsertSFT_LoT.Append(")");
                    data.sqlExecuteNonQuery(sqlinsertSFT_LoT.ToString(),false);

                    //INSERT INTO LOT TABLE STATUS 50
                    StringBuilder sqlinsertSFT_LOT_50 = new StringBuilder();
                    sqlinsertSFT_LOT_50.Append("insert into LOT ");
                    sqlinsertSFT_LOT_50.Append(@"(ID, TYPE, MOID, ITEMID, LOTSIZE, STATUS, ISSUPPLY, ISPLANNED, ");
                    sqlinsertSFT_LOT_50.Append("UNIT, PKQTYPER, MO_SEQUENCE, HEAD_OP_SEQ, SUBMITFLAG,");
                    sqlinsertSFT_LOT_50.Append("ERP_OPSEQ, ERP_OPID, ERP_WSID, LOT004, LOT005, LOT007, LOT011,");
                    sqlinsertSFT_LOT_50.Append("RELEASEDATETIME,DUEDATETIME,PRIORITY,ROUTESEQ,STEPSEQ,ALTID,");
                    sqlinsertSFT_LOT_50.Append("OPERATIONSEQ,EQUIPMENTID,OPERATEDTIME,REMAINTIME,MFGFLAG,");
                    sqlinsertSFT_LOT_50.Append("QTIME1,QTIME2,QTIME3,QTIME4,QTIME5,QTIME6,QTIME7,QTIME8,ISBANKORDER,RECIPEID,");
                    sqlinsertSFT_LOT_50.Append("OUTPUTQTY,REMAININGTIME,LOTGROUP,PARTSISSUEDSTATUS,KEYVALUE,LOCKVALUE,PASSVALUE,QTYPER,PKUNIT)");
                    sqlinsertSFT_LOT_50.Append(" values ( ");
                    sqlinsertSFT_LOT_50.Append("'" + QR[0] + "',0,'" + QR[0] + "','" + ITEMID_TC047 + "',0,50,1,1,");
                    sqlinsertSFT_LOT_50.Append("'PCS',0,0,0,0,'0010','B01','B01',1,1,0,'N',");
                    sqlinsertSFT_LOT_50.Append("NULL,NULL,NULL,NULL,NULL,NULL,");
                    sqlinsertSFT_LOT_50.Append("NULL,NULL,NULL,NULL,NULL,");
                    sqlinsertSFT_LOT_50.Append("NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,");
                    sqlinsertSFT_LOT_50.Append("NULL,NULL,NULL,NULL,NULL,NULL,NULL,1,''");
                    sqlinsertSFT_LOT_50.Append(")");
                    data.sqlExecuteNonQuery(sqlinsertSFT_LOT_50.ToString(), false);

                }
                else
                {
                    StringBuilder updateLOTB02 = new StringBuilder();
                    updateLOTB02.Append("update LOT set TYPE = 0, LOTSIZE =  LOTSIZE +" + output + "  where ID = '" + QR[0] + "' and ERP_OPSEQ = '0020' and ERP_OPID = 'B02' and STATUS !='50'");
                    data.sqlExecuteNonQuery(updateLOTB02.ToString(), false);

                }


                //insert into SFT_WS_RUN--Checkin

                StringBuilder sqlinsertSFT_WS_RUN_IN = new StringBuilder();
                sqlinsertSFT_WS_RUN_IN.Append(@"insert into SFT_WS_RUN ");
                sqlinsertSFT_WS_RUN_IN.Append("(WSID,OPID,ID,SEQUENCE,EXECUTETIME,EXECUTETYPE,EXECUTEQTY,USERID,");//1
                sqlinsertSFT_WS_RUN_IN.Append("QTYPERUNIT,UNIT,QTYPER,PLUSINDEX,ERP_OPSEQ,ERP_OPID,ERP_WSID,WR002,");//2
                sqlinsertSFT_WS_RUN_IN.Append("WR003,WR004,WR005,WR007,WR008,WR009,WR021,");//3
                sqlinsertSFT_WS_RUN_IN.Append(" WR026,CREATE_DATE,WR031,WR032,");//4
                sqlinsertSFT_WS_RUN_IN.Append("EQID,EXECUTENAME,UNITCOUNT,PKUNIT,WR006,WR012,WR013,WR016,WR033,WR035,WR001)");//5
                sqlinsertSFT_WS_RUN_IN.Append(" values(");
                sqlinsertSFT_WS_RUN_IN.Append("'B01','B01---B01','" + QR[0] + "'," + SEQUENCE_SFT_WS_RUN + ",'" + datetime + "','checkIn'," + Total + ",'BQC01',");//1
                sqlinsertSFT_WS_RUN_IN.Append("0,'',1,0,'0010','B01','B01','',");//2
                sqlinsertSFT_WS_RUN_IN.Append("''," + Total + "," + Total + ",'0020','B02','B01',NULL,"); //3
                sqlinsertSFT_WS_RUN_IN.Append("NULL,'" + datetime + "',NULL,NULL,");//4
                sqlinsertSFT_WS_RUN_IN.Append("'','','','',0,'','',0,NULL,'',-1");//5
                sqlinsertSFT_WS_RUN_IN.Append(")");
                data.sqlExecuteNonQuery(sqlinsertSFT_WS_RUN_IN.ToString(), false); //


                //insert into SFT_WS_RUN--CheckOUT
                StringBuilder sqlinsertSFT_WS_RUN_OUT = new StringBuilder();
                sqlinsertSFT_WS_RUN_OUT.Append(@"insert into SFT_WS_RUN ");
                sqlinsertSFT_WS_RUN_OUT.Append("(WSID,OPID,ID,SEQUENCE,EXECUTETIME,EXECUTETYPE,EXECUTEQTY,USERID,");//1
                sqlinsertSFT_WS_RUN_OUT.Append("QTYPERUNIT,QTYPER,PLUSINDEX,ERP_OPSEQ,ERP_OPID,ERP_WSID,WR002,");//2
                sqlinsertSFT_WS_RUN_OUT.Append("WR003,WR004,WR005,WR007,WR008,WR009,WR021,");//3
                sqlinsertSFT_WS_RUN_OUT.Append(" WR026,CREATE_DATE,WR030,WR031,WR032,WR033,");//4
                sqlinsertSFT_WS_RUN_OUT.Append("EQID,EXECUTENAME,UNITCOUNT,PKUNIT,UNIT,WR006,WR012,WR013,WR001,WR035,WR016)");//5
                sqlinsertSFT_WS_RUN_OUT.Append(" values(");
                sqlinsertSFT_WS_RUN_OUT.Append("'B01','B01---B01','" + QR[0] + "'," + SEQUENCE_SFT_WS_RUN_OUT + ",'" + datetime + "','checkOut'," + output + ",'BQC01',");//1
                sqlinsertSFT_WS_RUN_OUT.Append("0,1,0,'0010','B01','B01','" + TOID + "',");//2
                sqlinsertSFT_WS_RUN_OUT.Append("'0001'," + output + "," + output + ",'0020','B02','B01','BQC01',"); //3
                sqlinsertSFT_WS_RUN_OUT.Append("'" + datetime + "','" + datetime + "',1,0,0,'',");//4
                sqlinsertSFT_WS_RUN_OUT.Append("'','','','','PCS',0,'','',-1,'',0");//5
                sqlinsertSFT_WS_RUN_OUT.Append(")");
                data.sqlExecuteNonQuery(sqlinsertSFT_WS_RUN_OUT.ToString(), false);



                //insert into SFT_TRANSORDER <có  duplicate key in object 'dbo.SFT_TRANSORDER'.>
                StringBuilder sqlinsertSFT_TRANSORDER = new StringBuilder();
                sqlinsertSFT_TRANSORDER.Append(@"insert into SFT_TRANSORDER ");
                sqlinsertSFT_TRANSORDER.Append("(CREATER,CREATE_DATE,FLAG,TRANSTYPE,TRANSNO,TRANSDATE,OUTTYPE,OUTDEPID,");
                sqlinsertSFT_TRANSORDER.Append("OUTDEPNAME,INTYPE,INDEPID,INDEPNAME,FACTORYID,CONFIRMCODE,DOCUMENTDATE,");
                sqlinsertSFT_TRANSORDER.Append("INVOICECOUNT,TAXATIONTYPE,DISCOUNTDEVIDE,DECLARATIONDATE,SALESTAXRATE,");
                sqlinsertSFT_TRANSORDER.Append("COMPANYID,KEYID,STOCKINTYPE,TO001,TO007,TO008,TO011,TO012,COINSTYPE,CONFIRMER,TO013,");
                sqlinsertSFT_TRANSORDER.Append("COMMENT,VENDORNO,INVOICEDATE,INVOICENO,MANUFACTURERORDERNO,TAXCODE,CREATOR,TO002,TO003,TO004,TO005,TO006,TO009,TO010)");
                sqlinsertSFT_TRANSORDER.Append(" values(");
                sqlinsertSFT_TRANSORDER.Append("'BQC01','" + datetime + "',0,'D201','" + TRANSNO + "','" + date + "','1','B01',");
                sqlinsertSFT_TRANSORDER.Append("'M+H生产线ONGM+H','1','B01','M+H生产线ONGM+H','TL','Y','" + date + "',");
                sqlinsertSFT_TRANSORDER.Append("'1','1','1','" + month + "','0.2',");
                sqlinsertSFT_TRANSORDER.Append("'TECHLINK','" + QR[0] + "','0','1','D201','" + TO008 + "',0,0,'VND','BQC01',1,");
                sqlinsertSFT_TRANSORDER.Append("'','','','','','','','','','','','','',''");
                sqlinsertSFT_TRANSORDER.Append(")");
                data.sqlExecuteNonQuery(sqlinsertSFT_TRANSORDER.ToString(), false);


                //insert into SFT_TRANSORDER_LINE 

                int PRODUCTIONSEQ_SFT_TRANSORDER_LINE = int.Parse(data.sqlExecuteScalarString("select max(PRODUCTIONSEQ) from SFT_TRANSORDER_LINE where KEYID = '" + QR[0] + "'"));
                if (PRODUCTIONSEQ_SFT_TRANSORDER_LINE == -1) PRODUCTIONSEQ_SFT_TRANSORDER_LINE = 0;
                PRODUCTIONSEQ_SFT_TRANSORDER_LINE++;
                StringBuilder sqlinsertSFT_TRANSORDER_LINE = new StringBuilder();
                sqlinsertSFT_TRANSORDER_LINE.Append(@"insert into SFT_TRANSORDER_LINE ");
                sqlinsertSFT_TRANSORDER_LINE.Append("(CREATE_DATE,TRANSORDERTYPE,TRANSNO,SN,MOTYPE,MONO,OUTOPSEQ,");//1
                sqlinsertSFT_TRANSORDER_LINE.Append("OUTOP,INOPSEQ,INOP,UNIT,PATTERN,SCRAPQTY,LABORHOUR,MACHINEHOUR,");//2
                sqlinsertSFT_TRANSORDER_LINE.Append("OUTDEP,EMERGENCY,TRANSQTY,INDEP,ITEMID,ITEMNAME,");//3
                sqlinsertSFT_TRANSORDER_LINE.Append("TC015,TC017,TC018,TC019,KEYID,PRODUCTIONSEQ,TL002,TL003,TL004,");//4
                sqlinsertSFT_TRANSORDER_LINE.Append("TL005,TL006,SFTUPDATE,TC055,TL007,TL008,TL010,TL011,TL012,TL015,");//5
                sqlinsertSFT_TRANSORDER_LINE.Append("TL016,SPC,TWINUNIT,KEY_TRANSORDER,FACTORYID,INWSTYPE,OUTWSTYPE,");//6
                sqlinsertSFT_TRANSORDER_LINE.Append("TL017,TL018,TL023,TL024,TL025,TL029,TL027,");//7
                sqlinsertSFT_TRANSORDER_LINE.Append("OUTORDERTYPE,OUTORDERNO,OUTORDERSEQ,LOTNO,ITEMDESCRIPTION,INSTORAGESPACE,TC012,TC024,TC040,NOTE, TL001,TC039,TC057,TL009,TL014,TL026)");
                sqlinsertSFT_TRANSORDER_LINE.Append(" values(");
                sqlinsertSFT_TRANSORDER_LINE.Append("'" + datetime + "','D201','" + TRANSNO + "','0001','" + ML[0] + "','" + ML[1] + "','0010',");//1
                sqlinsertSFT_TRANSORDER_LINE.Append("'B01','0020','B02','PCS','1'," + NG + ",0,0,");//2
                sqlinsertSFT_TRANSORDER_LINE.Append("'B01','N'," + Total + ",'B01','" + ITEMID_TC047 + "','" + ITEMNAME_TC048 + "',");//3
                sqlinsertSFT_TRANSORDER_LINE.Append("0,0,0,'0','" + QR[0] + "'," + PRODUCTIONSEQ_SFT_TRANSORDER_LINE + "," + output + ",0,0,");//4
                sqlinsertSFT_TRANSORDER_LINE.Append("0,'0',0,'0',0,0,'" + date + "','D201','" + TO008 + "'," + SEQUENCE_SFT_WS_RUN_OUT + ","); //5
                sqlinsertSFT_TRANSORDER_LINE.Append("0,'N','Y',1,'TL','1','1',");//6
                sqlinsertSFT_TRANSORDER_LINE.Append("'0',1,0,0,0,'N',0,");//7
                sqlinsertSFT_TRANSORDER_LINE.Append("'','','','','" + ITEMDESCRIPTION + "','','','','','','','','','','',''");
                sqlinsertSFT_TRANSORDER_LINE.Append(")");
                data.sqlExecuteNonQuery(sqlinsertSFT_TRANSORDER_LINE.ToString(), false);

            }
            catch (Exception ex)
            {

                SystemLog.Output(SystemLog.MSG_TYPE.Err, " InsertdataToSFT(string barcode, string output, string NG)", ex.Message);
            }
        }

    }
}
