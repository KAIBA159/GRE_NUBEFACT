using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRE_NUBEFACT
{
    class Common
    {

        static string Conexion_bd = ConfigurationManager.ConnectionStrings["db_gre"].ConnectionString;

        private const string SP_LIST_PENDIENTE_ENVIO_GRE = "SP_LIST_PENDIENTE_ENVIO_GRE";

        public static DataTable getDocumentoByIndicator()
        {
            using (DataTable dt = new DataTable())
            {
                using (SqlConnection con = new SqlConnection(Conexion_bd))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_LIST_PENDIENTE_ENVIO_GRE))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@vc_Indicator", SqlDbType.NVarChar).Value = indicator;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {

                            sda.Fill(dt);
                        }
                    }
                }
                return dt;
            }
        }

    }
}
