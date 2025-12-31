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
using System.Data.SqlClient;

namespace SistemaBase
{
    public partial class FrmCompra : FormBase
    {
        cFunciones fun;
        Boolean PuedeAgregarCodigoBarra;
        Boolean PuedeAgregarCodigoBarra2;
        DataTable tbDetalle;
        public FrmCompra()
        {
            InitializeComponent();
        }

        private void txtCodigoBarra_TextChanged(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            string Codigo = txtCodigoBarra.Text;
            string Nombre = "";
            cProducto prod = new cProducto();
            DataTable trdo = prod.GetProductoxCodBarra(Codigo);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodProducto"].ToString() != "")
                {
                    txtCodProducto.Text = trdo.Rows[0]["CodProducto"].ToString();
                    Nombre = trdo.Rows[0]["Nombre"].ToString();
                    txtNombre.Text = Nombre;
                    txtPrecio.Text = trdo.Rows[0]["Precio"].ToString();
                    txtCosto.Text = trdo.Rows[0]["Costo"].ToString();
                    //txtCodigo.Text = trdo.Rows[0]["Codigo"].ToString();
                    txtStock.Text = trdo.Rows[0]["stock"].ToString();
                    if (txtPrecio.Text != "")
                    {
                        txtPrecio.Text = fun.SepararDecimales(txtPrecio.Text);
                        //  txtPrecio.Text = fun.FormatoEnteroMiles(txtPrecio.Text);
                    }

                    if (txtCosto.Text != "")
                    {
                        txtCosto.Text = fun.SepararDecimales(txtCosto.Text);
                        //  txtPrecio.Text = fun.FormatoEnteroMiles(txtPrecio.Text);
                    }

                    if (trdo.Rows[0]["CodMarca"].ToString() != "")
                    {
                        string CodMarca = trdo.Rows[0]["CodMarca"].ToString();
                        cmbMarca.SelectedValue = CodMarca;
                    }

                    txtCantidad.Text = "1";
                    txtCantidad.Focus();
                    PuedeAgregarCodigoBarra = true;
                    PuedeAgregarCodigoBarra2 = false;
                }
            }
        }

        private void FrmCompra_Load(object sender, EventArgs e)
        {        
            Inicialiar();
            fun.EstiloBotones(btnGrabar);
            fun.EstiloBotones(btnCancelar);
            fun.EstiloBotones(btnAnular);
            PuedeAgregarCodigoBarra = false;
            PuedeAgregarCodigoBarra2 = false;
            BuscarUsuario();
            fun.LlenarCombo(cmbMarca, "Marca", "Nombre", "CodMarca");
            CargarNumeroCompra();
            if (Principal.CodCompra !=0)
            {
                btnGrabar.Enabled = false;
                btnCancelar.Enabled = true;
                BuscarComrpa(Principal.CodCompra);
            }
        }

        private void BuscarComrpa(Int32 CodCompra)
        {
            tbDetalle.Rows.Clear();
            cFunciones fun = new cFunciones();
            cCompra compra = new Clases.cCompra();
            cDetalleCompra detalle = new cDetalleCompra();
            DataTable trdo = compra.GetCompraxCodigo(CodCompra);
            DataTable tbDet = detalle.GetDetalle(CodCompra);
            if (trdo.Rows.Count >0)
            {
                txtTotal.Text = trdo.Rows[0]["Total"].ToString();
                txtTotal.Text = fun.SepararDecimales(txtTotal.Text);
            }

            if (tbDet.Rows.Count >0)
            {
                string CodProducto = tbDet.Rows[0]["CodProducto"].ToString();
                string Nombre = tbDet.Rows[0]["Nombre"].ToString();
                string Cantidad = tbDet.Rows[0]["Cantidad"].ToString();
                string Costo = tbDet.Rows[0]["Costo"].ToString();
                string Precio = tbDet.Rows[0]["Precio"].ToString();
                string SubTotal = tbDet.Rows[0]["Subtotal"].ToString();
                string val = CodProducto + ";" + Nombre;
                val = val + ";" + Cantidad + ";" + Costo + ";" + Precio;
                val = val + ";" + fun.SepararDecimales(SubTotal.ToString());
                tbDetalle = fun.AgregarFilas(tbDetalle, val);
                tbDetalle = fun.TablaaMiles(tbDetalle, "Costo");
                tbDetalle = fun.TablaaMiles(tbDetalle, "Precio");
                Grilla.DataSource = tbDetalle;
                fun.AnchoColumnas(Grilla, "0;40;15;15;15;15");
            }

        }

        private void CargarNumeroCompra()
        {
            int CodCompra = 0;
            cCompra Compra = new Clases.cCompra();
            CodCompra = Compra.GetMaxNumeroCompraa();
            CodCompra = CodCompra + 1;
            lBlNumeroVenta.Text = "Compra Número " + CodCompra.ToString();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (PuedeAgregarCodigoBarra == true)
                {
                    if (PuedeAgregarCodigoBarra2 == true)
                    {
                        Agregar();
                        txtCodigoBarra.Focus();
                    }
                    else
                    {
                        PuedeAgregarCodigoBarra2 = true;
                    }

                }
            }
        }

        private void Agregar()
        {
            if (txtCodProducto.Text == "")
            {
                Mensaje("Debe ingresar un producto para continuar");
                return;
            }
            if (txtPrecio.Text == "")
            {
                Mensaje("Debe ingresar un precio para continuar");
                return;
            }
            // string Col = "CodProducto;Nombre;Cantidad;Precio;SubTotal"; 

            Int32 CodProducto = Convert.ToInt32(txtCodProducto.Text);
            string Nombre = txtNombre.Text;
            string Precio = "0";
            string Costo = "0";
            string Cantidad = "1";
            Double SubTotal = 0;
            if (txtCantidad.Text != "")
            {
                Cantidad = txtCantidad.Text;
            }

            ModificarCantidad(CodProducto, Convert.ToInt32(Cantidad));
            //se utiliza por si las dudas vuelve
            // a ingresar el mismo proucto

            if (txtCantidad.Text != "")
            {
                Cantidad = txtCantidad.Text;
            }

            if (txtPrecio.Text != "")
            {
                Precio = txtPrecio.Text;
            }

            if (txtCosto.Text != "")
            {
                Costo = txtCosto.Text;
            }

            SubTotal = Convert.ToDouble(Cantidad) * Convert.ToDouble(Costo);

            string val = CodProducto + ";" + Nombre;
            val = val + ";" + Cantidad + ";" + Costo + ";" + Precio;
            val = val + ";" + fun.SepararDecimales(SubTotal.ToString());
            tbDetalle = fun.AgregarFilas(tbDetalle, val);
            Grilla.DataSource = tbDetalle;
            fun.AnchoColumnas(Grilla, "0;40;15;15;15;15");
            Double Total = fun.TotalizarColumna(tbDetalle, "SubTotal");
            txtTotal.Text = fun.SepararDecimales(Total.ToString());
            Limpiar();
            txtCodigoBarra.Focus();
            PuedeAgregarCodigoBarra = false;
            PuedeAgregarCodigoBarra2 = false;
        }

        private void Limpiar()
        {
            cmbMarca.SelectedIndex = 0;
            txtCodigo.Text = "";
            txtCodProducto.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
            txtCodigoBarra.Text = "";
            txtNombre.Text = "";
            txtStock.Text = "";
            txtCosto.Text = "";

        }

        private void ModificarCantidad(int CodProducto, int Canatidad)
        {

            int Codigo = 0;
            int stock = 0;
            int b = 0;

            for (int i = 0; i < tbDetalle.Rows.Count; i++)
            {
                Codigo = Convert.ToInt32(tbDetalle.Rows[i]["CodProducto"].ToString());
                if (Codigo == CodProducto)
                {
                    b = 1;
                    stock = Convert.ToInt32(tbDetalle.Rows[i]["Cantidad"]);
                    fun.EliminarFila(tbDetalle, "CodProducto", CodProducto.ToString());

                }
            }

            if (b == 1)
            {
                stock = stock + Canatidad;
                txtCantidad.Text = stock.ToString();
            }

        }

        private void BuscarUsuario()
        {
            int CodUsuario = Principal.CodUsuarioLogueado;
            cUsuario user = new cUsuario();
            string Nombre = user.GetNombreUsuarioxCodUsuario(CodUsuario);
            lblUsuario.Text = "Usuario " + Nombre;
        }

        private void Inicialiar()
        {
            fun = new cFunciones();
            string Col = "CodProducto;Nombre;Cantidad;Costo;Precio;SubTotal";
            tbDetalle = new DataTable();
            tbDetalle = fun.CrearTabla(Col);
            txtCodigoBarra.Focus();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (tbDetalle.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar una venta para continuar");
                return;
            }

            cCompra compra = new Clases.cCompra();
            int CodUsuario = Principal.CodUsuarioLogueado;
            cFunciones fun = new cFunciones();
            SqlTransaction Transaccion;
            SqlConnection con = new SqlConnection(cConexion.GetConexion());
            con.Open();
            Transaccion = con.BeginTransaction();
            Int32 CodCompra = 0; 
            DateTime Fecha = daFecha.Value;        
            Double Total = 0;
            if (txtTotal.Text != "")
            {
                Total = fun.ToDouble(txtTotal.Text);

            }

            try
            {           
                CodCompra  = compra.InsertarCompra(con, Transaccion, Total, Fecha, CodUsuario);
                GrabarDetalle(CodCompra, con, Transaccion);
                Transaccion.Commit();
                con.Close();
                Mensaje("Datos grabados correctamente");
                tbDetalle.Clear();
                Grilla.DataSource = tbDetalle;
                Limpiar();
                CargarNumeroCompra();
                txtTotal.Text = "";
                if (cmbMarca.SelectedIndex > 0)
                {
                    cmbMarca.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error en el proceso ");
                MessageBox.Show(ex.Message.ToString());
                Transaccion.Rollback();
                con.Close();

            }
        }

        private void GrabarDetalle(Int32 CodVenta, SqlConnection con, SqlTransaction tran)
        {
            cProducto prod = new cProducto();
            cDetalleCompra detalle = new cDetalleCompra();
            Int32 CodProducto = 0;
            Int32 Cantidad = 0;
            Double Precio = 0;
            Double Costo = 0;
            Double Subtotal = 0;
            for (int i = 0; i < tbDetalle.Rows.Count; i++)
            {
                CodProducto = Convert.ToInt32(tbDetalle.Rows[i]["CodProducto"].ToString());
                Cantidad = Convert.ToInt32(tbDetalle.Rows[i]["Cantidad"].ToString());
                Costo = fun.ToDouble(tbDetalle.Rows[i]["Costo"].ToString());
                Precio = fun.ToDouble(tbDetalle.Rows[i]["Precio"].ToString());
                Subtotal = fun.ToDouble(tbDetalle.Rows[i]["Subtotal"].ToString());
                detalle.InsertarDetalle(con, tran, CodVenta, CodProducto, Cantidad, Precio, Subtotal, Costo);
                prod.ActualizarStockTransaccion(con, tran, CodProducto,((-1) * Cantidad));
                prod.ActualizarPrecioTransaccion(con, tran, CodProducto, Precio,Costo);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Agregar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Principal.CodProoducto = 0;
            FrmBuscarProductocs frm = new SistemaBase.FrmBuscarProductocs();
            frm.FormClosing += new FormClosingEventHandler(Continuar);
            frm.ShowDialog();
        }

        private void Continuar(object sender, FormClosingEventArgs e)
        {
            if (Principal.CodProoducto != 0)
            {
                Int32 CodProducto = Convert.ToInt32(Principal.CodProoducto);
                BuscarProductoxCodigo(CodProducto);
            }
        }

        private void BuscarProductoxCodigo(Int32 CodProducto)
        {
            int b = 0;
            cFunciones fun = new cFunciones();
            cProducto prod = new Clases.cProducto();
            DataTable trdo = prod.GetProductoxCodigo(CodProducto);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodProducto"].ToString() != "")
                {
                    b = 1;
                    txtCodProducto.Text = trdo.Rows[0]["CodProducto"].ToString();
                    string Nombre = trdo.Rows[0]["Nombre"].ToString();
                    txtNombre.Text = Nombre;
                    txtPrecio.Text = trdo.Rows[0]["Precio"].ToString();
                    if (txtPrecio.Text != "")
                    {
                        txtPrecio.Text = fun.SepararDecimales(txtPrecio.Text);
                        //  txtPrecio.Text = fun.FormatoEnteroMiles(txtPrecio.Text);
                    }
                    txtCodigo.Text = trdo.Rows[0]["Codigo"].ToString();
                    txtStock.Text = trdo.Rows[0]["stock"].ToString();
                    txtCosto.Text = trdo.Rows[0]["Costo"].ToString();
                    if (txtCosto.Text != "")
                    {
                        txtCosto.Text = fun.SepararDecimales(txtCosto.Text);
                        //  txtPrecio.Text = fun.FormatoEnteroMiles(txtPrecio.Text);
                    }

                    if (trdo.Rows[0]["CodMarca"].ToString() != "")
                    {
                        string CodMarca = trdo.Rows[0]["CodMarca"].ToString();
                        cmbMarca.SelectedValue = CodMarca;
                    }

                    txtCantidad.Text = "1";
                    txtCantidad.Focus();
                    PuedeAgregarCodigoBarra = true;
                    PuedeAgregarCodigoBarra2 = true;

                    // txtCodigoBarra.Text = trdo.Rows[0]["CodigoBarra"].ToString();

                }
            }
            if (b == 1)
                txtCantidad.Focus();
        }
    }
}
