namespace ENT;

public class clsPersonaje
{
    #region Constructores

    public clsPersonaje(string nombre, string foto)
    {
        this.Nombre = nombre;
        this.Foto = foto;
    }

    #endregion

    #region Atributos

    #endregion

    #region Propiedades

    public string Nombre { get; }

    public string Foto { get; }

    #endregion
}