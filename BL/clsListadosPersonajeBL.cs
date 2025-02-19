using DAL;
using ENT;

namespace BL;

public class clsListadoPersonajeBL
{
    /// <summary>
    ///     Obtiene la lista de personajes.
    /// </summary>
    /// <returns>Una lista de objetos clsPersonaje.</returns>
    public static List<clsPersonaje> GetListaPersonajesBL()
    {
        // Llama al método de la capa de acceso a datos para obtener la lista de personajes
        return clsListadoPersonajeDAL.GetListaPersonajesDAL();
    }

    /// <summary>
    ///     Obtiene la imagen de un personaje dado su nombre.
    /// </summary>
    /// <param name="nombre">El nombre del personaje.</param>
    /// <returns>La foto del personaje si se encuentra; de lo contrario, null.</returns>
    public static string ObtenerImagenPersonajeBL(string nombre)
    {
        // Llama al método de la capa de acceso a datos para obtener la imagen del personaje
        return clsListadoPersonajeDAL.ObtenerImagenPersonajeDAL(nombre);
    }

    /// <summary>
    ///     Obtiene un personaje aleatorio.
    /// </summary>
    /// <returns>Un personaje aleatorio.</returns>
    public static clsPersonaje ObtenerPersonajeAleatorio()
    {
        Random random = new Random();
        List<clsPersonaje> listadoPersonajes = clsListadoPersonajeDAL.GetListaPersonajesDAL();
        int index = random.Next(listadoPersonajes.Count);
        return listadoPersonajes[index];
    }
}