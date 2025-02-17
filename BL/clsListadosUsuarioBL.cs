using DAL;
using ENT;

namespace BL;

public class clsListadosUsuarioBL
{
        /// <summary>
        /// Metodo para obtener la lista lista de los usuario
        /// Post: Se espera una lista de usuario que no este vacia o null
        /// </summary>
        /// <returns>Devuelve una lista de usuarios</returns>
        public static List<clsUsuario> getListaUsuariosDAL()
        {
            //Llama al método de la capa DAL para obtener el listado de los usuario
            return clsListadosUsuarioDAL.getListaUsuariosDAL(); ;
        }

        /// <summary>
        /// Funcion para añadir el usuario registrado
        /// Pre: Se espera el nombre del usuario para crear el suer
        /// Post: Se espera un un usario que no este vacio o null
        /// </summary>
        /// <param name="nombre">Se pasara el nombre del usuario registrado en la pagina</param>
        /// <returns></returns>
        public static clsUsuario addUserDAL(string nombre)
        {
            //Llama al método de la capa DAL para añadir el usuario a la lista 
            return clsListadosUsuarioDAL.addUserDAL(nombre); ;
        }
}