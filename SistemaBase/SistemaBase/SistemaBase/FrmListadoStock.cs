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
    public partial class FrmListadoStock : FormBase
    {
        public FrmListadoStock()
        {
            InitializeComponent();
        }

        private void FrmListadoStock_Load(object sender, EventArgs e)
        {
            Inicializar();
            Buscar();
        }

        private void Buscar()
        {
            int SinStock = 0;
            if (chkSinStock.Checked == true)
                SinStock = 1;
            Int32? CodMarca = null;
            if (CmbMarca.SelectedIndex >0)
                CodMarca = Convert.ToInt32(CmbMarca.SelectedValue);
            string Nombre = "";
            if (txtNombre.Text != "")
                Nombre = txtNombre.Text;
            cProducto prod = new Clases.cProducto();
            DataTable trdo = prod.GetProductos(SinStock, CodMarca, Nombre);
            cFunciones fun = new cFunciones();
            trdo = fun.TablaaMiles(trdo, "Costo");
            trdo = fun.TablaaMiles(trdo, "Precio");
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "0;50;20;10;10;10");
            Grilla.DataSource = trdo;
        }

        private void Inicializar()
        {
            cFunciones fun = new cFunciones();
            fun.LlenarCombo(CmbMarca, "Marca", "Nombre", "CodMarca");
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }
    }
}
