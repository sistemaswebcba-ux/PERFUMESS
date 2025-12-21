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
    public partial class FrmBuscadorCliente : FormBase
    {
        public FrmBuscadorCliente()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtApellido.Text =="")
            {
                MessageBox.Show("Debe ingresar un apellido ");
                return;
            }
            cCliente cli = new Clases.cCliente();
            cFunciones fun = new cFunciones();
            string apellido = txtApellido.Text;
            DataTable trdo = cli.GetClientexApellido(apellido);
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "0;20;40;40");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un producto para continuar ");
                return;
            }

            Int32 CodCliente = Convert.ToInt32(Grilla.CurrentRow.Cells[0].Value);
            Principal.CodCliente = CodCliente;
            this.Close();
        }

        private void FrmBuscadorCliente_Load(object sender, EventArgs e)
        {
            cFunciones fun = new Clases.cFunciones();
            fun.EstiloBotones(btnAceptar);
            fun.EstiloBotones(btnCancelar);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
