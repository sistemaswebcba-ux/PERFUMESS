using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SistemaBase.Clases
{
    public class cCompra
    {
        public int GetMaxNumeroCompraa()
        {
            int CodCompra = 0;
            string sql = " select max(CodCompra) as CodCompra from Compra ";
            DataTable trdo = cDb.GetDatatable(sql);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodCompra"].ToString() != "")
                {
                    CodCompra = Convert.ToInt32(trdo.Rows[0]["CodCompra"].ToString());
                }

            }
            return CodCompra;
        }

        public Int32 InsertarCompra(SqlConnection con, SqlTransaction Transaccion, double Total, DateTime Fecha, int CodUsuario)
        {
            string sql = "insert into Compra(";
            sql = sql + "Fecha,CodUsuario,Total)";
            sql = sql + " values (";
            sql = sql + "'" + Fecha.ToShortDateString() + "'";
            sql = sql + "," + CodUsuario.ToString();
            sql = sql + "," + Total.ToString().Replace(",", ".");
            sql = sql + ")";
            return cDb.EjecutarEscalarTransaccion(con, Transaccion, sql);
        }

        public DataTable GetComprassxFecha(DateTime FechaDesde, DateTime FechaHasta)
        {
            string sql = "select c.CodCompra, c.Fecha,";
            sql = sql + "(select nombre from usuario where codusuario = c.CodUsuario) as Usuario ";
            sql = sql + ", c.Total  ";
            sql = sql + " from Compra c ";
            sql = sql + " where c.Fecha >=" + "'" + FechaDesde.ToShortDateString() + "'";
            sql = sql + " and c.Fecha <=" + "'" + FechaHasta.ToShortDateString() + "'";
            sql = sql + " order by c.CodCompra desc ";
            return cDb.GetDatatable(sql);
        }

        public DataTable GetCompraxCodigo(Int32 CodCompra)
        {
            string sql = "select * from compra c ";
            sql = sql + " inner join Usuario u on c.CodUsuario =u.CodUsuario ";
            sql = sql + " where CodCompra =" + CodCompra.ToString();
            DataTable trdo = cDb.GetDatatable(sql);
            return trdo;
        }
    }
}
