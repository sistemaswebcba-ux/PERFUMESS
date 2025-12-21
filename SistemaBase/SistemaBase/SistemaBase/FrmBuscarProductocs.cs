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
    public partial class FrmBuscarProductocs : FormBase
    {
        public FrmBuscarProductocs()
        {
            InitializeComponent();
        }

        private void BuscarProducto()
        {
            cProducto prod = new Clases.cProducto();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            cFunciones fun = new Clases.cFunciones();
            if (txtNombre.Text =="")
            {
                MessageBox.Show("Debe ingresar un´nombre del producto ");
                return;
            }

            string Nombre = txtNombre.Text;
            cProducto prod = new Clases.cProducto();
            DataTable trdo = prod.GetProductoxNombre(Nombre);
            trdo = fun.TablaaMiles(trdo , "Precio");
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "0;40;20;20;20");
            Grilla.Columns[4].HeaderText = "Codigo Interno"; 
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar un producto para continuar ");
                return;
            }

            Int32 CodProducto = Convert.ToInt32(Grilla.CurrentRow.Cells[0].Value);
            Principal.CodProoducto = CodProducto;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmBuscarProductocs_Load(object sender, EventArgs e)
        {   
            cFunciones fun = new Clases.cFunciones();
            fun.EstiloBotones(btnAceptar);
            fun.EstiloBotones(btnCancelar);
        }
    }
}
