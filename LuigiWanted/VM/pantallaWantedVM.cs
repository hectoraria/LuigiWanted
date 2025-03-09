using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BL;
using DTO;
using ENT;
using Microsoft.AspNetCore.SignalR.Client;

namespace LuigiWanted.VM
{
    // Propiedades para recoger los parametros de la pagina Register
    [QueryProperty(nameof(Wanted), "wantedDTO")]
    [QueryProperty(nameof(Usuario), "usuario")]
    public class pantallaWantedVM : INotifyPropertyChanged
    {
        #region Atributos
        // Almacena datos en formato JSON
        private string json;
        // Usuario actual
        private clsUsuario usuario;
        // Personaje que se busca
        private clsPersonaje personajeABuscar;
        // Duración total del temporizador en segundos
        private int duracionTemporizador;
        // Momento en que se inicia el temporizador
        private DateTime tiempoInicializacion;
        // Tiempo restante en el temporizador
        private int tiempoRestante;
        // Lista de usuarios conectados
        private List<clsUsuario> listadoUsuarios;
        // Maneja la conexión con el servidor 
        private HubConnection _connection;
        #endregion

        #region Propiedades
        // Propiedad que devuelve el personaje a buscar
        public clsPersonaje PersonajeABuscar
        {
            get { return personajeABuscar; }
        }

        // Propiedad que muestra el tiempo restante o un mensaje si se acaba el tiempo
        public String TiempoRestante
        {
            get
            {
                if (tiempoRestante > 0)
                {
                    return tiempoRestante.ToString();
                }
                else
                {
                    return "Esperando a los demas jugadores";
                }
            }
        }

        // Propiedad para acceder y modificar la lista de usuarios
        public List<clsUsuario> ListadoUsuarios
        {
            get { return listadoUsuarios; }
            set { listadoUsuarios = value; }
        }

        // Propiedad para asignar el personaje a buscar y la lista de usuarios desde JSON
        public WantedDTO Wanted
        {
            set
            {
                // Deserializa el JSON recibido a un objeto WantedDTO
                personajeABuscar = value.Personaje;
                listadoUsuarios = value.Usuarios;

                // Notifica cambios 
                NotifyPropertyChanged(nameof(PersonajeABuscar));
                NotifyPropertyChanged(nameof(ListadoUsuarios));

                // Configura el temporizador
                tiempoInicializacion = DateTime.Now;
                duracionTemporizador = 5;
                tiempoRestante = duracionTemporizador;

                // Inicia el temporizador
                ComenzarTemporizador();
            }
        }

        // Propiedad para asignar el usuario actual
        public clsUsuario Usuario
        {
            set { usuario = value; }
        }
        #endregion

        #region Constructor
        // Constructor que inicializa la lista de usuarios y establece la conexión
        public pantallaWantedVM()
        {
            listadoUsuarios = new List<clsUsuario>();
            Inicializar();
        }
        #endregion

        #region Metodos

        // Método asíncrono para iniciar el temporizador
        private async void ComenzarTemporizador()
        {

            while (tiempoRestante > 0)
            {
                // Calcula el tiempo transcurrido y actualiza el tiempo restante
                var elapsedTime = (DateTime.Now - tiempoInicializacion).TotalSeconds;
                tiempoRestante = (int)(duracionTemporizador - elapsedTime);


                NotifyPropertyChanged(nameof(TiempoRestante));

                await Task.Delay(1000);
            }

            await _connection.InvokeAsync("EmpezarBusqueda", personajeABuscar, usuario.Id);
        }

        // Método asíncrono para configurar la conexión con SignalR
        private async void Inicializar()
        {

            _connection = new HubConnectionBuilder()
                .WithUrl("https://luigiwantedsignalr20250309205400-hwd4ekfmaaf2e5bj.spaincentral-01.azurewebsites.net/gamehub")
                .Build();

            // Escucha el evento "BusquedaLista" 
            _connection.On<BuscarDTO>("BusquedaLista", CambiarBuscar);

            await StartConnection();
        }

        // Método asíncrono para iniciar la conexión a SignalR
        private async Task StartConnection()
        {
            try
            {
                await _connection.StartAsync();
                System.Diagnostics.Debug.WriteLine("Conexión exitosa. Estado: " + _connection.State);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error de conexión: {ex.Message}");
            }
        }

        // Método para cambiar de pantalla cuando se recibe el evento "BusquedaLista"
        private void CambiarBuscar(BuscarDTO buscarDTO)
        {
            // Ejecuta en el hilo principal para evitar conflictos con la UI
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    // Parámetros para la navegación
                    var queryParams = new Dictionary<string, object>
                    {
                        { "buscar", buscarDTO },
                        { "usuario", usuario }
                    };

                    // Navega a la pantalla "Buscar"
                    await Shell.Current.GoToAsync("///Buscar", queryParams);
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

        // Método para invocar el evento PropertyChanged y actualizar la UI
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
