using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace SistemaBase.Clases
{
    public class cDetalleCompra
    {
        public void InsertarDetalle(SqlConnection con, SqlTransaction Transaccion, Int32 CodVenta, Int32 CodProducto, int Cantidad, Double Precio, Double Subtotal, Double Costo)
        {
            string sql = "insert into DetalleCompra(";
            sql = sql + "CodCompra,CodProducto,Cantidad,Precio, Subtotal, Costo)";
            sql = sql + " values (" + CodVenta.ToString();
            sql = sql + "," + CodProducto.ToString();
            sql = sql + "," + Cantidad.ToString();
            sql = sql + "," + Precio.ToString().Replace(",", ".");
            sql = sql + "," + Subtotal.ToString().Replace(",", ".");
            sql = sql + "," + Costo.ToString().Replace(",", ".");
            sql = sql + ")";
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public DataTable GetDetalle(Int32 CodCompra)
        {
            string sql = "";
            sql = "select p.CodProducto, p.Nombre ,";
            sql = sql + " d.cantidad,d.costo,d.precio, d.SubTotal ";
            sql = sql + " from detallecompra d, producto p ";
            sql = sql + " where d.CodProducto = p.CodProducto ";
            sql = sql + " and d.CodCompra =" + CodCompra.ToString();
            return cDb.GetDatatable(sql);
        }

    }
}
