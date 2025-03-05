using ENT;

namespace DTO;

public class WantedDTO
{
    #region Propiedades

    private clsPersonaje personaje;
    private List<clsUsuario> usuarios;

    #endregion

    #region Atributos

    public clsPersonaje Personaje
    {
        get { return personaje; }
    }

    public List<clsUsuario> Usuarios
    {
        get { return usuarios; }
    }

    #endregion

    #region Constructores

    public WantedDTO(clsPersonaje personaje, List<clsUsuario> usuarios)
    {
        this.personaje = personaje;
        this.usuarios = usuarios;
    }

    #endregion
}
