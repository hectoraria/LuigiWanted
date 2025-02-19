namespace ENT;

public class clsUsuario
{
    #region Contructores
    public clsUsuario(int id, string nombre)
    {
        this.Id = id;
        this.Nombre = nombre;
    }
    public clsUsuario(string nombre)
    {
        this.Nombre = nombre;
    }
    #endregion

    #region Atributos

    #endregion

    #region Propiedades

    public int Id { get; }

    public string Nombre { get; }

    public int Score { get; set; }

    #endregion
}