namespace WS_GRENUBEFACT
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviciogre_v6 = new System.ServiceProcess.ServiceProcessInstaller();
            this.WS_Servicio_gre_nubefact_v6 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviciogre_v6
            // 
            this.serviciogre_v6.Password = null;
            this.serviciogre_v6.Username = null;
            // 
            // WS_Servicio_gre_nubefact_v6
            // 
            this.WS_Servicio_gre_nubefact_v6.Description = "envio de gre a nubefact v6";
            this.WS_Servicio_gre_nubefact_v6.DisplayName = "WS_GRENUBEFACT";
            this.WS_Servicio_gre_nubefact_v6.ServiceName = "WS_GRENUBEFACT";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.WS_Servicio_gre_nubefact_v6,
            this.serviciogre_v6});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviciogre_v6;
        private System.ServiceProcess.ServiceInstaller WS_Servicio_gre_nubefact_v6;
    }
}