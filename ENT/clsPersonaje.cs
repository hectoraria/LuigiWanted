namespace ENT;

public class clsPersonaje
{
    #region Propiedades

    private string nombre;
    private string foto;

    #endregion

    #region Atributos

    public string Nombre
    {
        get { return nombre; }
    }

    public string Foto
    {
        get { return foto; }
    }

    #endregion

    #region Constructores

    public clsPersonaje(string nombre, string foto)
    {
        this.nombre = nombre;
        this.foto = foto;
    }

    #endregion
}