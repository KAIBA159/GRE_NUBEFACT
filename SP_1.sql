
ALTER PROCEDURE SP_INSERT_RESPUESTA
    @v_compania NVARCHAR(50),
    @v_serie NVARCHAR(50),
    @v_correlativo NVARCHAR(50),
	@v_codigo integer,
	@v_mensajerespuesta NVARCHAR(100)

AS
BEGIN




	UPDATE	LOG_GuiaRemision_GE 
    SET		SUNAT_Rpta  = @v_codigo,
			SUNAT_Rpta_Mensaje = @v_mensajerespuesta

    WHERE CompaniaSocio = @v_compania 
			and SerieNumero = @v_serie
			and GuiaNumero = @v_correlativo

	RETURN @@ROWCOUNT;

END

/*
--	SELECT SUNAT_Rpta,SUNAT_Rpta_Mensaje,* FROM LOG_GuiaRemision_GE
--	SELECT * FROM LOG_GuiaRemision_GE

	UPDATE	LOG_GuiaRemision_GE 
    SET		SUNAT_Rpta  = 0,
			SUNAT_Rpta_Mensaje = ''
*/