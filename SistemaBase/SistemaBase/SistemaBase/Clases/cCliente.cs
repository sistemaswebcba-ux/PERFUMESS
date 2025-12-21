using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBase.Clases
{
    public class cCliente
    {
        public DataTable GetClientexNroDoc(string NroDoc)
        {
            string sql = "";
            sql = "select * from Cliente ";
            sql = sql + " where NroDoc=" + "'" + NroDoc + "'";
            return cDb.GetDatatable(sql);
        }

        public Int32  InsertarClienteTRan (SqlConnection con, SqlTransaction Transaccion,string NroDoc, string  Apellido ,string Nombre)
        {
            string  sql ="";
            sql = "insert into Cliente (NroDoc,Apellido,Nombre)";
            sql = sql + " values (";
            sql = sql + "'" + NroDoc + "'";
            sql = sql + "," + "'" + Apellido  + "'";
            sql = sql + "," + "'" + Nombre + "'";
            sql = sql + ")";
            return cDb.EjecutarEscalarTransaccion(con, Transaccion, sql);
        }

        public DataTable GetClientexApellido(string Apellido)
        {
            string sql = "";
            sql = "select CodCliente,NroDoc, Apellido,Nombre ";
            sql = sql + " from Cliente ";
            sql = sql + " where Apellido like " + "'%" + Apellido + "%'";
            return cDb.GetDatatable(sql);
        }

        public DataTable GetClientexCodigo(Int32 CodCliente)
        {
            string sql = "";
            sql = "select * from Cliente ";
            sql = sql + " where CodCliente=" + CodCliente.ToString();
            return cDb.GetDatatable(sql);
        }

    }
}
