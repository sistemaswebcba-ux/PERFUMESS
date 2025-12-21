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
    public partial class FrmAmbProducto : FormBase 
    {
        cFunciones fun;
        public FrmAmbProducto()
        {
            InitializeComponent();
        }

        private void Botonera(int Jugada)
        {
            switch (Jugada)
            {
                //estado inicial
                case 1:
                    btnNuevo.Enabled = true;
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnAceptar.Enabled = false;
                    btnCancelar.Enabled = false;
                    break;
                case 2:
                    btnNuevo.Enabled = false;
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = true;
                    btnAceptar.Enabled = true;
                    btnCancelar.Enabled = true;
                    break;
                case 3:
                    //viene del buscador
                    btnNuevo.Enabled = true;
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                    btnAceptar.Enabled = false;
                    btnCancelar.Enabled = false;
                    break;
            }

        }

        private void FrmAmbProducto_Load(object sender, EventArgs e)
        {
            Botonera(1);
            Grupo.Enabled = false;
            fun = new cFunciones();
            fun.LlenarCombo(cmb_CodMarca, "Marca", "Nombre", "CodMarca");
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txt_Nombre.Text == "")
            {
                Mensaje("Debe ingresar un nombre de usuario continuar");
                return;
            }
            
            if (txt_Precio.Text == "")
            {
                Mensaje("Debe ingresar un precio para continuar");
                return;
            }

            txt_Precio.Text = txt_Precio.Text.Replace(".", "");
            if (txt_Costo.Text !="")
            {
                txt_Costo.Text = txt_Costo.Text.Replace(".", "");
            }
            if (txtCodigo.Text == "")
                fun.GuardarNuevoGenerico(this, "Producto");
            else
            {
                // if (txt_Ruta.Text != "")
                //   txt_Ruta.Text = txt_Ruta.Text.Replace("\\", "\\\\");
                fun.ModificarGenerico(this, "Producto", "CodProducto", txtCodigo.Text);

            }
            MessageBox.Show("Datos grabados correctamente");
            fun.LimpiarGenerico(this);
            Botonera(1);
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            Principal.OpcionesdeBusqueda = "Nombre";
            Principal.TablaPrincipal = "Producto";
            Principal.OpcionesColumnasGrilla = "CodProducto;Nombre";
            Principal.ColumnasVisibles = "0;1";
            Principal.ColumnasAncho = "0;580";
            FrmBuscadorGenerico form = new FrmBuscadorGenerico();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Principal.CodigoPrincipalAbm != null)
            {
                Botonera(3);
                txtCodigo.Text = Principal.CodigoPrincipalAbm.ToString();

                fun.CargarControles(this, "Producto", "CodProducto", txtCodigo.Text);
                if (txt_Precio.Text !="")
                {
                    txt_Precio.Text = fun.SepararDecimales(txt_Precio.Text);
                }
                 
                if (txt_Costo.Text != "")
                {
                    txt_Costo.Text = fun.SepararDecimales(txt_Costo.Text);
                }

            }
            Grupo.Enabled = false;

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
            fun.LimpiarGenerico(this);
        }
    }
}
