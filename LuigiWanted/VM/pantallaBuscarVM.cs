using System.ComponentModel;
using System.Runtime.CompilerServices;
using DTO;
using ENT;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace LuigiWanted.VM;

[QueryProperty(nameof(Buscar), "buscar")]
[QueryProperty(nameof(Usuario), "usuario")]
public class pantallaBuscarVM: INotifyPropertyChanged
{
    #region Atributos
    private clsPersonaje personajeSeleccionado;
    private clsUsuario usuario;
    private clsPersonaje personajeCorrecto;
    private List<clsPersonaje> listadoPersonajes;
    private HubConnection _connection;
    #endregion

    #region Propiedades

    public List<clsPersonaje> ListadoPersonajes
    {
        get { return listadoPersonajes; }
    }

    public clsPersonaje PersonajeSeleccionado
    {
        set
        {
            personajeSeleccionado = value;

            //Comprobacion de que la persona seleccionada es la correcta
            if (personajeSeleccionado.Nombre.Equals(personajeCorrecto.Nombre))
            {
                usuario.Score += 1;
                ActulizarPuntuacion();
                PersonajeEncontrado();
            }

            else
            {
                usuario.Score -= 1;
                ActulizarPuntuacion();
            }
        }
    }

    public clsUsuario Usuario
    {
        set { usuario = value; }
    }

    public string Buscar
    {
        set
        {
            try
            {
                BuscarDTO personajeConListadoUsuario = JsonConvert.DeserializeObject<BuscarDTO>(value);
                personajeCorrecto = personajeConListadoUsuario.PersonajeCorrecto;
                listadoPersonajes = personajeConListadoUsuario.ListadoPersonajes;
                NotifyPropertyChanged(nameof(ListadoPersonajes));
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error de deserialización: {ex.Message}");
            }
        }
    }

    #endregion

    #region Constructores
    public pantallaBuscarVM()
    {
        Inicializar();
    }
    #endregion

    #region Metodos

    /// <summary>
    /// Crea la conexion
    /// </summary>
    private void Inicializar()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7120/gamehub")
            .Build();

        _connection.On<string>("PersonajeEncontrado", CambiarWanted);

        StartConnection();
    }

    /// <summary>
    /// Empieza la conexión con el Hub
    /// </summary>
    private async void StartConnection()
    {
        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al iniciar la conexión: {ex.Message}");
        }
    }

    /// <summary>
    /// Funcion para notificar al resto de usuarios que le usuario ha sido encontrado para que no sigan buscando
    /// </summary>
    private async void PersonajeEncontrado()
    {
        await _connection.InvokeAsync("PersonajeEncontrado");
    }

    /// <summary>
    /// Funcion para actualizar la puntuacion del usuario
    /// </summary>
    private async void ActulizarPuntuacion()
    {
        await _connection.InvokeCoreAsync("ActualizarPuntuacion", args: new[] { usuario });
    }

    /// <summary>
    /// Funcion para pasar a la siguiente pagina y mandar el personaje a buscar
    /// </summary>
    /// <param name="personaje">Personaje aleatorio de la lista de personajes</param>
    /// <returns></returns>
    private void CambiarWanted(string wantedDTO)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var queryParams = new Dictionary<string, object>
                {
                    { "personajeConListadoUsuario", wantedDTO },
                    { "usuario", usuario },
                };

                await Shell.Current.GoToAsync("///Wanted", queryParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar de pantalla: {ex.Message}");
            }
        });
    }
    #endregion

    #region Notify
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}