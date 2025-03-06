using ENT;

namespace DTO;

public class BuscarDTO
{
    #region Propiedades
    private readonly string personajeCorrecto;
    private readonly List<clsPersonaje> listadoPersonajes;
    #endregion

    #region Atributos
    public string PersonajeCorrecto
    {
        get { return personajeCorrecto; }
    }

    public List<clsPersonaje> ListadoPersonajes
    {
        get { return listadoPersonajes; }
    }
    #endregion

    #region Constructores
    public BuscarDTO(string personajeCorrecto, List<clsPersonaje> listadoPersonajes)
    {
        this.personajeCorrecto = personajeCorrecto;
        this.listadoPersonajes = listadoPersonajes;
    }
    #endregion
}
