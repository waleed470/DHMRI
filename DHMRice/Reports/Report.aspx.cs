using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DHMRice.SqlClasses;

namespace DHMRice.Reports
{
    public partial class Report : System.Web.UI.Page
    {
        ApplicationDbContext db = new ApplicationDbContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["value"] == "IndividualReport" && Request.QueryString["E_id"] != null)
            {

                TestingGatePassInward rpt = new TestingGatePassInward();
                int id = Convert.ToInt32(Request.QueryString["E_id"]);
                //ReportDal dal = new ReportDal();
                //rpt.SetDataSource(dal.GatePassInward(id));

                DataTable dt = new DataSet1.GatePassInwardDataTable();
                var temp = db.RarRices.Find(id);
                var Rice_cat = db.Rice_Categories.Where(r => r.Rice_category_Id == temp.Rice_category_Id).SingleOrDefault();
                var datagat = db.GatePassInwareds.Where(r => r.RawRice_id == temp.RawRice_id).SingleOrDefault();
                var expense = db.RawRiceExpense.Where(p => p.RawRice_id == temp.RawRice_id).ToList();
                if (expense == null)
                {
                    dt.Rows.Add(temp.Item_Name, temp.party.Party_Name, temp.party.Party_Code, temp.Item_Code, temp.Bags_qty, temp.Total_Weight, "", 0, datagat.Vehicle_No, datagat.Driver_Name, datagat.Bility_No, datagat.Purchased_By, datagat.Designation, temp.packing.Packing_Type, Rice_cat.Rice_Category_Name, temp.Total_Mann);

                }
                else
                {
                    foreach (var item in expense)
                    {
                        dt.Rows.Add(temp.Item_Name, temp.party.Party_Name, temp.party.Party_Code, temp.Item_Code, temp.Bags_qty, temp.Total_Weight, item.RawRiceExpense_Name, item.RawRiceExpense_Amount, datagat.Vehicle_No, datagat.Driver_Name, datagat.Bility_No, datagat.Purchased_By, datagat.Designation, temp.packing.Packing_Type, Rice_cat.Rice_Category_Name, temp.Total_Mann);
                    }
                }
               

             

                rpt.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpt;
            }
            if (Request.QueryString["value"] == "AllRice" )
            {
                decimal TotalExpense = 0;
                All_Rice rpt = new All_Rice();
                
              

                DataTable dt = new DataSet1.All_RiceDataTable();
                var RawRice = db.RarRices.ToList();

                //if (expense == null)
                //{
                //    dt.Rows.Add(temp.Item_Name, temp.party.Party_Name, temp.party.Party_Code, temp.Item_Code, temp.Bags_qty, temp.Total_Weight, "", 0, datagat.Vehicle_No, datagat.Driver_Name, datagat.Bility_No, datagat.Purchased_By, datagat.Designation, temp.packing.Packing_Type, Rice_cat.Rice_Category_Name, temp.Total_Mann);

                //}


                foreach (var item in RawRice)
                {
                    var pricing = db.Pricing.Where(r => r.item_id == item.RawRice_id && r.item_Type == "RawRice").SingleOrDefault();

                    var expense = db.RawRiceExpense.Where(p => p.RawRice_id == item.RawRice_id).ToList();
                    if (expense != null)
                    {
                        foreach (var exp in expense)
                        {
                            TotalExpense += exp.RawRiceExpense_Amount;
                        }
                    }

                    dt.Rows.Add(item.Item_Name, item.Item_Code, item.Bags_qty, item.Total_Weight, item.packing.Packing_Type, item.Total_Mann, item.Bags_Sold_qty, pricing.PerBagPrice, pricing.Pricing_Rate_Maan, pricing.Pricing_Total, pricing.Pricing_Date, pricing.Pricing_NetTotal, TotalExpense, item.packing.Packing_Type, item.broker.Broker_Name);
                }





                rpt.SetDataSource(RawRice);
                CrystalReportViewer1.ReportSource = rpt;
            }
        }
    }
}