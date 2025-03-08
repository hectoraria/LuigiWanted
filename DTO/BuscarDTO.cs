using ENT;

namespace DTO;

public class BuscarDTO
{
    #region Propiedades
    
    private readonly clsPersonaje personajeCorrecto;
    private readonly List<clsPersonaje> listadoPersonajes;
    #endregion

    #region Atributos

    /// <summary>
    /// Propiedad que devuelve el personaje correcto que debe ser encontrado.
    /// </summary>
    public clsPersonaje PersonajeCorrecto
    {
        get { return personajeCorrecto; }  
    }

    /// <summary>
    /// Propiedad que devuelve la lista de personajes disponibles.
    /// </summary>
    public List<clsPersonaje> ListadoPersonajes
    {
        get { return listadoPersonajes; }  
    }

    #endregion

    #region Constructores

    /// <summary>
    /// Constructor que inicializa un objeto `BuscarDTO` con un personaje correcto y una lista de personajes.
    /// </summary>
    /// <param name="personajeCorrecto">El personaje que debe ser encontrado.</param>
    /// <param name="listadoPersonajes">Lista de personajes disponibles para elegir.</param>
    public BuscarDTO(clsPersonaje personajeCorrecto, List<clsPersonaje> listadoPersonajes)
    {
        this.personajeCorrecto = personajeCorrecto;  
        this.listadoPersonajes = listadoPersonajes;  
    }

    #endregion
}
