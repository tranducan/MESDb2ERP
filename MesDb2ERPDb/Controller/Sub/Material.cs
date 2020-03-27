
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MesDb2ERPDb
{
    class Material
    {
        public bool KiemtraNguyenVatLieu(string code, string No,double SLUpload, out bool IsDuSoLuong, out bool isDunguyenvanLieu, out List<MaterialAdapt> materials, out List<string> Messages) //code :P511 , No : 1900010
        {
            bool _NVL = false;
            List<string> listMessasge = new List<string>();
            List<NVLTheoLSX> _listNVL = ListNVL(code, No);
            List<MaterialAdapt> materialAdapts = new List<MaterialAdapt>();
            List<LSX_SFTTA> _listSFTTA = ListSFTTA(code, No);
            if (_listNVL.Count ==0)
            {
                IsDuSoLuong = false;
                isDunguyenvanLieu = false;
                materials = null;
                listMessasge.Add("Don't find Material for Production");
                Messages = listMessasge;
                return false;
            }
           

            bool _SL = false;

            if(_listSFTTA.Count==1)
            {
                if (_listSFTTA[0].SLKeHoach_TA010 >= _listSFTTA[0].SLOutput_TA011 + _listSFTTA[0].SLBaoPhe_TA012)
                {
                    _SL = false;
                }
                else
                {
                    _SL = true;
                }
                foreach (var item in _listNVL)
                {
                    MaterialAdapt adapt = new MaterialAdapt();
                    adapt.tenVatLieu = item.NVL_TB003;
                    if (item.NVLCan_TB004 != 0)
                    {
                        adapt.SL_DapUng = (int)(_listSFTTA[0].SLKeHoach_TA010 * (item.NVLLanh_TB005 / item.NVLCan_TB004));
                        adapt.SL_Thieu = (int)(_listSFTTA[0].SLKeHoach_TA010 - adapt.SL_DapUng);
                    }
                    else
                    {
                        adapt.SL_DapUng = _listSFTTA[0].SLKeHoach_TA010;
                        adapt.SL_Thieu = 0;
                    }
                    materialAdapts.Add(adapt);
            }
               double SLCoTheDapUngDuoc = materialAdapts.Select(d => d.SL_DapUng).ToArray().Min();
                if(_listSFTTA[0].SLOutput_TA011+ _listSFTTA[0].SLBaoPhe_TA012 + SLUpload < SLCoTheDapUngDuoc)
                {
                    _NVL = true;
                }
                else _NVL = false;
            }
            else if(_listSFTTA.Count ==0)
            {
                listMessasge.Add("Don't have production plan in SFTTA Table");
            }
            else if (_listSFTTA.Count  > 1)
            {
                listMessasge.Add("Too many production plan in SFTTA Table");
            }

            IsDuSoLuong = _SL;
            isDunguyenvanLieu = _NVL;
            materials = materialAdapts;
            Messages = listMessasge;

            return (!IsDuSoLuong & isDunguyenvanLieu);
        }
        public List<NVLTheoLSX> ListNVL (string code, string No)
        {

            List<NVLTheoLSX> _listNVL = new List<NVLTheoLSX>();
           sqlERPCON  query= new sqlERPCON();
            StringBuilder strSQL = new StringBuilder();
            DataTable dt = new DataTable();
            strSQL.Append("select TB001,TB002, TB003,TB004,TB005,TB006 from MOCTB where TB006 ='****' and TB018 ='Y' and ");
            strSQL.Append(" TB001 = '" + code + "' and ");
            strSQL.Append(" TB002 = '" + No + "'");
            query.sqlDataAdapterFillDatatable(strSQL.ToString(), ref dt );
            //Load data into list
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NVLTheoLSX nVL = new NVLTheoLSX();
               
                nVL.code = dt.Rows[i]["TB001"].ToString();
                nVL.No = dt.Rows[i]["TB002"].ToString();
                nVL.NVL_TB003 = dt.Rows[i]["TB003"].ToString();
                nVL.NVLCan_TB004 = dt.Rows[i]["TB004"].ToString() != "" ? double.Parse(dt.Rows[i]["TB004"].ToString()):0;
                nVL.NVLLanh_TB005 = dt.Rows[i]["TB005"].ToString() != "" ? double.Parse(dt.Rows[i]["TB005"].ToString()) : 0;
                nVL.NVLPercentLanh = (nVL.NVLCan_TB004 != 0) ? (nVL.NVLLanh_TB005 / nVL.NVLCan_TB004) :((nVL.NVLCan_TB004 == nVL.NVLLanh_TB005)?1:0);
                nVL.CD_TB006 = dt.Rows[i]["TB006"].ToString();
                _listNVL.Add(nVL);

            }
         
               
            return _listNVL;
        }
        public List<LSX_SFTTA> ListSFTTA (string code, string No)
        {
            List<LSX_SFTTA> lSX_SFTTAs = new List<LSX_SFTTA>();
            sqlERPCON query = new sqlERPCON();
            StringBuilder strSQL = new StringBuilder();
            DataTable dt = new DataTable();
            strSQL.Append("select TA001,TA002,TA003,TA004,TA008,TA009,TA010,TA011,TA012 from SFCTA where TA003 = '0010' and  ");
            strSQL.Append(" TA001 = '" + code + "' and ");
            strSQL.Append(" TA002 = '" + No + "'");
            query.sqlDataAdapterFillDatatable(strSQL.ToString(), ref dt);
            //Load data into list
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LSX_SFTTA sFTTA = new LSX_SFTTA();
                sFTTA.code = dt.Rows[i]["TA001"].ToString();
                sFTTA.No = dt.Rows[i]["TA002"].ToString();
                sFTTA.MaSX_TA003 = dt.Rows[i]["TA003"].ToString();
                sFTTA.MaSX_TA004 = dt.Rows[i]["TA004"].ToString();
                sFTTA.NgayBatdau_TA008 = dt.Rows[i]["TA008"].ToString();
                sFTTA.NgayKetThuc_TA009 = dt.Rows[i]["TA009"].ToString();
                sFTTA.SLKeHoach_TA010 = dt.Rows[i]["TA010"].ToString() != "" ? double.Parse(dt.Rows[i]["TA010"].ToString()) : 0;
                sFTTA.SLOutput_TA011 = dt.Rows[i]["TA011"].ToString() != "" ? double.Parse(dt.Rows[i]["TA011"].ToString()) : 0;
                sFTTA.SLBaoPhe_TA012 = dt.Rows[i]["TA012"].ToString() != "" ? double.Parse(dt.Rows[i]["TA012"].ToString()) : 0;
                lSX_SFTTAs.Add(sFTTA);

            }
            return lSX_SFTTAs;
        }
    }
    public class NVLTheoLSX
    {
        public string code { get; set; }
        public string No { get; set; }
        public string NVL_TB003 { get; set; }
        public double NVLCan_TB004 { get; set; }
        public double NVLLanh_TB005 { get; set; }
        public double NVLPercentLanh { get; set; }
        public string CD_TB006 { get; set; }

    }
    public class LSX_SFTTA
    {
        public string code { get; set; }
        public string No { get; set; }
        public string MaSX_TA003 { get; set; }
        public string MaSX_TA004 { get; set; }
        public string NgayBatdau_TA008 { get; set; }
        public string NgayKetThuc_TA009 { get; set; }
        public double SLKeHoach_TA010 { get; set; }
        public double SLOutput_TA011 { get; set; }
        public double SLBaoPhe_TA012 { get; set; }

    }
    public class MaterialAdapt
    {
        public string tenVatLieu { get; set; }
        public double SL_DapUng { get; set; }
        public double SL_Thieu { get; set; }

    }
}
