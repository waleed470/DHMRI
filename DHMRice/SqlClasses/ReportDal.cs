using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DHMRice.SqlClasses
{
    public class ReportDal
    {
        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-EJ08DMO;Initial Catalog=DHMRice; Trusted_Connection=True;");
            
        private SqlCommand cmd = new SqlCommand();
        public DataTable GatePassInward(int Rawrice_Id)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                con.Open();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@RawRice_id", SqlDbType.Int).Value = Rawrice_Id;
                cmd.CommandText = "GatePassInward";
                cmd.ExecuteNonQuery();
                con.Close();
                da.SelectCommand = cmd;
                da.Fill(dt);
                return dt;
            }
            catch (SqlException e)
            {
               
            }
            return null;
        }
    }
}