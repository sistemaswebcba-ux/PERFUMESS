using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBase.Clases
{
    public class cPago
    {
        public void Registrar (SqlConnection con, SqlTransaction Transaccion, Int32 CodVenta, DateTime Fecha, Double Importe)
        {
            string sql = "insert into Pago (Importe, Fecha, CodVenta) ";
            sql = sql + " values (";
            sql = sql + Importe.ToString().Replace(",", ".");
            sql = sql + "," + "'" + Fecha.ToShortDateString() + "'";
            sql = sql + "," + CodVenta.ToString();
            sql = sql + ")";
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public DataTable GetPagosxCodVenta(Int32  CodVenta)
        {
            string sql = "select CodPago,Fecha,Importe ";
            sql = sql + " from Pago ";
            sql = sql + " where CodVenta =" + CodVenta.ToString();
            
            return cDb.GetDatatable(sql);
        }

        public void AnularPago(SqlConnection con, SqlTransaction Transaccion, Int32 CodVenta)
        {
            string sql = "delete pago where CodVenta =" + CodVenta.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }
    }
}
