using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBase.Clases
{
    public static class cConexion
    {
        public static string GetConexion()
        {
            //SISTEMA PERFUMES DESKTOP-TRC4UMG
           // string cadena = "Data Source=DESKTOP-TRC4UMG;Initial Catalog=SISTEMA;Integrated Security=True;TrustServerCertificate=True;";
          

            //nueva cadena de conexion   
              string cadena = "Data Source=DESKTOP-PICJCLR\\SQLEXPRESS;Initial Catalog=PERFUMES;Integrated Security=True";
          
            return cadena;
        }
    }
}
