using ENT;

namespace DTO;

public class WantedDTO
{
    #region Propiedades

    
    private clsPersonaje personaje;
    private List<clsUsuario> usuarios;

    #endregion

    #region Atributos

    
    // Propiedad que devuelve el personaje asociado al DTO.
    public clsPersonaje Personaje
    {
        get { return personaje; }  
    }

    
    // Propiedad que devuelve la lista de usuarios relacionados con el DTO.
    public List<clsUsuario> Usuarios
    {
        get { return usuarios; } 
    }

    #endregion

    #region Constructores

    /// <summary>
    /// Constructor que inicializa un objeto `WantedDTO` con un personaje y una lista de usuarios.
    /// </summary>
    /// <param name="personaje">El personaje asociado al DTO.</param>
    /// <param name="usuarios">Lista de usuarios relacionados con el DTO.</param>
    public WantedDTO(clsPersonaje personaje, List<clsUsuario> usuarios)
    {
        this.personaje = personaje;  
        this.usuarios = usuarios;  
    }

    #endregion
}
