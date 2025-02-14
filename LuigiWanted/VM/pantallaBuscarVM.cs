using ENT;

namespace LuigiWanted.VM;

public class pantallaBuscarVM
{
    #region Constructores

    public pantallaBuscarVM(clsUsuario usuario, List<clsPersonaje> listadoPersonajes, string personajeCorrecto)
    {
        this.usuario = usuario;
        this.ListadoPersonajes = listadoPersonajes;
        this.personajeCorrecto = personajeCorrecto;
    }

    #endregion

    #region Atributos

    private readonly clsUsuario usuario;
    private string personajeSeleccionado;
    private readonly string personajeCorrecto;

    #endregion

    #region Propiedades

    public List<clsPersonaje> ListadoPersonajes { get; }

    public string PersonajeSeleccionado
    {
        set
        {
            personajeSeleccionado = value;

            //Comprobacion de que la persona seleccionada es la correcta
            if (personajeSeleccionado == personajeCorrecto)
                usuario.Score += 1;
            //Notifico al hub que se ha descubierto al personaje
            else
                usuario.Score -= 1;

            //Notifico al hub  que ha cambiado la puntuacion
        }
    }

    public int TiempoRestante { get; }

    #endregion
}