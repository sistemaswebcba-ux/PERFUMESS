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
    public partial class FrmAbmCliente : FormBase
    {
        cFunciones fun;
        public FrmAbmCliente()
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

        private void FrmAbmCliente_Load(object sender, EventArgs e)
        {
            Botonera(1);
            Grupo.Enabled = false;
            fun = new cFunciones();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            if (txt_NroDoc.Text == "")
            {
                Mensaje("Debe ingresar un número de documento ");
                return;
            }

            if (txt_Nombre.Text == "")
            {
                Mensaje("Debe ingresar un nombre de usuario continuar");
                return;
            }
             
            if (txt_Apellido.Text == "")
            {
                Mensaje("Debe ingresar un apellido ");
                return;
            }

            if (txtCodigo.Text == "")
                fun.GuardarNuevoGenerico(this, "Cliente");
            else
            {
                // if (txt_Ruta.Text != "")
                //   txt_Ruta.Text = txt_Ruta.Text.Replace("\\", "\\\\");
                fun.ModificarGenerico(this, "Cliente", "CodCliente", txtCodigo.Text);

            }
            MessageBox.Show("Datos grabados correctamente");
            fun.LimpiarGenerico(this);
            Botonera(1);
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            Principal.OpcionesdeBusqueda = "Nombre";
            Principal.TablaPrincipal = "Cliente";
            Principal.OpcionesColumnasGrilla = "CodCliente;NroDoc;Apellido;Nombre";
            Principal.ColumnasVisibles = "0;1;1;1";
            Principal.ColumnasAncho = "0;100;240;240";
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

                fun.CargarControles(this, "Cliente", "CodCliente", txtCodigo.Text);

            }
            Grupo.Enabled = false;

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
            fun.LimpiarGenerico(this);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            Botonera(1);
            Grupo.Enabled = false;
            fun.LimpiarGenerico(this);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            cFunciones fun = new Clases.cFunciones();
            string msj = "Confirma Eliminar el Cliente ";
            var result = MessageBox.Show(msj, "Información",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }
            try
            {
                fun.EliminarGenerico("Cliente", "CodCliente", txtCodigo.Text);
                MessageBox.Show("Datos Borrados correctamente");
                fun.LimpiarGenerico(this);
                Botonera(1);
                Grupo.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puede eliminar el registro, tien datos asociados");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_NroDoc_TextChanged(object sender, EventArgs e)
        {
            int b = 0;
            if (txt_NroDoc.Text.Length >0)
            {
                string NroDco = txt_NroDoc.Text;
                cCliente cli = new Clases.cCliente();
                string NroDoc = txt_NroDoc.Text;
                DataTable trdo = cli.GetClientexNroDoc(NroDoc);
                if (trdo.Rows.Count >0)
                {
                    if (trdo.Rows[0]["CodCliente"].ToString ()!="")
                    {
                        txtCodigo.Text = trdo.Rows[0]["CodCliente"].ToString();
                        txt_Nombre.Text = trdo.Rows[0]["Nombre"].ToString();
                        txt_Apellido.Text = trdo.Rows[0]["Apellido"].ToString();
                        b = 1;
                    }
                }
            }
            if (b ==0)
            {
                txtCodigo.Text = "";
                txt_Apellido.Text = "";
                txt_Nombre.Text = "";
            }
        }
    }
}
