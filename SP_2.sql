alter PROCEDURE SP_LIST_PENDIENTE_ENVIO_GRES_CONTENIDO
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Ciudad NVARCHAR(50)
AS
BEGIN

	-- CABECERA
SELECT		cab.CompaniaSocio AS CompaniaSocio
			,cab.SerieNumero AS SerieNumero
			,cab.GuiaNumero AS GuiaNumero


			,'generar_guia'																		AS operacion
			,'7'																				AS tipo_de_comprobante
			,cab.SerieNumero 																	AS serie --AS serie
			,ISNULL(right(cab.GuiaNumero, 8),'') 												AS numero
			,'6'																				AS cliente_tipo_de_documento -- ??? 6
			,ISNULL(cab.DestinatarioRUC,'')  													AS cliente_numero_de_documento --  RUC
			,ISNULL(LEFT (cab.DestinatarioNombre,100),'')										AS cliente_denominacion --  RAZON SOCIAL
			,ISNULL(LEFT (cab.DestinatarioDireccion,100),'')									AS cliente_direccion --  DestinatarioDireccion
			,'correoprueba@gmail.com'															AS cliente_email -- ??? cliente_email
			,'correoprueba1@gmail.com' 															AS cliente_email_1 -- ??? cliente_email xx
			,'correoprueba2@gmail.com'															AS cliente_email_2 -- ??? cliente_email xx
			,CONVERT(VARCHAR(10), cab.FechaDocumento, 105)										AS fecha_de_emision --  fecha_de_emision
			,ISNULL( LEFT(cab.Comentarios,100),'')												AS observaciones --  fecha_de_emision
			,ISNULL('01','')																	AS motivo_de_traslado -- ???  motivo_de_traslado
			,ISNULL('1','') 																	AS peso_bruto_total -- ???  peso_bruto_total
			,ISNULL('KGM','')																	AS peso_bruto_unidad_de_medida -- ???  KGM

			,ISNULL('1','') 																	AS numero_de_bultos --  ??? 1 Decimal Obligatorio 1 hasta 6 enteros
			,ISNULL('01','') 																	AS tipo_de_transporte --  ??? 01 String Obligatorio 2 exactos -- "01" = "TRANSPORTE PÚBLICO" "02" = "TRANSPORTE PRIVADO"  
			,CONVERT(VARCHAR(10), cab.FechaInicioTraslado, 105) 								AS fecha_de_inicio_de_traslado -- 15-10-2024


			,ISNULL('6','') 																	AS transportista_documento_tipo -- tabla LOG_TransportistaVehiculo 6 ???
			,ISNULL((SELECT v1.documentofiscal FROM LOG_TransportistaVehiculo v1
			WHERE v1.PlacaVehiculo = cab.TransportistaPlaca)  ,'')								AS transportista_documento_numero -- tabla LOG_TransportistaVehiculo documentofiscal ???
			,ISNULL((SELECT v1.Descripcion FROM LOG_TransportistaVehiculo v1
			WHERE v1.PlacaVehiculo = cab.TransportistaPlaca)  ,'') 								AS transportista_denominacion -- tabla LOG_TransportistaVehiculo Descripcion ???
			,ISNULL(LEFT(REPLACE(cab.TransportistaPlaca, '-', ''),8),'')						AS transportista_placa_numero --

			--SELECT * FROM LOG_TransportistaVehiculo
			/*
			,ISNULL('1','') AS conductor_documento_tipo -- ??
			,ISNULL('XXXXXXXX','') AS conductor_documento_numero -- ??
			,ISNULL('JORGE','') AS conductor_nombre -- ??
			,ISNULL('LOPEZ','') AS conductor_apellidos -- ??
			,ISNULL('QXXXXXXXX','') AS conductor_numero_licencia -- ??*/


			,ISNULL('151021','')																AS punto_de_partida_ubigeo -- falta , vladi , NO TENGO EL CAMPO EN LA TABLA.
			,ISNULL((SELECT Direccion from LOG_AlmacenMast
			WHERE AlmacenCodigo = cab.AlmacenCodigo)
			,'') 																				AS punto_de_partida_direccion -- select Direccion,* from LOG_AlmacenMast
			,ISNULL('0000','')																	AS punto_de_partida_codigo_establecimiento_sunat -- falta , vladi , NO TENGO EL CAMPO EN LA TABLA.


			--SELECT Destinatario, DestinatarioDireccionSecuencia,* FROM LOG_GuiaRemision_GE

			--SELECT AlmacenCodigo ,* FROM LOG_GuiaRemision_GE
			/*select * from MT_Direccion
			SELECT Destinatario, DestinatarioDireccionSecuencia ,* FROM LOG_GuiaRemision_GE*/

			,ISNULL((select Ubigeo from MT_Direccion
			WHERE Persona = cab.Destinatario AND Secuencia = cab.DestinatarioDireccionSecuencia),'')
																								AS punto_de_llegada_ubigeo -- XX
			,ISNULL((select Direccion from MT_Direccion
			WHERE Persona = cab.Destinatario AND Secuencia = cab.DestinatarioDireccionSecuencia),'')
																								AS punto_de_llegada_direccion -- XX
			,ISNULL('0000','')																	AS punto_de_llegada_codigo_establecimiento_sunat --


			,ISNULL('false','')																	AS enviar_automaticamente_al_cliente --
			,ISNULL('A4','') 																	AS	formato_de_pdf --

			/* Documento relacionado */
			/*
			,ISNULL('01','') AS tipo --
			,ISNULL('F001','') AS serie --
			,ISNULL('1','') AS numero --
			*/

			/* vehiculos_secundarios */
			,ISNULL('ABC445','') AS placa_numero --
			,ISNULL('ABC446','') AS placa_numero --



	,*
	FROM LOG_GuiaRemision_GE cab inner join LOG_GuiaRemisionDetalle_GE det ON cab.CompaniaSocio = det.CompaniaSocio AND
	cab.SerieNumero = det.SerieNumero AND
	cab.GuiaNumero = det.GuiaNumero 


END