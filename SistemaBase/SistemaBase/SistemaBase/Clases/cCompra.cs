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
    }
}
