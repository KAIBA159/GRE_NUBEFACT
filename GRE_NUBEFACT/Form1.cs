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
        string conexion = ConfigurationManager.ConnectionStrings["cnx"].ConnectionString;
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
            //carga siempre
            // envio de guias_pendientes a NUBEFACT
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

                    //DocEntry_aux_1 = dtc.Rows[i]["DocEntry"].ToString();
                    //ObjType_aux_1 = dtc.Rows[i]["ObjType"].ToString();
                    //aux_docsubtype = dtc.Rows[i]["DocSubType"].ToString();
                    //Descuento_global = dtc.Rows[i]["Porcentaje_GLOBAL"].ToString();
                    //count_fact_referenciada = dtc.Rows[i]["band_ant"].ToString();      //  para saber si tiene alguna factura por anticipo referenciada

                    ////oDocumento.facturaNegociable 

                    ////standd by
                    //#region FACT ANTICIPO
                    ////se envia flag solo si tiene un anticipo ingresara 
                    //if (count_fact_referenciada != "0")
                    //{

                    //    List<ws_dfacture.Anticipos> anticipo_list = new List<Anticipos>();
                    //    DataTable lista_anticipo = Common.getDocumentoProductoByDocEntryAndObjType_odpi_lista_anticipos(DocEntry_aux_1, ObjType_aux_1);
                    //    //List<ws_dfacture.Producto> productos = new List<Producto>();

                    //    for (int p = 0; p <= lista_anticipo.Rows.Count - 1; p++)
                    //    {
                    //        Anticipos anticipo_new = new Anticipos()
                    //        {

                    //            codigoTipoMoneda = lista_anticipo.Rows[p]["codigoTipoMoneda_LA"].ToString(),
                    //            fecha = lista_anticipo.Rows[p]["fecha_LA"].ToString(),
                    //            identificador = lista_anticipo.Rows[p]["identificador_LA"].ToString(),
                    //            montoPrepagado = lista_anticipo.Rows[p]["montoPrepagado_LA"].ToString(),
                    //            numerodocumento = lista_anticipo.Rows[p]["numerodocumento_LA"].ToString(),
                    //            rucEmisorComprobante = lista_anticipo.Rows[p]["rucEmisorComprobante_LA"].ToString(),
                    //            tipodocumento = lista_anticipo.Rows[p]["tipodocumento_LA"].ToString()


                    //        };

                    //        anticipo_list.Add(anticipo_new);
                    //        oDocumento.anticipo = anticipo_list.ToArray();

                    //    }

                    //    //productos.Add(oProducto);
                    //    //oDocumento.producto = productos.ToArray();


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

                            switch (Convert.ToInt32(deserializedResponse))
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


            //XmlDocument xmlfile_temp = new XmlDocument() ;
            //try
            //{
            //    DataTable dtc = Common.getDocumentoByIndicator("01");
            //    if (dtc.Rows.Count == 0) { return; }

            //    for (int i = 0; i <= dtc.Rows.Count - 1; i++)
            //    {
            //        string aux_docsubtype = "";
            //        string tipo_de_subtota = "IGV";
            //        string Descuento_global = "0.0";
            //        string count_fact_referenciada = string.Empty;

            //        //FacturaBE factura = new FacturaBE();
            //        string DocEntry_aux_1 = string.Empty;
            //        string ObjType_aux_1 = string.Empty;


            //        DocumentoElectronico oDocumento = new DocumentoElectronico();

            //        DocEntry_aux_1 = dtc.Rows[i]["DocEntry"].ToString();
            //        ObjType_aux_1 = dtc.Rows[i]["ObjType"].ToString();
            //        aux_docsubtype = dtc.Rows[i]["DocSubType"].ToString();
            //        Descuento_global = dtc.Rows[i]["Porcentaje_GLOBAL"].ToString();
            //        count_fact_referenciada = dtc.Rows[i]["band_ant"].ToString();      //  para saber si tiene alguna factura por anticipo referenciada

            //        //oDocumento.facturaNegociable 

            //        //standd by
            //        #region FACT ANTICIPO
            //        //se envia flag solo si tiene un anticipo ingresara 
            //        if (count_fact_referenciada != "0")
            //        {

            //            List<ws_dfacture.Anticipos> anticipo_list = new List<Anticipos>();
            //            DataTable lista_anticipo = Common.getDocumentoProductoByDocEntryAndObjType_odpi_lista_anticipos(DocEntry_aux_1, ObjType_aux_1);
            //            //List<ws_dfacture.Producto> productos = new List<Producto>();

            //            for (int p = 0; p <= lista_anticipo.Rows.Count - 1; p++)
            //            {
            //                Anticipos anticipo_new = new Anticipos()
            //                {

            //                    codigoTipoMoneda = lista_anticipo.Rows[p]["codigoTipoMoneda_LA"].ToString(),
            //                    fecha = lista_anticipo.Rows[p]["fecha_LA"].ToString(),
            //                    identificador = lista_anticipo.Rows[p]["identificador_LA"].ToString(),
            //                    montoPrepagado = lista_anticipo.Rows[p]["montoPrepagado_LA"].ToString(),
            //                    numerodocumento = lista_anticipo.Rows[p]["numerodocumento_LA"].ToString(),
            //                    rucEmisorComprobante = lista_anticipo.Rows[p]["rucEmisorComprobante_LA"].ToString(),
            //                    tipodocumento = lista_anticipo.Rows[p]["tipodocumento_LA"].ToString()


            //                };

            //                anticipo_list.Add(anticipo_new);
            //                oDocumento.anticipo = anticipo_list.ToArray();

            //            }

            //            //productos.Add(oProducto);
            //            //oDocumento.producto = productos.ToArray();

            //        }
            //        #endregion FACT ANTICIPO

            //        //oDocumento.facturaNegociable

            //        #region EMISOR

            //        DataTable dtEmisor = Common.getCompaniaEmisor();
            //        Emisor oEmisor = new Emisor()
            //        {


            //            domicilioFiscal = dtEmisor.Rows[0]["emisor_domicilioFiscal"].ToString(),//emisor_domicilioFiscal"AV. LA FONTANA NRO. 1182 URB. SANTA PATRICIA LIMA - LIMA - LA MOLINA",
            //            lugarExpedicion = dtEmisor.Rows[0]["codigo_lugarExpedicion"].ToString(),
            //            nombreComercial = dtEmisor.Rows[0]["emisor_nombreComercial"].ToString(),

            //            //direccion = dtEmisor.Rows[0]["emisor_nombreComercial"].ToString(),

            //            ruc = dtEmisor.Rows[0]["emisor_ruc"].ToString(),


            //        };
            //        oDocumento.emisor = oEmisor;

            //        #endregion

            //        #region RECEPTOR
            //        Receptor oReceptor = new Receptor()
            //        {
            //            tipoDocumento = dtc.Rows[i]["receptor_tipoDocumento"].ToString(),
            //            numDocumento = dtc.Rows[i]["receptor_numDocumento"].ToString(),
            //            razonSocial = dtc.Rows[i]["receptor_razonSocial"].ToString(),

            //            email = dtc.Rows[i]["receptor_email"].ToString(),
            //            ubigeo = dtc.Rows[i]["receptor_ubigeo"].ToString(),

            //            //AGREGADO
            //            direccion = dtc.Rows[i]["receptor_direccion"].ToString(),

            //            //,
            //        };

            //        oDocumento.receptor = oReceptor;
            //        #endregion

            //        #region COMPROBANTE
            //        oDocumento.fechaEmision = dtc.Rows[i]["fechaEmision"].ToString();
            //        oDocumento.horaEmision = dtc.Rows[i]["horaEmision"].ToString();
            //        oDocumento.tipoDocumento = dtc.Rows[i]["tipoDocumento"].ToString();
            //        oDocumento.serie = dtc.Rows[i]["serie"].ToString();
            //        oDocumento.correlativo = dtc.Rows[i]["correlativo"].ToString();
            //        oDocumento.codigoTipoOperacion = dtc.Rows[i]["codigoTipoOperacion"].ToString();
            //        oDocumento.idTransaccion = $"{dtc.Rows[i]["serie"].ToString()}-{dtc.Rows[i]["correlativo"].ToString()}";


            //        //oDocumento. = "0";


            //        #endregion

            //        #region PRODUCTOS

            //        DataTable dtp = Common.getDocumentoProductoByDocEntryAndObjType(DocEntry_aux_1, ObjType_aux_1);
            //        List<ws_dfacture.Producto> productos = new List<Producto>();

            //        for (int p = 0; p <= dtp.Rows.Count - 1; p++)
            //        {
            //            #region lineas de productos
            //            Producto oProducto = new Producto();

            //            string lineNum = dtp.Rows[p]["LineNum"].ToString();

            //            oProducto.numeroOrden = dtp.Rows[p]["numeroOrden"].ToString();//
            //            oProducto.unidadMedida = dtp.Rows[p]["unidadMedida"].ToString();    //
            //            oProducto.descripcion = dtp.Rows[p]["descripcion"].ToString();//  -
            //            oProducto.cantidad = dtp.Rows[p]["cantidad"].ToString();    //  -
            //            oProducto.valorUnitarioBI = dtp.Rows[p]["valorUnitarioBI"].ToString();          //
            //            oProducto.precioVentaUnitarioItem = dtp.Rows[p]["precioVentaUnitarioItem"].ToString();  //
            //            oProducto.valorVentaItemQxBI = dtp.Rows[p]["valorVentaItemQxBI"].ToString();            //
            //            oProducto.montoTotalImpuestoItem = dtp.Rows[p]["montoTotalImpuestoItem"].ToString();    //.
            //            oProducto.codigoPLU = dtp.Rows[p]["codigoPLU"].ToString();

            //            string porcentaje = dtp.Rows[p]["Porcentaje"].ToString();

            //            #region  Impuesto de la linea
            //            // campo version_ws

            //            //DataTable dtimpuesto = Common.getImpuestoProductoByDocEntryAndObjTypeAndLineNum(DocEntry_aux_1, ObjType_aux_1, lineNum);
            //            DataTable dtimpuesto = Common.getImpuestoProductoByDocEntryAndObjTypeAndLineNum(DocEntry_aux_1, ObjType_aux_1, lineNum, version_ws);

            //            tipo_de_subtota = dtp.Rows[p]["TipoImpuesto"].ToString();

            //            switch (dtp.Rows[p]["TipoImpuesto"].ToString())
            //            {
            //                case "IGV":
            //                    ProductoIGV lineIGV = new ProductoIGV()
            //                    {

            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString(),

            //                    };
            //                    oProducto.IGV = lineIGV;
            //                    break;
            //                case "EXP":
            //                    break;

            //                case "INA":

            //                    ProductoIGV lineIGV_INA = new ProductoIGV()
            //                    {
            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString(),
            //                        //lineIGV_INA.ExtensionData

            //                    };
            //                    oProducto.IGV = lineIGV_INA;
            //                    break;
            //                case "GRA":
            //                    break;
            //                case "EXO":
            //                    ProductoIGV lineIGV_EXO = new ProductoIGV()
            //                    {
            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString()

            //                    };
            //                    oProducto.IGV = lineIGV_EXO;
            //                    break;
            //                case "IRII":
            //                    ProductoIRII lineaIRII = new ProductoIRII()
            //                    {
            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString()
            //                    };
            //                    oProducto.IRII = lineaIRII;
            //                    break;
            //                case "ISC":
            //                    ProductoISC lineaISC = new ProductoISC()
            //                    {
            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString()
            //                    };
            //                    oProducto.ISC = lineaISC;
            //                    break;
            //                case "IVAP":
            //                    ProductoIVAP lineaIVAP = new ProductoIVAP()
            //                    {
            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString()
            //                    };
            //                    oProducto.IVAP = lineaIVAP;
            //                    break;

            //                default:
            //                    List<ProductoOtrosTributos> lproductoOtrosTributos = new List<ProductoOtrosTributos>();
            //                    ProductoOtrosTributos lineaotrosTributos = new ProductoOtrosTributos()
            //                    {
            //                        porcentaje = dtimpuesto.Rows[0]["porcentaje"].ToString(),
            //                        tipo = dtimpuesto.Rows[0]["tipo"].ToString(),
            //                        monto = dtimpuesto.Rows[0]["monto"].ToString(),
            //                        baseImponible = dtimpuesto.Rows[0]["baseImponible"].ToString()
            //                    };
            //                    lproductoOtrosTributos.Add(lineaotrosTributos);
            //                    oProducto.otrosTributos = lproductoOtrosTributos.ToArray();
            //                    break;
            //            }
            //            #endregion

            //            if (porcentaje != "0.000000")
            //            {
            //                ProductoDescuento pd = new ProductoDescuento()
            //                {
            //                    baseImponible = dtp.Rows[p]["valorUnitarioBI"].ToString(),
            //                    codigo = "00",                         // LISTO
            //                    monto = dtp.Rows[p]["monto_descuento"].ToString(),
            //                    porcentaje = dtp.Rows[p]["Porcentaje_dos"].ToString()//  Convert.ToString(Convert.ToInt32(dtp.Rows[p]["Porcentaje_dos"].ToString())/100) 

            //                };

            //                oProducto.descuento = pd;

            //            }

            //            //inicio

            //            //fin

            //            productos.Add(oProducto);
            //            oDocumento.producto = productos.ToArray();
            //            #endregion
            //        }
            //        #endregion

            //        #region DESCUENTO_GLOBAL

            //        if (dtc.Rows[i]["Porcentaje_GLOBAL"].ToString() != "0.000000")
            //        {
            //            DescuentosGlobales oDescuentoglo = new DescuentosGlobales()
            //            {


            //                baseImponible = dtc.Rows[i]["DG_baseImponible"].ToString(),
            //                monto = dtc.Rows[i]["DG_monto_descuent"].ToString(),
            //                motivo = "03",
            //                porcentaje = dtc.Rows[i]["DG_Porcentaje_dos"].ToString()
            //                //<n1:baseImponible > 118.00 </ n1:baseImponible >
            //                //< n1:monto > 20 </ n1:monto >
            //                //< n1:motivo > 03 </ n1:motivo >
            //                //< n1:porcentaje > 0.16949 </ n1:porcentaje >
            //            };

            //            oDocumento.descuentosGlobales = oDescuentoglo;

            //        }

            //        #endregion DESCUENTO_GLOBAL

            //        #region TOTALES
            //        DataTable dtTotales = Common.getTotalesDocumentoByDocEntryAndObjType(DocEntry_aux_1, ObjType_aux_1);
            //        Totales oTotales = new Totales()
            //        {

            //            //  LISTO   IGV,EXO,INA


            //            importeTotalVenta = dtTotales.Rows[0]["importeTotalVenta"].ToString(),      //  LISTO   IGV,EXO,INA
            //            importeTotalPagar = dtTotales.Rows[0]["importeTotalPagar"].ToString(),       //  LISTO   IGV, EXO, INA
            //            montoTotalImpuestos = dtTotales.Rows[0]["montoTotalImpuestos"].ToString(),  //  LISTO   IGV,EXO,INA


            //            subtotalValorVenta = dtTotales.Rows[0]["subtotalValorVenta"].ToString(),    //  LISTO   IGV,EXO,INA

            //            totalIGV = dtTotales.Rows[0]["totalIGV"].ToString(),
            //            //totalIRII = dtTotales.Rows[0]["totalIRII"].ToString(),
            //            //totalISC = dtTotales.Rows[0]["totalISC"].ToString(),
            //            //totalIVAP = dtTotales.Rows[0]["totalIVAP"].ToString(),

            //            montoTotalAnticipos = dtTotales.Rows[0]["MontoTotalAnticipo"].ToString(),


            //            //if (count_fact_referenciada != "0")
            //            //{
            //            //        monto
            //            //        //montoTotalAnticipos = "100.00",
            //            //}

            //        };



            //        #region SUBTOTAL
            //        DataTable dtSubTotal = Common.getTotalesSubTotalDocumentoByDocEntryAndObjType(DocEntry_aux_1, ObjType_aux_1);

            //        //Subtotal oSubtotal = new Subtotal()
            //        //{
            //        //    IGV         = dtSubTotal.Rows[0]["IGV"].ToString(),
            //        //    inafectas   = "0"
            //        //};
            //        //oTotales.subtotal = oSubtotal;




            //        if (tipo_de_subtota == "IGV")
            //        {
            //            Subtotal oSubtotal = new Subtotal()
            //            {

            //                IGV = dtSubTotal.Rows[0]["IGV"].ToString()
            //            };
            //            oTotales.subtotal = oSubtotal;


            //        }

            //        if (tipo_de_subtota == "EXO")
            //        {
            //            Subtotal oSubtotal = new Subtotal()
            //            {

            //                IGV = dtSubTotal.Rows[0]["IGV"].ToString(),
            //                gratuitas = dtSubTotal.Rows[0]["gratuitas"].ToString()
            //            };
            //            oTotales.subtotal = oSubtotal;


            //        }

            //        if (tipo_de_subtota == "INA")
            //        {
            //            Subtotal oSubtotal = new Subtotal()
            //            {
            //                IGV = dtSubTotal.Rows[0]["IGV"].ToString(),
            //                inafectas = "0"

            //            };
            //            oTotales.subtotal = oSubtotal;
            //        }

            //        #endregion

            //        oDocumento.totales = oTotales;

            //        #endregion TOTALES

            //        #region  PAGO
            //        DataTable dtPago = Common.getPagoDocumentoByDocEntryAndObjType(DocEntry_aux_1, ObjType_aux_1);
            //        Pago oPago = new Pago()
            //        {
            //            moneda = dtPago.Rows[0]["moneda"].ToString(),
            //            fechaInicio = dtPago.Rows[0]["fechaInicio"].ToString(),
            //            fechaFin = dtPago.Rows[0]["fechaFin"].ToString(),
            //            tipoCambio = dtPago.Rows[0]["tipoCambio"].ToString()//,
            //            //modo
            //        };
            //        oDocumento.pago = oPago;
            //        #endregion

            //        #region  ModoPago
            //        //DataTable dtPago = Common.getPagoDocumentoByDocEntryAndObjType(DocEntry_aux_1, ObjType_aux_1);
            //        if (version_ws == "v2")
            //        {
            //            ModoPago oModoPago = new ModoPago()
            //            {
            //                //moneda = dtPago.Rows[0]["moneda"].ToString(),
            //                //fechaInicio = dtPago.Rows[0]["fechaInicio"].ToString(),
            //                //fechaFin = dtPago.Rows[0]["fechaFin"].ToString(),
            //                //tipoCambio = dtPago.Rows[0]["tipoCambio"].ToString()//,
            //                ////modo
            //                ///
            //                modoPago = "Contado",
            //                montoNetoPendiente = "0.00"
            //            };

            //            oDocumento.facturaNegociable = oModoPago;
            //        }
            //        else
            //        {

            //        }



            //        #endregion

            //        #region lineasadicionales

            //        //DataTable dtp = Common.getDocumentoProductoByDocEntryAndObjType(DocEntry_aux_1, ObjType_aux_1);
            //        List<ws_dfacture.PersonalizacionPDF> PersonalizacionPDF_ob = new List<PersonalizacionPDF>();

            //        #region lineas de productos
            //        PersonalizacionPDF oPersonalizacionPDF = new PersonalizacionPDF();

            //        //string lineNum = dtp.Rows[p]["LineNum"].ToString();

            //        oPersonalizacionPDF.seccion = "1";
            //        oPersonalizacionPDF.titulo = "Comentarios";
            //        oPersonalizacionPDF.valor = dtc.Rows[i]["Comentarios_InformacionAdcional_observaciones"].ToString();


            //        PersonalizacionPDF_ob.Add(oPersonalizacionPDF);
            //        oDocumento.personalizacionPDF = PersonalizacionPDF_ob.ToArray();
            //        #endregion

            //        #endregion



            //        #region historia_FACT 
            //        // BD SCO_FE INTERMEDIA
            //        //consulta_unico registro
            //        DataTable dt_SCO_FE_UNICO = Common.Consultar_unico_registro(dtc.Rows[i]["DocEntry"].ToString(),
            //                                                                        dtc.Rows[i]["DocNum"].ToString(),
            //                                                                        dtc.Rows[i]["ObjType"].ToString(),
            //                                                                        dtc.Rows[i]["DocType"].ToString(),
            //                                                                        dtc.Rows[i]["DocSubType"].ToString()

            //                                                                        );
            //        //INICIO  CONSULTA si existe o no el registro de dicho documento.
            //        if (dt_SCO_FE_UNICO.Rows.Count == 0) //validar si ya existe el documento.
            //        {
            //            Common.Insertar_Table_SCORE_FE(
            //                                        dtc.Rows[i]["DocEntry"].ToString(),
            //                                        dtc.Rows[i]["DocNum"].ToString(),
            //                                        dtc.Rows[i]["ObjType"].ToString(),
            //                                        dtc.Rows[i]["DocType"].ToString(),
            //                                        dtc.Rows[i]["DocSubType"].ToString(),

            //                                        Convert.ToDateTime(dtc.Rows[i]["DocDate"].ToString()),
            //                                        Convert.ToDateTime(dtc.Rows[i]["DocDueDate"].ToString()),
            //                                        Convert.ToDateTime(dtc.Rows[i]["TaxDate"].ToString()),
            //                                        dtc.Rows[i]["CardCode"].ToString(),
            //                                        dtc.Rows[i]["receptor_razonSocial"].ToString(),
            //                                        dtc.Rows[i]["NumAtCard"].ToString(),
            //                                        dtc.Rows[i]["serie"].ToString(),
            //                                        dtc.Rows[i]["correlativo"].ToString(),
            //                                        dtc.Rows[i]["tipoDocumento"].ToString(),
            //                                        Convert.ToDateTime(DateTime.Now),
            //                                        Convert.ToDateTime(DateTime.Now),
            //                                        dtc.Rows[i]["UserSign"].ToString(),
            //                                        Convert.ToDateTime(dtc.Rows[i]["UpdateDate"].ToString()),
            //                                        Convert.ToDateTime(dtc.Rows[i]["UpdateDate"].ToString())


            //            );
            //        }
            //        else
            //        {
            //            Common.SCO_FE_ALTER_EXISTE_DOC(
            //                                                dtc.Rows[i]["DocEntry"].ToString(),
            //                                                dtc.Rows[i]["DocNum"].ToString(),
            //                                                dtc.Rows[i]["ObjType"].ToString(),
            //                                                dtc.Rows[i]["DocType"].ToString(),
            //                                                dtc.Rows[i]["DocSubType"].ToString(),

            //                                                Convert.ToDateTime(DateTime.Now),
            //                                                Convert.ToDateTime(DateTime.Now),
            //                                                Convert.ToDateTime(dtc.Rows[i]["UpdateDate"].ToString()),
            //                                                Convert.ToDateTime(dtc.Rows[i]["UpdateDate"].ToString()),
            //                                                dtc.Rows[i]["UserSign2"].ToString()


            //                );



            //        }

            //        #endregion historia_FACT   

            //        #region  REQUEST
            //        string ruta_request_1 = string.Empty;
            //        string hora_minuto_seg = DateTime.Now.ToString("HHmmss");
            //        string ano_mes_dia = DateTime.Now.ToString("yyyyMMdd");
            //        string ruta_creacion = PATH_REQUEST + "\\" + ano_mes_dia;

            //        if (!Directory.Exists(ruta_creacion))
            //        {
            //            Directory.CreateDirectory(PATH_REQUEST + "\\" + ano_mes_dia);
            //        }

            //        ruta_request_1 = ruta_creacion + "\\" + dtc.Rows[i]["tipoDocumento"].ToString() + "-" +
            //                                                                        dtc.Rows[i]["serie"].ToString() + "-" +
            //                                                                        dtc.Rows[i]["correlativo"].ToString() + "_" + hora_minuto_seg + ".xml";

            //        XmlSerializer formateador = new XmlSerializer(typeof(DocumentoElectronico));
            //        Stream miStream = new FileStream(ruta_request_1, FileMode.Create, FileAccess.Write, FileShare.None);
            //        formateador.Serialize(miStream, oDocumento);
            //        miStream.Close();
            //        #endregion

            //        var response = proxy.Enviar(ruc, usuario, clave, oDocumento);
            //        //ADocumentoElectronico.A

            //        string cont_1 = "";

            //        cont_1 = response.xml;
            //        //response.
            //        //INICIO    webservice

            //        #region actualizar_1

            //        string numeracion = string.Empty;
            //        //string numeracion_aux = null;
            //        if (response.numeracion == null)
            //        {
            //            numeracion = "";
            //        }
            //        else
            //        {
            //            numeracion = response.numeracion;
            //        }

            //        Common.SCO_FE_ALTER_1(
            //                                        dtc.Rows[i]["DocEntry"].ToString(),
            //                                        dtc.Rows[i]["DocNum"].ToString(),
            //                                        dtc.Rows[i]["ObjType"].ToString(),
            //                                        dtc.Rows[i]["DocType"].ToString(),
            //                                        dtc.Rows[i]["DocSubType"].ToString(),

            //                                        DateTime.Now,
            //                                        DateTime.Now,
            //                                        dtc.Rows[i]["UserSign2"].ToString(),
            //                                        DateTime.Now,
            //                                        DateTime.Now,
            //                                        Convert.ToString(response.codigo),
            //                                        response.mensaje,
            //                                        numeracion
            //            );

            //        #endregion actualizar_1
            //        //FIN   WEBSERVICE

            //        if (response.codigo == 0)
            //        {


            //            //JS update a la base de datos_ pendiente
            //            DataTable update_dtp = Common.update_response_oinv(DocEntry_aux_1, ObjType_aux_1, "Código: " + response.codigo + " - " + response.mensaje, "1");

            //            //js UPDATE DEL CDR
            //        }
            //        else
            //        {
            //            //replicar a todos los documentos.
            //            if (response.codigo == 207)
            //            {
            //                DataTable update_dtp = Common.update_response_oinv(DocEntry_aux_1, ObjType_aux_1, "Código: " + response.codigo + " - " + "La numeración ya existe, esta en Uso,sino asignar el Estado Enviado", "2");

            //            }
            //            else
            //            {

            //                DataTable update_dtp = Common.update_response_oinv(DocEntry_aux_1, ObjType_aux_1, "Código: " + response.codigo + " - " + response.mensaje, "2");


            //            }

            //        }


            //    }


            //}
            //catch (Exception ex)
            //{
            //    //Console.WriteLine(ex.Message);

            //    this.log_1(DateTime.Now + "  : Error = " + ex.Message);
            //}

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
