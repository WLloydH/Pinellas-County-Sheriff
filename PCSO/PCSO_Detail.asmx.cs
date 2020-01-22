using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Services;

namespace PCSO
{
    [WebService(Namespace = "http://www.myclearwater.com/")]
    [ToolboxItem(false)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PCSO_Detail : WebService
    {
        private static ArrayList __ENCList = new ArrayList();
        private DataSet ds;

        [DebuggerNonUserCode]
        static PCSO_Detail()
        {
        }

        public PCSO_Detail()
        {
            lock (PCSO_Detail.__ENCList)
                PCSO_Detail.__ENCList.Add((object)new WeakReference((object)this));
            this.ds = new DataSet();
        }

        [WebMethod]
        public DataSet GetCallInfo(string strPwd)
        {
            if (String.Compare(strPwd, "pcso", false) != 0)
            {
                return (DataSet)null;
            }
            try
            {
                this.GetData("CLW_PCSO_RMI", "Response_Master_Incident");
                this.GetData("CLW_PCSO_RVA", "Response_Vehicles_Assigned");
                this.GetData("CLW_PCSO_RVP", "Response_Vehicles_Personnel");
                this.GetData("CLW_PCSO_RC", "Response_Comments");
            }
            catch (Exception ex)
            {
               this.ds = (DataSet)null;
             }
            return this.ds;
        }

        private void GetData(string strProcName, string strTableName)
        {
            DataTable table = new DataTable(strTableName);
            using (SqlConnection connection = new SqlConnection("Data Source=CPDPIE01RPT01;Initial Catalog=System;Integrated Security=True"))
            {
                using (SqlCommand sqlCommand1 = new SqlCommand(strProcName, connection))
                {
                    SqlCommand sqlCommand2 = sqlCommand1;
                    sqlCommand2.Connection.Open();
                    sqlCommand2.CommandType = CommandType.StoredProcedure;
                    table.Load((IDataReader)sqlCommand2.ExecuteReader(CommandBehavior.CloseConnection));
                    this.ds.Tables.Add(table);
                }
            }
        }
    }
}
