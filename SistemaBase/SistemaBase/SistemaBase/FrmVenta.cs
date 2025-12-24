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
    public partial class FrmVenta : FormBase 
    {
        cFunciones fun;
        Boolean PuedeAgregarCodigoBarra;
        Boolean PuedeAgregarCodigoBarra2;
        DataTable tbDetalle;
        public FrmVenta()
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

            ModificarCantidad(CodProducto,Convert.ToInt32 (Cantidad));
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

            if (txtCosto.Text !="")
            {
                Costo = txtCosto.Text;
            }

            SubTotal = Convert.ToDouble(Cantidad) * Convert.ToDouble(Precio);

            string val = CodProducto + ";"+ Nombre;
            val = val + ";" + Cantidad + ";"  + Costo + ";" + Precio;
            val = val + ";" + fun.SepararDecimales (SubTotal.ToString ());
            tbDetalle = fun.AgregarFilas(tbDetalle, val);
            Grilla.DataSource = tbDetalle;
            fun.AnchoColumnas(Grilla, "0;40;15;15;15;15");
            Double Total = fun.TotalizarColumna(tbDetalle, "SubTotal");
            txtTotal.Text = fun.SepararDecimales (Total.ToString());
            Limpiar();
            txtCodigoBarra.Focus();
            PuedeAgregarCodigoBarra = false;
            PuedeAgregarCodigoBarra2 = false;
        }

        private void Limpiar()
        {
            txtCodigo.Text = "";
            txtCodProducto.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
            txtCodigoBarra.Text = "";
            txtNombre.Text ="";
            txtStock.Text = "";
            txtCosto.Text = "";
            
        }

        private void ModificarCantidad(int CodProducto, int Canatidad)
        {
            
            int Codigo = 0;
            int stock = 0;
            int b = 0;
            
            for (int i = 0; i < tbDetalle.Rows.Count ; i++)
            {
                Codigo = Convert.ToInt32(tbDetalle.Rows[i]["CodProducto"].ToString());
                if (Codigo == CodProducto)
                {
                    b = 1;
                    stock =Convert.ToInt32 (tbDetalle.Rows[i]["Cantidad"]);
                    fun.EliminarFila(tbDetalle, "CodProducto", CodProducto.ToString());
                   
                }
            }

            if (b ==1)
            {
                stock = stock + Canatidad;
                txtCantidad.Text = stock.ToString();        
            }
            
        }

        private void Inicialiar()
        {         
            fun = new cFunciones();
            string Col = "CodProducto;Nombre;Cantidad;Costo;Precio;SubTotal";
            tbDetalle = new DataTable();
            tbDetalle = fun.CrearTabla(Col);
            txtCodigoBarra.Focus();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {        
            Agregar();
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            Inicialiar();
            CargarNumeroVenta();
            fun.EstiloBotones(btnGrabar);
            fun.EstiloBotones(btnCancelar);
            fun.EstiloBotones(btnAnular);
            PuedeAgregarCodigoBarra = false;
            PuedeAgregarCodigoBarra2 = false;
            BuscarUsuario();
            txtApellido.Enabled = false;
            txtNombreCliente.Enabled = false;
            txtNroDoc.Enabled = false;        
            fun.LlenarCombo(cmbMarca, "Marca", "Nombre", "CodMarca");
            if (Principal.CodVenta !=0)
            {
                Int32 CodVenta = Convert.ToInt32(Principal.CodVenta);
                BuscarVenta(CodVenta);
            }
        }

        private void CargarNumeroVenta()
        {
            int CodVenta = 0;
            cVenta venta = new Clases.cVenta();
            CodVenta = venta.GetMaxNumeroVenta();
            CodVenta = CodVenta + 1;
            lBlNumeroVenta.Text = "Venta Número " + CodVenta.ToString();
        }

        private void BuscarUsuario()
        {
            int CodUsuario = Principal.CodUsuarioLogueado;
            cUsuario user = new cUsuario();
            string Nombre = user.GetNombreUsuarioxCodUsuario(CodUsuario);
            lblUsuario.Text = "Usuario " + Nombre;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (Grilla.CurrentRow == null)
            {
                Mensaje("Debe seleccionar un elelemnto");
                return;
            }
            string CodProducto = Grilla.CurrentRow.Cells[0].Value.ToString();
            tbDetalle = fun.EliminarFila(tbDetalle, "CodProducto", CodProducto);
            Grilla.DataSource = tbDetalle;
            Double Total = fun.TotalizarColumna(tbDetalle, "Precio");
            txtTotal.Text = Total.ToString();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (PuedeAgregarCodigoBarra == true)
                {
                    if (PuedeAgregarCodigoBarra2 ==true)
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

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (chkCliente.Checked ==true)
            {
                if (txtNroDoc.Text =="")
                {
                    MessageBox.Show("Debe ingresar un numero de documento para continuar");
                    return;
                }

                if (txtApellido.Text  == "")
                {
                    MessageBox.Show("Debe ingresar un apellido para continuar");
                    return;
                }
                  
                if (txtNombreCliente.Text == "")
                {
                    MessageBox.Show("Debe ingresar un nombre para continuar");
                    return;
                }
            }
            if (tbDetalle.Rows.Count <1)
            {
                MessageBox.Show("Debe ingresar una venta para continuar");
                return;
            }

            if (txtEbtrega.Text =="")
            {
                MessageBox.Show("Debe registrar una entrega para continuar");
                return;
            }

            int CodUsuario =  Principal.CodUsuarioLogueado;
            cFunciones fun = new cFunciones();
            SqlTransaction Transaccion;
            SqlConnection con = new SqlConnection(cConexion.GetConexion());
            con.Open();
            Transaccion = con.BeginTransaction();
            
            Int32 CodVenta = 0;
            Int32 CodCliente = 0;
            Int32? CodCli = null;
            cCliente cli = new cCliente();
            DateTime Fecha = daFecha.Value;
            cVenta venta = new Clases.cVenta();
            Double Total = 0;
            Double Entrega = 0;
            Double Ganancia = 0;
            Double Saldo = 0;
            Ganancia = CalcularGanancia();
            if(txtTotal.Text !="")
            {
                Total = fun.ToDouble (txtTotal.Text);
                
            }
               
            if (txtEbtrega.Text != "")
            {
                Entrega  = fun.ToDouble(txtEbtrega.Text);
            }
             
            if (txtSaldo.Text != "")
            {
                Saldo = fun.ToDouble(txtSaldo.Text);
            }

            if (Entrega > Total)
            {
                MessageBox.Show("La entrega no puede superar al total ");
                return;
            }

            try
            {
                if (chkCliente.Checked == true)
                {
                    if (txtCodCliente.Text =="")
                    {
                        CodCliente = cli.InsertarClienteTRan(con, Transaccion, txtNroDoc.Text, txtApellido.Text, txtNombreCliente.Text);
                        CodCli = Convert.ToInt32(CodCliente);
                    }
                    else
                    {
                        CodCli = Convert.ToInt32(txtCodCliente.Text);
                    }
                   
                }
                CodVenta = venta.InsertarVenta(con, Transaccion, Total, Fecha, CodUsuario, CodCli , Ganancia, Entrega ,Saldo );
               
                GrabarDetalle(CodVenta, con, Transaccion);
                Transaccion.Commit();
                con.Close();
                Mensaje("Datos grabados correctamente");
                tbDetalle.Clear();
                Grilla.DataSource = tbDetalle;
                Limpiar();
                CargarNumeroVenta();
                txtTotal.Text = "";
                txtSaldo.Text = "";
                txtEbtrega.Text = ""; 
                txtApellido.Text = "";
                txtNombreCliente.Text = "";
                txtCodCliente.Text = "";
                txtNroDoc.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error en el proceso ");
                MessageBox.Show(ex.Message.ToString());
                Transaccion.Rollback();
                con.Close();

            }

        }

        private void GrabarDetalle(Int32 CodVenta, SqlConnection con , SqlTransaction tran)
        {
            cProducto prod = new cProducto();
            cDetalleVentacs detalle = new cDetalleVentacs();
            Int32 CodProducto = 0;
            Int32 Cantidad = 0;
            Double Precio = 0;
            Double Costo = 0;
            Double Subtotal = 0;
            for (int i = 0; i < tbDetalle.Rows.Count; i++)
            {
                CodProducto = Convert.ToInt32 (tbDetalle.Rows[i]["CodProducto"].ToString());
                Cantidad = Convert.ToInt32(tbDetalle.Rows[i]["Cantidad"].ToString());
                Costo = fun.ToDouble(tbDetalle.Rows[i]["Costo"].ToString());
                Precio = fun.ToDouble(tbDetalle.Rows[i]["Precio"].ToString());

                Subtotal = fun.ToDouble(tbDetalle.Rows[i]["Subtotal"].ToString());
                detalle.InsertarDetalle(con, tran, CodVenta, CodProducto, Cantidad, Precio, Subtotal,Costo);
                prod.ActualizarStockTransaccion(con, tran, CodProducto, Cantidad);
            }
        }

        private void txtCodigoBarra_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (PuedeAgregarCodigoBarra == true)
                {
                    if (PuedeAgregarCodigoBarra2 == true)
                    {
                        Agregar();
                    }
                    else
                    {
                        PuedeAgregarCodigoBarra2 = true;
                    }
                }

                txtCodigo.Focus();
            }
            */
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            string Codigo = txtCodigo.Text;
            string Nombre = "";
            cProducto prod = new cProducto();
            DataTable trdo = prod.GetProductoxCodIGOInterno(Codigo);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodProducto"].ToString() != "")
                {
                    txtCodProducto.Text = trdo.Rows[0]["CodProducto"].ToString();
                    Nombre = trdo.Rows[0]["Nombre"].ToString();
                    txtNombre.Text = Nombre;
                    txtPrecio.Text = trdo.Rows[0]["Precio"].ToString();
                    txtCodigo.Text = trdo.Rows[0]["Codigo"].ToString();
                    txtStock.Text = trdo.Rows[0]["stock"].ToString();
                    if (txtPrecio.Text != "")
                    {
                        txtPrecio.Text = fun.SepararDecimales(txtPrecio.Text);
                        //  txtPrecio.Text = fun.FormatoEnteroMiles(txtPrecio.Text);
                    }
                    txtCantidad.Text = "1";
                    txtCantidad.Focus();
                    PuedeAgregarCodigoBarra = true;
                    PuedeAgregarCodigoBarra2 = true;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
            tbDetalle.Clear();
            Grilla.DataSource = tbDetalle;
            if (Principal.CodVenta!=0)
            {
                this.Close();
            }
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

        private void ContinuarCliente(object sender, FormClosingEventArgs e)
        {
            if (Principal.CodCliente != 0)
            {
                Int32 CodCliente = Convert.ToInt32(Principal.CodCliente);
                cCliente cli = new Clases.cCliente();
                DataTable trdo = cli.GetClientexCodigo(CodCliente);
                if (trdo.Rows.Count >0)
                {
                    if (trdo.Rows[0]["CodCliente"].ToString ()!="")
                    {
                        txtCodCliente.Text = trdo.Rows[0]["CodCliente"].ToString();
                        txtNombreCliente.Text = trdo.Rows[0]["Nombre"].ToString();
                        txtApellido.Text = trdo.Rows[0]["Apellido"].ToString();
                        txtNroDoc.Text = trdo.Rows[0]["NroDoc"].ToString();
                    }
                }
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
                    // txtCodigoBarra.Text = trdo.Rows[0]["CodigoBarra"].ToString();
                    
                }
            }
            if (b == 1)
                txtCantidad.Focus();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            FrmAnularVenta frm = new SistemaBase.FrmAnularVenta();
            frm.ShowDialog();
        }

        private void chkCliente_Click(object sender, EventArgs e)
        {
           
        }

        private void chkCliente_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCliente.Checked == true)
            {
                txtApellido.Enabled = true;
                txtNombreCliente.Enabled = true;
                txtNroDoc.Enabled = true;
                btnBuscarCliente.Enabled = true;
            }
            else
            {
                txtApellido.Enabled = false;
                txtNombreCliente.Enabled = false;
                txtNroDoc.Enabled = false;
                txtApellido.Text = "";
                txtNombreCliente.Text = "";
                txtNroDoc.Text = "";
                txtCodCliente.Text = "";
                btnBuscarCliente.Enabled = false;
            }
        }

        private void txtNroDoc_TextChanged(object sender, EventArgs e)
        {
            int b = 0;
            cCliente cli = new Clases.cCliente();
            string NroDoc = txtNroDoc.Text;
            DataTable trdo = cli.GetClientexNroDoc(NroDoc);
            if (trdo.Rows.Count >0)
            {
                if (trdo.Rows[0]["CodCliente"].ToString ()!="")
                {
                    b = 1;
                    txtNombreCliente.Text = trdo.Rows[0]["Nombre"].ToString(); 
                    txtApellido.Text = trdo.Rows[0]["Apellido"].ToString();
                    txtCodCliente.Text = trdo.Rows[0]["CodCliente"].ToString();
                }
            }
            if (b ==0)
            {
                txtNombreCliente.Text = "";
                txtApellido.Text = "";
                txtCodCliente.Text = "";
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            Principal.CodCliente = 0;
            FrmBuscadorCliente  frm = new SistemaBase.FrmBuscadorCliente();
            frm.FormClosing += new FormClosingEventHandler(ContinuarCliente);
            frm.ShowDialog();
        }

        public Double  CalcularGanancia()
        {
            Double Ganancia = 0;
            Double GananciaTotal = 0;
            Double Costo = 0;
            Double Precio = 0;
            int Cantidad = 0;
            cFunciones fun = new cFunciones();
            for (int i = 0; i < tbDetalle.Rows.Count ; i++)
            {
                Cantidad = Convert.ToInt32(tbDetalle.Rows[i]["Cantidad"].ToString());
                Costo = fun.ToDouble(tbDetalle.Rows[i]["Costo"].ToString());
                Precio = fun.ToDouble(tbDetalle.Rows[i]["Precio"].ToString());
                Ganancia = Cantidad * (Precio - Costo);
                GananciaTotal = GananciaTotal + Ganancia;
            }
            return GananciaTotal;
        }

        private void BuscarVenta (Int32 CodVenta)
        {
            cVenta venta = new Clases.cVenta();
            DataTable trdo = venta.GetVentaxCodigo(CodVenta);
            if (trdo.Rows.Count >0)
            {
                if (trdo.Rows[0]["CodCliente"].ToString ()!="")
                {
                    Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString());
                    cCliente cli = new cCliente();
                    DataTable tbcli = cli.GetClientexCodigo(CodCliente);
                    if (tbcli.Rows.Count >0)
                    {
                        txtCodCliente.Text = tbcli.Rows[0]["CodCliente"].ToString();
                        txtNombreCliente.Text = tbcli.Rows[0]["Nombre"].ToString();
                        txtApellido.Text = tbcli.Rows[0]["Apellido"].ToString();
                        txtNroDoc.Text = tbcli.Rows[0]["NroDoc"].ToString();
                    }
                }

                txtTotal.Text = trdo.Rows[0]["Total"].ToString();
                txtEbtrega.Text = trdo.Rows[0]["Entrega"].ToString();
                txtSaldo.Text = trdo.Rows[0]["Saldo"].ToString();
                txtTotal.Text = fun.SepararDecimales(txtTotal.Text);
                txtEbtrega.Text = fun.SepararDecimales(txtEbtrega.Text);
                txtSaldo.Text = fun.SepararDecimales(txtSaldo.Text);
                CargarDetalleVenta(CodVenta);
            }
           
        }

        private void CargarDetalleVenta(Int32 CodVenta)
        {
            tbDetalle.Rows.Clear();
            cDetalleVentacs det = new Clases.cDetalleVentacs();
            DataTable trdo = det.GetDetallexCodVenta(CodVenta);
            string Nombre = "";
            string Cantidad = "";
            string Precio = "";
            string Costo = "";
            string Subtotal = "";
            string CodProducto = "";
            string Val = "";
            cFunciones fun = new cFunciones();
            if (trdo.Rows.Count >0)
            {
                CodProducto = trdo.Rows[0]["CodProducto"].ToString();
                Nombre = trdo.Rows[0]["Nombre"].ToString();
                Cantidad = trdo.Rows[0]["Cantidad"].ToString();
                Precio = trdo.Rows[0]["Precio"].ToString();
                Costo = trdo.Rows[0]["Costo"].ToString();
                Subtotal = trdo.Rows[0]["Subtotal"].ToString();
                Val = CodProducto + ";" + Nombre;
                Val = Val + ";" + Cantidad + ";" + Costo + ";" + Precio;
                Val = Val + ";" + Subtotal;
                tbDetalle = fun.AgregarFilas(tbDetalle, Val);
            }
            tbDetalle = fun.TablaaMiles(tbDetalle, "Sustotal");
            tbDetalle = fun.TablaaMiles(tbDetalle, "Costo");
            tbDetalle = fun.TablaaMiles(tbDetalle, "Precio");

            Grilla.DataSource = tbDetalle;
            fun.AnchoColumnas(Grilla, "0;40;15;15;15;15");
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;
            btnGrabar.Enabled = false;
            
        }

        private void txtEbtrega_Leave(object sender, EventArgs e)
        {
            cFunciones fun = new Clases.cFunciones();
            Double Total = 0;
            Double Entrega = 0;
            Double Saldo = 0;
            Total = fun.ToDouble(txtTotal.Text);
            Entrega = fun.ToDouble(txtEbtrega.Text);
            Saldo = Total - Entrega;
            txtSaldo.Text = Saldo.ToString();
            txtSaldo.Text = fun.SepararDecimales(Saldo.ToString());
            txtEbtrega.Text = fun.SepararDecimales(Entrega.ToString());
        }

        private void AnularVenta(Int32 CodVenta)
        {
            SqlTransaction Transaccion;
            SqlConnection con = new SqlConnection(cConexion.GetConexion());
            Int32 CodProducto = 0;
            int Cantidad = 0;
            cProducto producto = new cProducto();
            cPago pago = new cPago();
            cDetalleVentacs detalle = new Clases.cDetalleVentacs();
            DataTable trdo = detalle.GetDetallexCodVenta(CodVenta);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodVenta"].ToString() != "")
                {
                    con.Open();
                    Transaccion = con.BeginTransaction();
                    try
                    {
                        pago.AnularPago(con, Transaccion, CodVenta);
                        for (int i = 0; i < trdo.Rows.Count; i++)
                        {
                            CodProducto = Convert.ToInt32(trdo.Rows[i]["CodProducto"].ToString());
                            Cantidad = Convert.ToInt32(trdo.Rows[i]["Cantidad"].ToString());
                            producto.ActualizarStockTransaccionSuma(con, Transaccion, CodProducto, Cantidad);
                            //falta borrar la venta y etalle
                        }
                        Transaccion.Commit();
                        con.Close();
                        Mensaje("Venta anulada correctamente");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hubo un error en el proceso ");
                        MessageBox.Show(ex.Message.ToString());
                        Transaccion.Rollback();
                        con.Close(); ;
                    }

                }
            }
        }
    }
}
