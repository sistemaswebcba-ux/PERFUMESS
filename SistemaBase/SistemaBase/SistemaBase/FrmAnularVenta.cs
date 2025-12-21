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
    public partial class FrmAnularVenta : FormBase
    {
        public FrmAnularVenta()
        {
            InitializeComponent();
        }

        private void FrmAnularVenta_Load(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Buscar()
        { 
            cFunciones fun = new cFunciones();
            DateTime FechaDesde = daFechaDesde.Value;
            DateTime FechaHasta = daFechaDesde.Value;
            cVenta venta = new Clases.cVenta();
            DataTable trdo = venta.GetVentasxFecha(FechaDesde, FechaHasta, null);
            trdo = fun.TablaaMiles (trdo, "Total");
            trdo = fun.TablaaFechas(trdo, "Fecha");
            Grilla.DataSource = trdo;
            fun.AnchoColumnas(Grilla, "20;20;40;20");
            Grilla.Columns[0].HeaderText = "Nro Venta "; 
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
           if (Grilla.CurrentRow ==null)
            {
                MessageBox.Show("Debe selecciona run elemento ");
                return;
            }

            Int32 CodVenta = Convert.ToInt32(Grilla.CurrentRow.Cells[0].Value);
            AnularVenta(CodVenta);
        }

        private void AnularVenta (Int32 CodVenta)
        {
            SqlTransaction Transaccion;
            SqlConnection con = new SqlConnection(cConexion.GetConexion());
            Int32 CodProducto = 0;
            int Cantidad = 0;
            cProducto producto = new cProducto();
            cDetalleVentacs detalle = new Clases.cDetalleVentacs();
            DataTable trdo = detalle.GetDetallexCodVenta(CodVenta);
            if (trdo.Rows.Count >0)
            {
                if (trdo.Rows[0]["CodVenta"].ToString ()!="")
                {
                    con.Open();
                    Transaccion = con.BeginTransaction();
                    try
                    {
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
