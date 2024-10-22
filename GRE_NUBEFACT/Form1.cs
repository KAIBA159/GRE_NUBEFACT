using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

namespace GRE_NUBEFACT
{
    public partial class Form1 : Form
    {

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

    }
}
