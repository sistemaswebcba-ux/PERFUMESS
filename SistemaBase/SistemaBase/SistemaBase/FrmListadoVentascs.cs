using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Excel;
using SistemaBase.Clases;

namespace SistemaBase
{
    public partial class FrmListadoVentascs : FormBase
    {
        public FrmListadoVentascs()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {         
            Buscar();
        }

        private void Buscar()
        {
            Double Total = 0;
            Double Ganancia = 0;
            cFunciones fun = new cFunciones();
            DateTime FechaDesde = daFechaDesde.Value;
            DateTime FechaHasta = daFechaHasta.Value;
            cVenta venta = new Clases.cVenta();
            Int32? CodUsuario = null;
            if (cmbUsuario.SelectedIndex>0)
            {
                CodUsuario = Convert.ToInt32(cmbUsuario.SelectedValue);
            }
            DataTable trdo = venta.GetVentasxFecha(FechaDesde, FechaHasta, CodUsuario);
            Total = fun.TotalizarColumna(trdo, "Total");
            Ganancia = fun.TotalizarColumna(trdo, "Ganancia");
            trdo = fun.TablaaMiles(trdo, "Total");
            trdo = fun.TablaaFechas(trdo, "Fecha");
            trdo = fun.TablaaMiles(trdo, "Ganancia");
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "10;20;25;15;15;15");
            Grilla.Columns[0].HeaderText = "Nro";
            txtTotal.Text = fun.SepararDecimales(Total.ToString());
            txtGanancia.Text = fun.SepararDecimales(Ganancia.ToString());
            txtCantidad.Text = trdo.Rows.Count.ToString();
           
        }

        private void FrmListadoVentascs_Load(object sender, EventArgs e)
        {
            CargarUsuario();
            Buscar();

        }

        private void CargarUsuario()
        {
            cFunciones fun = new cFunciones();
            fun.LlenarCombo(cmbUsuario, "Usuario", "Nombre", "CodUsuario");
        }

        private void btnAbriVenta_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow ==null)
            {
                MessageBox.Show("Debe seleccionar un elemento para continaur");
                return;
            }

            Int32 CodVenta = Convert.ToInt32(Grilla.CurrentRow.Cells[0].Value);
            Principal.CodVenta = CodVenta;
            FrmVenta frm = new FrmVenta();
            frm.FormClosing += new FormClosingEventHandler(form_FormClosing);
            frm.ShowDialog();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Buscar();
        }
    }
}
