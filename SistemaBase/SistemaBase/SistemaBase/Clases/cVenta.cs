using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace SistemaBase.Clases
{
    public class cVenta
    {
        public Int32 InsertarVenta(SqlConnection con, SqlTransaction Transaccion, double Total, DateTime Fecha,int CodUsuario, Int32? CodCli , Double Ganancia , Double Entrega, Double Saldo)
        {
            string sql = "insert into Venta(";
            sql = sql + "Fecha,CodUsuario,Total, CodCliente, Ganancia, Entrega, Saldo)";
            sql = sql + " values (";
            sql = sql + "'" + Fecha.ToShortDateString() + "'";
            sql = sql + "," + CodUsuario.ToString();
            sql = sql + "," + Total.ToString().Replace(",", ".");
            if (CodCli !=null)
            {
                sql = sql + "," + CodCli.ToString();
            }
            else
            {
                sql = sql + ",null ";
            }   
            sql = sql + "," + Ganancia.ToString().Replace(",", ".");
            sql = sql + "," + Entrega.ToString().Replace(",", ".");
            sql = sql + "," + Saldo.ToString().Replace(",", ".");
            sql = sql + ")";
            return cDb.EjecutarEscalarTransaccion(con, Transaccion, sql);
        }

        public int GetMaxNumeroVenta()
        {
            int CodVenta = 0;
            string sql = " select max(CodVenta) as CodVenta from Venta ";
            DataTable trdo = cDb.GetDatatable(sql);
            if (trdo.Rows.Count>0)
            {
                if (trdo.Rows[0]["CodVenta"].ToString ()!="")
                {
                    CodVenta = Convert.ToInt32(trdo.Rows[0]["CodVenta"].ToString());
                }
                
            }
            return CodVenta;
        }

        public DataTable GetVentasxFecha(DateTime FechaDesde, DateTime FechaHasta, Int32? CodUsuario)
        {
            string sql = "select v.CodVenta, v.Fecha,";
            sql = sql + "(select nombre from usuario where codusuario = v.CodUsuario) as Usuario ,";
            sql = sql + "(select cli.Nombre + ' ' + cli.Apellido from Cliente cli where cli.CodCliente =v.CodCliente ) as Cliente  ";
            sql = sql + ", v.Total ,v.Ganancia ";
            sql = sql + " from Venta v ";
            sql = sql + " where v.Fecha >=" + "'" + FechaDesde.ToShortDateString() + "'";
            sql = sql + " and v.Fecha <=" + "'" + FechaHasta.ToShortDateString() + "'";
            if (CodUsuario !=null)
            {
                sql = sql + " and CodUsuario =" + CodUsuario.ToString();
            }
            sql = sql + " order by v.CodVenta desc ";
            return cDb.GetDatatable(sql);
        }

        public DataTable GetVentaResumida(DateTime FechaDesde, DateTime FechaHasta, Int32? CodUsuario)
        {
            string sql = ""; //"select v.CodVenta, v.Fecha,";
            sql = sql + "select u.Nombre ";
            sql = sql + ",sum(v.Total) as Total ";
            sql = sql + " from Venta v , usuario u ";
            sql = sql + " where v.CodUsuario = u.CodUsuario ";
            sql = sql + " and v.Fecha >=" + "'" + FechaDesde.ToShortDateString() + "'";
            sql = sql + " and v.Fecha <=" + "'" + FechaHasta.ToShortDateString() + "'";
            if (CodUsuario != null)
            {
                sql = sql + " and u.CodUsuario =" + CodUsuario.ToString();
            }
            sql = sql + " group by u.Nombre ";
            
            return cDb.GetDatatable(sql);
        }

        public DataTable GetVentaxCodigo(Int32 CodVenta)
        {
            string sql = "select * from venta ";
            sql = sql + " where CodVenta =" + CodVenta.ToString();
            return cDb.GetDatatable(sql);
        }

        public DataTable GetDeudores()
        {
            string sql = "select v.CodVenta, v.Fecha,";          
            sql = sql + "(select cli.Nombre + ' ' + cli.Apellido from Cliente cli where cli.CodCliente =v.CodCliente ) as Cliente  ";
            sql = sql + ", v.Total ,v.Ganancia, v.Entrega,v.Saldo ";
            sql = sql + " from Venta v ";
            sql = sql + " where isnull(v.Saldo,0) > 0 ";
            sql = sql + " order by v.CodVenta desc ";
            return cDb.GetDatatable(sql);
        }

        public void ActualizarSaldo(SqlConnection con, SqlTransaction Transaccion,Int32 CodVenta, Double Importe)
        {
            string sql = "update Venta ";
            sql = sql + " set Entrega = Entrega + " + Importe.ToString().Replace(",", ".");
            sql = sql + ", Saldo = Saldo - " + Importe.ToString().Replace(",", ".");
            sql = sql + " where CodVenta =" + CodVenta.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public void Anular (SqlConnection con, SqlTransaction Transaccion, Int32 CodVenta)
        {
            string sql = "delete from venta ";
            sql = sql + " where CodVenta =" + CodVenta.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

    }
}
