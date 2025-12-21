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
    public partial class FrmRegistrarPago : FormBase 
    {
        public FrmRegistrarPago()
        {
            InitializeComponent();
        }

        private void FrmRegistrarPago_Load(object sender, EventArgs e)
        {
            Int32 Codventa = Convert.ToInt32(Principal.CodVenta);
            BuscarDeuda(Codventa);
            BuscarPagos(Codventa);
            cFunciones fun = new cFunciones();
            fun.EstiloBotones(btnCobrar);
        }

        private void BuscarDeuda(Int32 CodVenta)
        {
            cFunciones fun = new Clases.cFunciones();
            cVenta venta = new Clases.cVenta();
            DataTable trdo = venta.GetVentaxCodigo(CodVenta);
            if (trdo.Rows.Count >0)
            {
                Double Saldo = Convert.ToDouble(trdo.Rows[0]["Saldo"].ToString());
                txtSaldo.Text = fun.SepararDecimales(Saldo.ToString());
                
            }

        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (txtEbtrega.Text =="")
            {
                MessageBox.Show("Debe ingresar un mento ");
                return;
            }
            Int32 CodVenta = Convert.ToInt32(Principal.CodVenta);
            cFunciones fun = new cFunciones();
            Double Importe = 0;
            Double Saldo = 0;
            Importe = fun.ToDouble(txtEbtrega.Text);
            Saldo = fun.ToDouble(txtSaldo.Text);
            if (Importe > Saldo)
            {
                MessageBox.Show("El importe no puede superar el saldo");
                return;
            }

            if (Saldo==0)
            {
                MessageBox.Show("La deuda esta saldada");
                return;
            }

            cVenta Venta = new cVenta();
            cPago Pago = new Clases.cPago();
            DateTime Fecha = DateTime.Now;
            SqlTransaction Transaccion;
            SqlConnection con = new SqlConnection(cConexion.GetConexion());
            con.Open();
            Transaccion = con.BeginTransaction();
            try
            {
                Venta.ActualizarSaldo(con, Transaccion, CodVenta, Importe);
                Pago.Registrar(con, Transaccion, CodVenta, Fecha, Importe);
                Transaccion.Commit();
                con.Close();
                Mensaje("Datos grabados correctamente");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error en el proceso ");
                MessageBox.Show(ex.Message.ToString());
                Transaccion.Rollback();
                con.Close();
            }
        }

        private void BuscarPagos(Int32 CodVenta)
        {
            cFunciones fun = new cFunciones();
            cPago pago = new Clases.cPago();
            DataTable trdo = pago.GetPagosxCodVenta(CodVenta);
            trdo = fun.TablaaMiles(trdo, "Importe");
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "0;30;70");
        }
    }
}
