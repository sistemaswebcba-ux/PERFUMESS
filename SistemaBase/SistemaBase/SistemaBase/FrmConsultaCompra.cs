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
    public partial class FrmConsultaCompra : FormBase
    {
        public FrmConsultaCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Buscar()
        {
            int Cantidad = 0;
            Double Total = 0;
            cFunciones fun = new Clases.cFunciones();
            cCompra compra = new Clases.cCompra();
            DateTime FechaDesde = Convert.ToDateTime(daFechaDesde.Value);
            DateTime FechaHasta = Convert.ToDateTime(daFechaHasta.Value);
            DataTable trdo = compra.GetComprassxFecha(FechaDesde, FechaHasta);
            Total = fun.TotalizarColumna(trdo, "Total");
            Cantidad = trdo.Rows.Count;
            trdo = fun.TablaaMiles(trdo, "Total");
            trdo = fun.TablaaFechas(trdo, "Fecha");
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "0;20;50;30");
            txtCantidad.Text = Cantidad.ToString();
            txtTotal.Text = fun.SepararDecimales(Total.ToString());
        }

        private void InicializarFechas()
        {
            DateTime Hoy = DateTime.Now;
            daFechaHasta.Value = Hoy;
            daFechaDesde.Value = (Hoy.AddMonths(-1));
        }

        private void FrmConsultaCompra_Load(object sender, EventArgs e)
        {
            InicializarFechas();
            Buscar();
        }

        private void btnAbriVenta_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow ==null)
            {
                MessageBox.Show("Debe seleccionar una compra ");
                return;
            }

            Int32 CodCompra = Convert.ToInt32(Grilla.CurrentRow.Cells[0].Value);
            Principal.CodCompra = CodCompra;
            FrmCompra frm = new SistemaBase.FrmCompra();
            frm.ShowDialog();
        }
    }
}
