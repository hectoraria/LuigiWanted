using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DTO;
using ENT;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace LuigiWanted.VM;

// Propiedades para recoger los parametros de la pagina Wanted
[QueryProperty(nameof(Buscar), "buscar")]
[QueryProperty(nameof(Usuario), "usuario")]
public class pantallaBuscarVM : INotifyPropertyChanged
{
    #region Atributos
    // Personaje seleccionado por el usuario
    private clsPersonaje personajeSeleccionado;
    // Usuario actual
    private clsUsuario usuario;
    // Personaje correcto que el usuario debe encontrar
    private clsPersonaje personajeCorrecto;
    // Lista de personajes disponibles
    private ObservableCollection<clsPersonaje> listadoPersonajes;
    // Maneja la conexión con el servidor 
    private HubConnection _connection;
    // Variable para saber si se puede seleccionar un personaje
    private bool puedeSeleccionar;
    #endregion

    #region Propiedades


    
    // Propiedad para acceder a la lista de personajes
    public ObservableCollection<clsPersonaje> ListadoPersonajes
    {
        get { return listadoPersonajes; }
    }

    // Propiedad para establecer el personaje seleccionado y comprobar si es el correcto
    public clsPersonaje PersonajeSeleccionado
    {
        set
        {
            if (puedeSeleccionar)
            {
                puedeSeleccionar = false;
                personajeSeleccionado = value;

                // Comprobación de si el personaje seleccionado es el correcto
                if (personajeSeleccionado.Nombre.Equals(personajeCorrecto.Nombre))
                {
                    usuario.Score += 1;  // Incrementar la puntuación si el personaje es correcto
                    ActulizarPuntuacion();
                    PersonajeEncontrado();
                }
                else
                {
                    usuario.Score -= 1;  // Decrementar la puntuación si el personaje es incorrecto
                    ActulizarPuntuacion();
                    Thread.Sleep(500);
                    puedeSeleccionar = true;
                }
            }
        }
    }

    // Propiedad para establecer el usuario
    public clsUsuario Usuario
    {
        set { usuario = value; }
    }

    // Propiedad para recibir los parámetros de búsqueda 
    public string Buscar
    {
        set
        {
            try
            {
                if (listadoPersonajes.Count == 0)
                {
                    // Deserializar los datos del personaje correcto y la lista de personajes
                    BuscarDTO personajeConListadoUsuario = JsonConvert.DeserializeObject<BuscarDTO>(value);
                    personajeCorrecto = personajeConListadoUsuario.PersonajeCorrecto;
                    listadoPersonajes = personajeConListadoUsuario.ListadoPersonajes;
                    NotifyPropertyChanged(nameof(ListadoPersonajes));
                    puedeSeleccionar = true;
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Error de deserialización: {ex.Message}");  
            }
        }
    }

    
    #endregion

    #region Constructores
    // Constructor que inicializa la conexión
    public pantallaBuscarVM()
    {
        listadoPersonajes = new ObservableCollection<clsPersonaje>();
        Inicializar();
    }
    #endregion

    #region Metodos

    /// <summary>
    /// Crea la conexión al Hub
    /// </summary>
    private void Inicializar()
    {
        
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7120/gamehub")  
            .Build();

        // Suscribirse al evento "EmpezarWanted" del Hub 
        _connection.On<string>("EmpezarWanted", CambiarWanted);

        StartConnection();  // Iniciar la conexión 
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
    /// Notifica a los demás usuarios que el personaje ha sido encontrado
    /// </summary>
    private async void PersonajeEncontrado()
    {
        // Llamar al método en el Hub para notificar a otros usuarios
        await _connection.InvokeAsync("PersonajeEncontrado");


        
    }

    /// <summary>
    /// Actualiza la puntuación del usuario en el servidor
    /// </summary>
    private async void ActulizarPuntuacion()
    {

        // Llamar al método en el Hub para actualizar la puntuación
        await _connection.InvokeCoreAsync("ActualizarPuntuacion", args: new[] { usuario });  

    }

    /// <summary>
    /// Cambia a la siguiente página y pasa los parámetros necesarios
    /// </summary>
    /// <param name="wantedDTO">DTO con la información para la siguiente pantalla</param>
    /// <returns></returns>
    private void CambiarWanted(string wantedDTO)
    {
        puedeSeleccionar = false;
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                listadoPersonajes = new ObservableCollection<clsPersonaje>();
                var queryParams = new Dictionary<string, object>
                {
                    { "wantedDTO", wantedDTO },
                    { "usuario", usuario },
                };

                // Navegar a la página "Wanted" con los parámetros
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

    // Método que dispara el evento de notificación de cambio de propiedad
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
