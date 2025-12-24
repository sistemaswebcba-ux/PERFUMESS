using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SistemaBase.Clases
{
    public  class cDetalleVentacs
    {
        public void InsertarDetalle (SqlConnection con, SqlTransaction Transaccion,Int32 CodVenta,Int32 CodProducto , int Cantidad, Double Precio , Double Subtotal , Double Costo)
        {
            string sql = "insert into DetalleVenta(";
            sql = sql + "CodVenta,CodProducto,Cantidad,Precio, Subtotal, Costo)";
            sql = sql + " values (" + CodVenta.ToString();
            sql = sql + "," + CodProducto.ToString();
            sql = sql + "," + Cantidad.ToString();
            sql = sql + "," + Precio.ToString().Replace(",", ".");
            sql = sql + "," + Subtotal.ToString().Replace(",", ".");
            sql = sql + "," + Costo.ToString().Replace(",", ".");
            sql = sql + ")";
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public DataTable GetDetallexCodVenta(int CodVenta)
        {
            string sql = "select * from DetalleVenta d, Producto p ";
            sql = sql + " where  d.CodProducto = p.CodProducto ";
            sql = sql + " and CodVenta =" + CodVenta.ToString();
            return cDb.GetDatatable(sql);
        }
        
        public void Anular(SqlConnection con, SqlTransaction Transaccion, Int32 CodVenta)
        {
            string sql = "delete from DetalleVenta ";
            sql = sql + " where CodVetna = " + CodVenta.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

    }
}
