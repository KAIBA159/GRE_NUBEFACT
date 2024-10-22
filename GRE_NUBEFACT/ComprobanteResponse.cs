using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

public class ComprobanteResponse
{
    [JsonPropertyName("tipo_de_comprobante")]
    public int TipoDeComprobante { get; set; }

    [JsonPropertyName("serie")]
    public string Serie { get; set; }

    [JsonPropertyName("numero")]
    public int Numero { get; set; }

    [JsonPropertyName("enlace")]
    public string Enlace { get; set; }

    [JsonPropertyName("aceptada_por_sunat")]
    public bool AceptadaPorSunat { get; set; }

    [JsonPropertyName("sunat_description")]
    public string SunatDescription { get; set; }

    [JsonPropertyName("sunat_note")]
    public string SunatNote { get; set; }

    [JsonPropertyName("sunat_responsecode")]
    public string SunatResponseCode { get; set; }

    [JsonPropertyName("sunat_soap_error")]
    public string SunatSoapError { get; set; }

    [JsonPropertyName("pdf_zip_base64")]
    public string PdfZipBase64 { get; set; }

    [JsonPropertyName("xml_zip_base64")]
    public string XmlZipBase64 { get; set; }

    [JsonPropertyName("cdr_zip_base64")]
    public string CdrZipBase64 { get; set; }

    [JsonPropertyName("cadena_para_codigo_qr")]
    public string CadenaParaCodigoQr { get; set; }

    [JsonPropertyName("enlace_del_pdf")]
    public string EnlaceDelPdf { get; set; }

    [JsonPropertyName("enlace_del_xml")]
    public string EnlaceDelXml { get; set; }

    [JsonPropertyName("enlace_del_cdr")]
    public string EnlaceDelCdr { get; set; }
}