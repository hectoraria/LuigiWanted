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
            return clsListadosUsuarioDAL.getListaUsuariosDAL(); ;
        }

        /// <summary>
        /// Funcion para añadir el usuario registrado
        /// Pre: Se espera el nombre del usuario para crear el suer
        /// Post: Se espera un un usario que no este vacio o null
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public static clsUsuario addUserDAL(string nombre)
        {
            
            return clsListadosUsuarioDAL.addUserDAL(nombre); ;
        }
    }
}
