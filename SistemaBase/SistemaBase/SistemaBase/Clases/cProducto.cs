using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBase.Clases
{
    public class cProducto
    {
        public DataTable GetProductoxCodBarra(string CodigoBarra)
        {
            string sql = " select * from Producto ";
            sql = sql + " where CodigoBarra=" + "'" + CodigoBarra + "'";
            DataTable trdo = cDb.GetDatatable(sql);
            return trdo;
        }

        public DataTable GetProductoxCodigo(Int32 CodProducto)
        {
            string sql = " select * from Producto ";
            sql = sql + " where CodProducto=" + "'" + CodProducto + "'";
            DataTable trdo = cDb.GetDatatable(sql);
            return trdo;
        }

        public void ActualizarPoroducto(int CodProducto, Double Precio)
        {
            string sql = "update Producto ";
            sql = sql + " set Precio =" + Precio.ToString().Replace(",", ".");
            sql = sql + " where CodProducto =" + CodProducto.ToString();
            cDb.Grabar(sql);
        }

        public void ActualizarCosto(int CodProducto, Double Costo)
        {
            string sql = "update Producto ";
            sql = sql + " set Costo =" + Costo.ToString().Replace(",", ".");
            sql = sql + " where CodProducto =" + CodProducto.ToString();
            cDb.Grabar(sql);
        }

        public void ActualizarStock(int CodProducto ,int Cantidad)
        {
            string sql = "";
            sql = "update Producto ";
            sql = sql + " set Stock = isnull(stock,0) + " + Cantidad.ToString();
            sql = sql + " where CodProducto =" + CodProducto.ToString();
            cDb.Grabar(sql);
        }

        public DataTable GetProductoxNombre(string Nombre)
        {
            string sql = "select p.CodProducto,p.Nombre,";
            sql = sql + "p.Precio,p.Stock,p.Codigo ";
            sql = sql + " from Producto p ";
            if (Nombre !="")
            {
                sql = sql + " where p.Nombre like " + "'%" + Nombre + "%'";
            }
            return cDb.GetDatatable(sql);
        }

        public DataTable GetProductoxCodIGOInterno(string Codigo)
        {
            string sql = " select * from Producto ";
            sql = sql + " where Codigo=" + "'" + Codigo + "'";
            DataTable trdo = cDb.GetDatatable(sql);
            return trdo;
        }

        public void ActualizarStockTransaccion(SqlConnection con, SqlTransaction Transaccion, int CodProducto, int Cantidad)
        {
            string sql = "";
            sql = "update Producto ";
            sql = sql + " set Stock = isnull(stock,0) - " + Cantidad.ToString();
            sql = sql + " where CodProducto =" + CodProducto.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public void ActualizarStockTransaccionSuma(SqlConnection con, SqlTransaction Transaccion, int CodProducto, int Cantidad)
        {
            string sql = "";
            sql = "update Producto ";
            sql = sql + " set Stock = isnull(stock,0) + " + Cantidad.ToString();
            sql = sql + " where CodProducto =" + CodProducto.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

    }
}
