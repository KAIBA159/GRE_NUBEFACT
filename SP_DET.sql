ALTER PROCEDURE SP_LIST_PENDIENTE_ENVIO_GRE_DETALLE
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Ciudad NVARCHAR(50)
AS
BEGIN

-- DETALLE
	SELECT
		cab.CompaniaSocio AS CompaniaSocio
		,cab.SerieNumero AS SerieNumero
		,cab.GuiaNumero AS GuiaNumero

		,'NIU' AS unidad_de_medida
		,ISNULL( LEFT(det.ItemCodigo,250),'') AS codigo
		,ISNULL( LEFT(det.Descripcion,250),'') AS descripcion
		,det.Cantidad AS cantidad

	--,*
	FROM LOG_GuiaRemision_GE cab inner join LOG_GuiaRemisionDetalle_GE det ON cab.CompaniaSocio = det.CompaniaSocio AND
	cab.SerieNumero = det.SerieNumero AND
	cab.GuiaNumero = det.GuiaNumero

END