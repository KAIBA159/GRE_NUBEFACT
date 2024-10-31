using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_GRENUBEFACT
{
    class Common
    {
        static string Conexion_bd = ConfigurationManager.ConnectionStrings["db_gre"].ConnectionString;

        private const string SP_LIST_PENDIENTE_ENVIO_GRE = "SP_LIST_PENDIENTE_ENVIO_GRE";
        private const string SP_INSERT_RESPUESTA = "SP_INSERT_RESPUESTA";
        private const string SP_LIST_PENDIENTE_ENVIO_GRE_CABECERA = "SP_LIST_PENDIENTE_ENVIO_GRE_CABECERA";
        private const string SP_LIST_PENDIENTE_ENVIO_GRE_DETALLE = "SP_LIST_PENDIENTE_ENVIO_GRE_DETALLE";


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

        public static DataTable getCabeceraDocument(string compania, string serieNumero, string guiaNumero)
        {
            using (DataTable dt = new DataTable())
            {
                using (SqlConnection con = new SqlConnection(Conexion_bd))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_LIST_PENDIENTE_ENVIO_GRE_CABECERA))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@compania", SqlDbType.NVarChar).Value = compania;
                        cmd.Parameters.AddWithValue("@serieNumero", SqlDbType.NVarChar).Value = serieNumero;
                        cmd.Parameters.AddWithValue("@guiaNumero", SqlDbType.NVarChar).Value = guiaNumero;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {

                            sda.Fill(dt);
                        }
                    }
                }
                return dt;
            }
        }

        public static DataTable getDetalleDocument(string compania, string serieNumero, string guiaNumero)
        //public static DataTable getCabeceraDocument()
        {
            using (DataTable dt = new DataTable())
            {
                using (SqlConnection con = new SqlConnection(Conexion_bd))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_LIST_PENDIENTE_ENVIO_GRE_DETALLE))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@compania", SqlDbType.NVarChar).Value = compania;
                        cmd.Parameters.AddWithValue("@serieNumero", SqlDbType.NVarChar).Value = serieNumero;
                        cmd.Parameters.AddWithValue("@guiaNumero", SqlDbType.NVarChar).Value = guiaNumero;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {

                            sda.Fill(dt);
                        }
                    }
                }
                return dt;
            }
        }

        public static int insertResponse(string compania, string serie, string correlativo, int codigo, string mensajerespuesta)
        {
            int filasAfectadas = 0;  // Variable para almacenar el número de filas afectadas

            using (SqlConnection con = new SqlConnection(Conexion_bd))
            {
                using (SqlCommand cmd = new SqlCommand(SP_INSERT_RESPUESTA, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Añadir los parámetros
                    cmd.Parameters.AddWithValue("@v_compania", compania);
                    cmd.Parameters.AddWithValue("@v_serie", serie);
                    cmd.Parameters.AddWithValue("@v_correlativo", correlativo);
                    cmd.Parameters.AddWithValue("@v_codigo", codigo);
                    cmd.Parameters.AddWithValue("@v_mensajerespuesta", mensajerespuesta);

                    // Parámetro para capturar el valor de retorno (filas afectadas)
                    SqlParameter returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    // Abrir conexión
                    con.Open();

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    // Obtener el valor de retorno (número de filas afectadas)
                    filasAfectadas = (int)returnParameter.Value;
                }
            }


            return filasAfectadas;  // Retorna el número de filas afectadas
        }
    }
}
