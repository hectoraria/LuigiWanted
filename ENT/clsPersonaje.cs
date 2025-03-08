namespace ENT;

public class clsPersonaje
{
    #region Atributos

    private string nombre;
    private string foto;

    #endregion

    #region Propiedades

    /// <summary>
    /// Propiedad que devuelve el nombre del personaje.
    /// </summary>
    public string Nombre
    {
        get { return nombre; }  
    }

    /// <summary>
    /// Propiedad que devuelve la foto del personaje.
    /// </summary>
    public string Foto
    {
        get { return foto; }  
    }

    #endregion

    #region Constructores

    /// <summary>
    /// Constructor que inicializa un objeto `clsPersonaje` con un nombre y una foto.
    /// </summary>
    /// <param name="nombre">El nombre del personaje.</param>
    /// <param name="foto">La URL o ruta de la foto del personaje.</param>
    public clsPersonaje(string nombre, string foto)
    {
        this.nombre = nombre;  
        this.foto = foto;  
    }

    #endregion
}
