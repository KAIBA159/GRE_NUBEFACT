using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Timers;
using RestSharp;

namespace WS_GRENUBEFACT
{
    public partial class Service1 : ServiceBase
    {

        string Conexion_bd = ConfigurationManager.ConnectionStrings["db_gre"].ConnectionString;

        string Apiweb = ConfigurationManager.ConnectionStrings["apiweb"].ConnectionString;
        string Apiweb_sin = ConfigurationManager.ConnectionStrings["apiweb_sin"].ConnectionString;
        string Token = ConfigurationManager.ConnectionStrings["token"].ConnectionString;

        public int time_event = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeService"]);  //RUTA DEL RUC

        public Timer tiempo;

        public Service1()
        {
            InitializeComponent();

            tiempo = new Timer();
            //tiempo.Interval = 8000;
            tiempo.Interval = time_event;

            tiempo.Elapsed += new ElapsedEventHandler(procesar);
            tiempo.AutoReset = true;


        }

        protected override void OnStart(string[] args)
        {
            tiempo.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        public void procesar(object sender, EventArgs e)
        {
            GRE();
        }

        private void GRE()
        {
            string compania = string.Empty;
            string SerieNumero = string.Empty;
            string GuiaNumero = string.Empty;
            int codigorespuesta = 0;
            string mensajerespuesta = string.Empty;
            int filasAfectadas = 0;
            string itemsdet = "";

            try
            {
                DataTable dtc = Common.getDocumentoByIndicator();
                if (dtc.Rows.Count == 0) { return; }

                for (int i = 0; i <= dtc.Rows.Count - 1; i++)
                {


                    var options = new RestClientOptions(Apiweb)
                    {
                        MaxTimeout = -1,
                    };


                    var client = new RestClient(options);
                    var request = new RestRequest(Apiweb_sin, Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", Token);

                    var body = "";

                    //string aux_docsubtype = "";
                    //string tipo_de_subtota = "IGV";
                    //string Descuento_global = "0.0";
                    //string count_fact_referenciada = string.Empty;

                    ////FacturaBE factura = new FacturaBE();
                    //string DocEntry_aux_1 = string.Empty;
                    //string ObjType_aux_1 = string.Empty;

                    /**/

                    //DocumentoElectronico oDocumento = new DocumentoElectronico();

                    compania = dtc.Rows[i]["CompaniaSocio"].ToString();
                    SerieNumero = dtc.Rows[i]["SerieNumero"].ToString();
                    GuiaNumero = dtc.Rows[i]["GuiaNumero"].ToString();


                    //LISTAR DETALLE

                    DataTable lista_detalle = Common.getDetalleDocument(compania, SerieNumero, GuiaNumero);

                    itemsdet = string.Empty;

                    for (int j = 0; j < lista_detalle.Rows.Count; j++)
                    {
                        itemsdet += @"{
                        " + "\n" +
                                            @"	""unidad_de_medida"": """ + lista_detalle.Rows[j]["unidad_de_medida"].ToString().Trim() + @""",
                        " + "\n" +
                                            @"	""codigo"": """ + lista_detalle.Rows[j]["codigo"].ToString().Trim() + @""",
                        " + "\n" +
                                            @"	""descripcion"": """ + lista_detalle.Rows[j]["descripcion"].ToString().Trim() + @""",
                        " + "\n" +
                                            @"	""cantidad"": """ + lista_detalle.Rows[j]["cantidad"].ToString().Trim() + @"""
                        " + "\n" +
                                            @"}";

                        // Añadir una coma entre los objetos, excepto después del último elemento
                        if (i < lista_detalle.Rows.Count - 1)
                        {
                            itemsdet += ",";
                        }
                        itemsdet += "\n";
                    }

                    //LISTAR CABECERA



                    DataTable lista_cabecera = Common.getCabeceraDocument(compania, SerieNumero, GuiaNumero);


                    body = string.Empty;

                    for (int p = 0; p <= lista_cabecera.Rows.Count - 1; p++)
                    {




                        body = @"{
                       
                      " + "\n" +
                             @"	""operacion"": """ + lista_cabecera.Rows[p]["operacion"].ToString().Trim() + @""",
                        " + "\n" +
                             @"	""tipo_de_comprobante"": 7,
                        " + "\n" +
                             @"	""serie"": """ + lista_cabecera.Rows[p]["serie"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""numero"": """ + lista_cabecera.Rows[p]["numero"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""cliente_tipo_de_documento"": """ + lista_cabecera.Rows[p]["cliente_tipo_de_documento"].ToString().Trim() + @""",
                        
                        " + "\n" +
                             @"	""cliente_numero_de_documento"": """ + lista_cabecera.Rows[p]["cliente_numero_de_documento"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""cliente_denominacion"": """ + lista_cabecera.Rows[p]["cliente_denominacion"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""cliente_direccion"": """ + lista_cabecera.Rows[p]["cliente_direccion"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""cliente_email"": """ + lista_cabecera.Rows[p]["cliente_email"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""fecha_de_emision"": """ + lista_cabecera.Rows[p]["fecha_de_emision"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""observaciones"": """ + lista_cabecera.Rows[p]["observaciones"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""motivo_de_traslado"": """ + lista_cabecera.Rows[p]["motivo_de_traslado"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""peso_bruto_total"": """ + lista_cabecera.Rows[p]["peso_bruto_total"].ToString().Trim() + @"""  ,                      

                        " + "\n" +
                             @"	""peso_bruto_unidad_de_medida"": """ + lista_cabecera.Rows[p]["peso_bruto_unidad_de_medida"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""numero_de_bultos"": """ + lista_cabecera.Rows[p]["numero_de_bultos"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""tipo_de_transporte"": """ + lista_cabecera.Rows[p]["tipo_de_transporte"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""fecha_de_inicio_de_traslado"": """ + lista_cabecera.Rows[p]["fecha_de_inicio_de_traslado"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""transportista_documento_tipo"": """ + lista_cabecera.Rows[p]["transportista_documento_tipo"].ToString().Trim() + @""" , 

                        " + "\n" +
                             @"	""transportista_documento_numero"": """ + lista_cabecera.Rows[p]["transportista_documento_numero"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""transportista_denominacion"": """ + lista_cabecera.Rows[p]["transportista_denominacion"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""transportista_placa_numero"": """ + lista_cabecera.Rows[p]["transportista_placa_numero"].ToString().Trim() + @""" ,
                        
                        " + "\n" +
                             @"	""transportista_placa_numero"": """ + lista_cabecera.Rows[p]["transportista_placa_numero"].ToString().Trim() + @""",


                        
                        


                        " + "\n" +
                             @"	""punto_de_partida_ubigeo"": """ + lista_cabecera.Rows[p]["punto_de_partida_ubigeo"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""punto_de_partida_direccion"": """ + lista_cabecera.Rows[p]["punto_de_partida_direccion"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""punto_de_partida_codigo_establecimiento_sunat"": """ + lista_cabecera.Rows[p]["punto_de_partida_codigo_establecimiento_sunat"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""punto_de_llegada_ubigeo"": """ + lista_cabecera.Rows[p]["punto_de_llegada_ubigeo"].ToString().Trim() + @""" ,

                        " + "\n" +
                             @"	""punto_de_llegada_direccion"": """ + lista_cabecera.Rows[p]["punto_de_llegada_direccion"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""punto_de_llegada_codigo_establecimiento_sunat"": """ + lista_cabecera.Rows[p]["punto_de_llegada_codigo_establecimiento_sunat"].ToString().Trim() + @""",

                        " + "\n" +
                             @"	""formato_de_pdf"": """",
                        
                        " + "\n" +

                        @"	""items"": [
                        " + "\n" +
                        itemsdet +
                        @"	],
                        " + "\n" +

                             @"	""vehiculos_secundarios"": [
                        " + "\n" +
                                    @"		{
                        " + "\n" +
                                    @"			""placa_numero"": """ + lista_cabecera.Rows[p]["placa_numero"].ToString().Trim() + @"""		},
                        " + "\n" +
                                    @"		{
                        " + "\n" +
                                    @"			""placa_numero"": """ + lista_cabecera.Rows[p]["placa_numero1"].ToString().Trim() + @"""		}
                        " + "\n" +
                                    @"	]
                        " + "\n" +
                        @"

                        " + "\n" +
                             @"}
                        " + "\n" +
                             @"
                        " + "\n" +
                             @"";



                    };

                    //LISTAR DETALLE


                    request.AddStringBody(body, DataFormat.Json);
                    //RestResponse response = await client.ExecuteAsync(request);
                    RestResponse response = client.Execute(request);

                    if (response.IsSuccessful)
                    {
                        // Deserializar el JSON en un objeto ApiResponse
                        var deserializedResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResp>(response.Content);


                        mensajerespuesta = string.Empty;
                        mensajerespuesta = (" Mensaje " + SerieNumero + "-" + GuiaNumero + "  " + deserializedResponse.nota_importante);
                        codigorespuesta = 1;


                        //ini_INSERT EN BD, respuesta
                        filasAfectadas = Common.insertResponse(compania, SerieNumero, GuiaNumero, codigorespuesta, mensajerespuesta);

                        // Verificar si se actualizaron filas
                        if (filasAfectadas > 0)
                        {
                            //un log agregar, si fuera necesario
                        }
                        else
                        {
                            //un log agregar, si fuera necesario

                        }


                        //INSERT EN BD, respuesta

                    }
                    else
                    {
                        // Manejar el error
                        //Console.WriteLine("Error en la solicitud: " + response.ErrorMessage);
                        var deserializedResponse = System.Text.Json.JsonSerializer.Deserialize<ErrorResponse>(response.Content);

                        // Acceder a los datos del error
                        if (deserializedResponse != null)
                        {
                            //Console.WriteLine("Error: " + deserializedResponse.errors);
                            //Console.WriteLine("Código: " + deserializedResponse.codigo);
                            mensajerespuesta = string.Empty;


                            if (deserializedResponse.codigo == 23)
                            {

                            }

                            switch (Convert.ToInt32(deserializedResponse.codigo))
                            {
                                case 23:
                                    mensajerespuesta = (" Codigo NubeFact : " + deserializedResponse.codigo + " -- " +
                                                    " Mensaje " + deserializedResponse.errors);
                                    codigorespuesta = 1;

                                    //ini_INSERT EN BD, respuesta
                                    filasAfectadas = Common.insertResponse(compania, SerieNumero, GuiaNumero, codigorespuesta, mensajerespuesta);

                                    break;

                                case 21:
                                    mensajerespuesta = (" Codigo NubeFact : " + deserializedResponse.codigo + " -- " +
                                                    " Mensaje " + deserializedResponse.errors);
                                    codigorespuesta = 2;

                                    //ini_INSERT EN BD, respuesta
                                    filasAfectadas = Common.insertResponse(compania, SerieNumero, GuiaNumero, codigorespuesta, mensajerespuesta);

                                    break;

                                case 12:

                                    break;

                                default:

                                    mensajerespuesta = (" Codigo NubeFact : " + deserializedResponse.codigo + " -- " +
                                                    " Mensaje " + deserializedResponse.errors);
                                    codigorespuesta = 2;

                                    //ini_INSERT EN BD, respuesta
                                    filasAfectadas = Common.insertResponse(compania, SerieNumero, GuiaNumero, codigorespuesta, mensajerespuesta);


                                    break;

                            }



                            // Verificar si se actualizaron filas
                            if (filasAfectadas > 0)
                            {
                                //un log agregar, si fuera necesario
                            }
                            else
                            {
                                //un log agregar, si fuera necesario

                            }
                            //fin_INSERT EN BD, respuesta
                        }
                    }

                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        public class ErrorResponse
        {
            public string errors { get; set; }
            public int codigo { get; set; }
        }

        public class ApiResp
        {
            public string nota_importante { get; set; }
            public int tipo_de_comprobante { get; set; }
            public string serie { get; set; }
            public int numero { get; set; }
            public string enlace { get; set; }
            public bool aceptada_por_sunat { get; set; }
            public string sunat_description { get; set; }
            public string sunat_note { get; set; }
            public string sunat_responsecode { get; set; }
            public string sunat_soap_error { get; set; }
            public string pdf_zip_base64 { get; set; }
            public string xml_zip_base64 { get; set; }
            public string cdr_zip_base64 { get; set; }
            public string cadena_para_codigo_qr { get; set; }
            public string enlace_del_pdf { get; set; }
            public string enlace_del_xml { get; set; }
            public string enlace_del_cdr { get; set; }
        }



    }
}
