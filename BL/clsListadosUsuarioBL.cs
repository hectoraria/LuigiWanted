using ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL
{
    public class clsListadosUsuarioBL
    {

        /// <summary>
        /// Metodo para obtener la lista lista de los usuario
        /// Post: Se espera una lista de usuario que no este vacia o null
        /// </summary>
        /// <returns>Devuelve una lista de usuarios</returns>
        public static List<clsUsuario> getListaUsuariosDAL()
        {
            List<clsUsuario> lista = clsListadosUsuarioDAL.getListaUsuariosDAL();
            return lista;
        }

        /// <summary>
        /// Funcion para añadir el usuario registrado
        /// Pre:
        /// Post:
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public static clsUsuario addUserDAL(string nombre)
        {
            
            clsUsuario nuevoUsuario = clsListadosUsuarioDAL.addUserDAL(nombre);
            
            return nuevoUsuario;
        }
    }
}
