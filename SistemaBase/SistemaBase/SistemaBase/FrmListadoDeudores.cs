using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaBase.Clases;

namespace SistemaBase
{
    public partial class FrmListadoDeudores : FormBase
    {
        public FrmListadoDeudores()
        {
            InitializeComponent();
        }

        private void FrmListadoDeudores_Load(object sender, EventArgs e)
        {
            cFunciones fun = new Clases.cFunciones();
            fun.EstiloBotones(btnCobrar);
            BuscarDeudores();
        }

        private void BuscarDeudores()
        {
            cFunciones fun = new cFunciones();
            cVenta venta = new cVenta();
            DataTable trdo = venta.GetDeudores();
            trdo = fun.TablaaFechas(trdo, "Fecha");
            trdo = fun.TablaaMiles(trdo, "Ganancia");
            trdo = fun.TablaaMiles(trdo, "Total");
            trdo = fun.TablaaMiles(trdo, "Saldo");
            trdo = fun.TablaaMiles(trdo, "Entrega");
            Grilla.DataSource = trdo;
            
            fun.AnchoColumnas(Grilla, "0;15;25;15;15;15;15");

        }
    }
}
