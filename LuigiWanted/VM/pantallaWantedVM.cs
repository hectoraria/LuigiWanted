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
using Newtonsoft.Json;


namespace LuigiWanted.VM
{
    [QueryProperty(nameof(Wanted), "wantedDTO")]
    [QueryProperty(nameof(Usuario), "usuario")]
    public class pantallaWantedVM : INotifyPropertyChanged
    {
        #region Atributos
        private string json;
        private clsUsuario usuario;
        private clsPersonaje personajeABuscar;
        private int duracionTemporizador;
        private DateTime tiempoInicializacion;
        private int tiempoRestante;
        private List<clsUsuario> listadoUsuarios;
        private HubConnection _connection;
        #endregion

        #region Propiedades
        public clsPersonaje PersonajeABuscar
        {
            get { return personajeABuscar; }

        }

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
        public List<clsUsuario> ListadoUsuarios
        {
            get { return listadoUsuarios; }
            set { listadoUsuarios = value; }
        }

        public string Wanted
        {
            set
            {
                try
                {
                    WantedDTO personajeConListadoUsuario = JsonConvert.DeserializeObject<WantedDTO>(value);
                    personajeABuscar = personajeConListadoUsuario.Personaje;
                    listadoUsuarios = personajeConListadoUsuario.Usuarios;
                    NotifyPropertyChanged(nameof(PersonajeABuscar));
                    NotifyPropertyChanged(nameof(ListadoUsuarios));
                    tiempoInicializacion = DateTime.Now;
                    duracionTemporizador = 3;
                    tiempoRestante = duracionTemporizador;
                    ComenzarTemporizador();
                }
                catch (JsonSerializationException ex)
                {
                    Console.WriteLine($"Error de deserialización: {ex.Message}");
                }
            }
        }

        public clsUsuario Usuario
        {
            set { usuario = value; }
        }
        #endregion

        #region Constructor
        public pantallaWantedVM()
        {
            listadoUsuarios = new List<clsUsuario>();
            Inicializar();
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Funcion para empezar el temporizador
        /// </summary>
        private async void ComenzarTemporizador()
        {
            while (tiempoRestante > 0)
            {
                var elapsedTime = (DateTime.Now - tiempoInicializacion).TotalSeconds;
                tiempoRestante = (int)(duracionTemporizador - elapsedTime);

                NotifyPropertyChanged(nameof(TiempoRestante));

                await Task.Delay(1000);
            }

            await _connection.InvokeAsync("EmpezarBusqueda", personajeABuscar);
        }


        private async void Inicializar() // Cambiado a async void para poder usar await
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7120/gamehub")
                .Build();

            _connection.On<string>("BusquedaLista", CambiarBuscar);

            await StartConnection(); // Espera a que la conexión se complete
        }

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

        /// <summary>
        /// Funcion para pasar a la siguiente pagina
        /// </summary>
        /// <param name="personaje">Personaje aleatorio de la lista de personajes</param>
        /// <returns></returns>
        private void CambiarBuscar(string buscarDTO)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var queryParams = new Dictionary<string, object>
                    {
                        { "buscar", buscarDTO },
                        { "usuario", usuario}
                    };

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

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}