using System.Text.Json.Serialization;

namespace ENT;

public class clsUsuario
{
    #region Propiedades
    // Propiedad que representa el ID del usuario
    public int Id { get; set; }

    // Propiedad que almacena el nombre del usuario
    public string Nombre { get; set; }

    // Propiedad que representa la puntuación del usuario
    public int Score { get; set; }
    #endregion

    #region Constructores
    /// <summary>
    /// Constructor que inicializa un objeto `clsUsuario` con un ID y un nombre.
    /// </summary>
    /// <param name="id">El ID único del usuario.</param>
    /// <param name="nombre">El nombre del usuario.</param>
    [JsonConstructor]
    public clsUsuario(int id, string nombre)
    {
        this.Id = id;  
        this.Nombre = nombre;  
    }

    /// <summary>
    ///  Constructor que inicializa un objeto `clsUsuario` con un nombre.
    /// </summary>
    /// <param name="nombre">El nombre del usuario.</param>
    public clsUsuario(string nombre)
    {
        this.Nombre = nombre; 
    }

    // Constructor vacío 
    public clsUsuario()
    {
    }
    #endregion
}
