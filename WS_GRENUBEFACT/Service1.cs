using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WS_GRENUBEFACT
{
    public partial class Service1 : ServiceBase
    {
        public int time_event = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimeService"]);  //RUTA DEL RUC


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
