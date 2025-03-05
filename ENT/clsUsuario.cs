using System.Text.Json.Serialization;

namespace ENT;

public class clsUsuario
{
    #region Propiedades
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Score { get; set; }

    #endregion

    #region Atributos

    #endregion

    #region Contructores
    [JsonConstructor]
    public clsUsuario(int id, string nombre)
    {
        this.Id = id;
        this.Nombre = nombre;
    }
    public clsUsuario(string nombre)
    {
        this.Nombre = nombre;
    }

    public clsUsuario()
    {
    }
    #endregion
}