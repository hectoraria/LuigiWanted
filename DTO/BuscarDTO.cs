using ENT;

namespace DTO;

public class BuscarDTO
{
    #region Propiedades
    private readonly clsPersonaje personajeCorrecto;
    private readonly List<clsPersonaje> listadoPersonajes;
    #endregion

    #region Atributos
    public clsPersonaje PersonajeCorrecto
    {
        get { return personajeCorrecto; }
    }

    public List<clsPersonaje> ListadoPersonajes
    {
        get { return listadoPersonajes; }
    }
    #endregion

    #region Constructores
    public BuscarDTO(clsPersonaje personajeCorrecto, List<clsPersonaje> listadoPersonajes)
    {
        this.personajeCorrecto = personajeCorrecto;
        this.listadoPersonajes = listadoPersonajes;
    }
    #endregion
}
