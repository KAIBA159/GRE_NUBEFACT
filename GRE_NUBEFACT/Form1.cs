using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http;
using static System.Windows.Forms.LinkLabel;
using System.IO;
using System.Security.Policy;
using System.Xml.Serialization;

namespace GRE_NUBEFACT
{
    public partial class Form1 : Form
    {

        string Conexion_bd = ConfigurationManager.ConnectionStrings["db_gre"].ConnectionString;

        string Apiweb = ConfigurationManager.ConnectionStrings["apiweb"].ConnectionString;
        string Apiweb_sin = ConfigurationManager.ConnectionStrings["apiweb_sin"].ConnectionString;
        string Token = ConfigurationManager.ConnectionStrings["token"].ConnectionString;

        	
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {

            var options = new RestClientOptions(Apiweb)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(Apiweb_sin, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", Token);
            var body = @"{
        " + "\n" +
                    @"	""operacion"": ""consultar_guia"",
        " + "\n" +
                    @"	""tipo_de_comprobante"": 7,
        " + "\n" +
                    @"	""serie"": """ + txtbox_serie.Text.ToString().Trim() + @""",
        " + "\n" +
                    @"	""numero"": """ + txtbox_correlativo.Text.ToString().Trim() + @"""
        " + "\n" +
                    @"}
        " + "\n" +
                    @"
        " + "\n" +
                    @"";
            request.AddStringBody(body, DataFormat.Json);
            //RestResponse response = await client.ExecuteAsync(request);
            RestResponse response = client.Execute(request);

            var comprobanteResponse = JsonSerializer.Deserialize<ComprobanteResponse>(response.Content);


            //Console.WriteLine(response.Content);

            if (comprobanteResponse.AceptadaPorSunat && (Descargar_Pdf(txtbox_serie.Text.ToString().Trim(), txtbox_correlativo.Text.ToString().Trim(),comprobanteResponse.EnlaceDelPdf.ToString())))
            {
                //existo
                MessageBox.Show("Descarga existosa ...", "Descarga PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //
                MessageBox.Show("Guia no encontrada o un error en la descarga PDF ... ", "Descarga PDF Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }


        static bool Descargar_Pdf(string serie, string correlativo,string args)
        {
            bool valor = false;
            
            string url = args; //"https://www.nubefact.com/guia/e004e591-d079-497f-a5b0-7ddd0b508fef.pdf";
            string rutaDestino = string.Empty;

            try
            {
                // Usar SaveFileDialog para seleccionar la ubicación donde guardar el archivo
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    saveFileDialog.Title = "Guardar archivo PDF";
                    saveFileDialog.FileName = "guia_nubefact_"+"09-"+ serie + "-" + correlativo + ".pdf"; // Nombre de archivo por defecto

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        rutaDestino = saveFileDialog.FileName; // Guardar la ruta seleccionada por el usuario
                    }
                }

                if (!string.IsNullOrEmpty(rutaDestino))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            Console.WriteLine("Iniciando la descarga...");

                            // Descargar el contenido de forma sincrónica
                            byte[] contenido = client.GetByteArrayAsync(url).Result;

                            // Guardar el contenido en un archivo local
                            File.WriteAllBytes(rutaDestino, contenido);

                            Console.WriteLine($"Archivo descargado y guardado en {rutaDestino}");
                        }
                        catch (Exception ex)
                        {
                            return valor = false;
                        }
                    }
                }
                else
                {

                    return valor = false;
                }

                valor = true;
            }
            catch (Exception ex)
            {
                return valor = false;
                
            }

            return valor;
        }

        private void descargarPDF_Click(object sender, EventArgs e)
        {

        }

        static bool Descargar_CDR(string serie, string correlativo, string args)
        {
            bool valor = false;

            string url = args; //"https://www.nubefact.com/guia/e004e591-d079-497f-a5b0-7ddd0b508fef.pdf";
            string rutaDestino = string.Empty;

            try
            {
                // Usar SaveFileDialog para seleccionar la ubicación donde guardar el archivo
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "CDR files (*.cdr)|*.cdr|All files (*.*)|*.*";
                    saveFileDialog.Title = "Guardar archivo CDR";
                    saveFileDialog.FileName = "guia_nubefact_" + "09-" + serie + "-" + correlativo + ".cdr"; // Nombre de archivo por defecto

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        rutaDestino = saveFileDialog.FileName; // Guardar la ruta seleccionada por el usuario
                    }
                }

                if (!string.IsNullOrEmpty(rutaDestino))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            Console.WriteLine("Iniciando la descarga...");

                            // Descargar el contenido de forma sincrónica
                            byte[] contenido = client.GetByteArrayAsync(url).Result;

                            // Guardar el contenido en un archivo local
                            File.WriteAllBytes(rutaDestino, contenido);

                            Console.WriteLine($"Archivo descargado y guardado en {rutaDestino}");
                        }
                        catch (Exception ex)
                        {
                            return valor = false;
                        }
                    }
                }
                else
                {

                    return valor = false;
                }

                valor = true;
            }
            catch (Exception ex)
            {
                return valor = false;

            }

            return valor;
        }

        private void descargarCDR_Click(object sender, EventArgs e)
        {
            var options = new RestClientOptions("https://api.nubefact.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/v1/92edb536-027e-439a-a002-21e807bf3c16", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "18b015ec8e304056bd89f8363bc92e0408798b08e9114f0f8966b2a9f98f88d3");
            var body = @"{
        " + "\n" +
                    @"	""operacion"": ""consultar_guia"",
        " + "\n" +
                    @"	""tipo_de_comprobante"": 7,
        " + "\n" +
                    @"	""serie"": """ + txtbox_serie.Text.ToString().Trim() + @""",
        " + "\n" +
                    @"	""numero"": """ + txtbox_correlativo.Text.ToString().Trim() + @"""
        " + "\n" +
                    @"}
        " + "\n" +
                    @"
        " + "\n" +
                    @"";
            request.AddStringBody(body, DataFormat.Json);
            //RestResponse response = await client.ExecuteAsync(request);
            RestResponse response = client.Execute(request);

            var comprobanteResponse = JsonSerializer.Deserialize<ComprobanteResponse>(response.Content);


            //Console.WriteLine(response.Content);

            if (comprobanteResponse.AceptadaPorSunat && (Descargar_CDR(txtbox_serie.Text.ToString().Trim(), txtbox_correlativo.Text.ToString().Trim(), comprobanteResponse.EnlaceDelCdr.ToString())))
            {
                //existo
                MessageBox.Show("Descarga existosa ...", "Descarga CDR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //
                MessageBox.Show("Guia no encontrada o un error en la descarga CDR ... ", "Descarga CDR Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void descargarXML_Click(object sender, EventArgs e)
        {
            var options = new RestClientOptions("https://api.nubefact.com")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/v1/92edb536-027e-439a-a002-21e807bf3c16", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "18b015ec8e304056bd89f8363bc92e0408798b08e9114f0f8966b2a9f98f88d3");
            var body = @"{
        " + "\n" +
                    @"	""operacion"": ""consultar_guia"",
        " + "\n" +
                    @"	""tipo_de_comprobante"": 7,
        " + "\n" +
                    @"	""serie"": """ + txtbox_serie.Text.ToString().Trim() + @""",
        " + "\n" +
                    @"	""numero"": """ + txtbox_correlativo.Text.ToString().Trim() + @"""
        " + "\n" +
                    @"}
        " + "\n" +
                    @"
        " + "\n" +
                    @"";
            request.AddStringBody(body, DataFormat.Json);
            //RestResponse response = await client.ExecuteAsync(request);
            RestResponse response = client.Execute(request);

            var comprobanteResponse = JsonSerializer.Deserialize<ComprobanteResponse>(response.Content);


            //Console.WriteLine(response.Content);

            if (comprobanteResponse.AceptadaPorSunat && (Descargar_XML(txtbox_serie.Text.ToString().Trim(), txtbox_correlativo.Text.ToString().Trim(), comprobanteResponse.EnlaceDelCdr.ToString())))
            {
                //existo
                MessageBox.Show("Descarga existosa ...", "Descarga XML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //
                MessageBox.Show("Guia no encontrada o un error en la descarga XML ... ", "Descarga XML Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static bool Descargar_XML(string serie, string correlativo, string args)
        {
            bool valor = false;

            string url = args; //"https://www.nubefact.com/guia/e004e591-d079-497f-a5b0-7ddd0b508fef.pdf";
            string rutaDestino = string.Empty;

            try
            {
                // Usar SaveFileDialog para seleccionar la ubicación donde guardar el archivo
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "XML files (*.cdr)|*.xml|All files (*.*)|*.*";
                    saveFileDialog.Title = "Guardar archivo XML";
                    saveFileDialog.FileName = "guia_nubefact_" + "09-" + serie + "-" + correlativo + ".xml"; // Nombre de archivo por defecto

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        rutaDestino = saveFileDialog.FileName; // Guardar la ruta seleccionada por el usuario
                    }
                }

                if (!string.IsNullOrEmpty(rutaDestino))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            Console.WriteLine("Iniciando la descarga...");

                            // Descargar el contenido de forma sincrónica
                            byte[] contenido = client.GetByteArrayAsync(url).Result;

                            // Guardar el contenido en un archivo local
                            File.WriteAllBytes(rutaDestino, contenido);

                            Console.WriteLine($"Archivo descargado y guardado en {rutaDestino}");
                        }
                        catch (Exception ex)
                        {
                            return valor = false;
                        }
                    }
                }
                else
                {

                    return valor = false;
                }

                valor = true;
            }
            catch (Exception ex)
            {
                return valor = false;

            }

            return valor;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ////carga siempre
            //// envio de guias_pendientes a NUBEFACT
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
                        
                        " +   "\n" +

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
                        mensajerespuesta = (" Mensaje " + SerieNumero + "-" + GuiaNumero + "  " +  deserializedResponse.nota_importante);
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
                            mensajerespuesta =string.Empty;


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

        public class ErrorResponse
        {
            public string errors { get; set; }
            public int codigo { get; set; }
        }


    }
}
