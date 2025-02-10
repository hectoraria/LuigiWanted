using ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class clsListadosUsuarioDAL
    {
        //Lista de Usuarios para recoger los usuario iniciados en el juego
        private static List<clsUsuario> listadoUsuarios = new List<clsUsuario>();

        /// <summary>
        /// Metodo para obtener la lista lista de los usuario
        /// Post: Se espera una lista de usuario que no este vacia o null
        /// </summary>
        /// <returns>Devuelve una lista de usuarios</returns>
        public static List<clsUsuario> getListaUsuariosDAL()
        {
            return new List<clsUsuario>(listadoUsuarios);
        }


        /// <summary>
        /// Metodo para calcular el id del proximo usuario que se registre
        ///Post: Se espera un valor mayor a 0
        /// </summary>
        /// 
        /// <returns>Devuelve un entero</returns>
        private static int obtenerUltimoIdUserDAL()
        {
            return listadoUsuarios.Any() ? listadoUsuarios.Max(u => u.Id) : 0;
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
            int nuevoId = obtenerUltimoIdUserDAL() + 1;
            clsUsuario nuevoUsuario = new clsUsuario(nuevoId, nombre);
            listadoUsuarios.Add(nuevoUsuario);
            return nuevoUsuario;
        }

    }
}
