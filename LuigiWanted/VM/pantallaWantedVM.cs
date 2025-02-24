using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BL;
using ENT;
using Microsoft.AspNetCore.SignalR.Client;


namespace LuigiWanted.VM
{

    [QueryProperty(nameof(PersonajeABuscar), "personaje")]
    public class pantallaWantedVM: INotifyPropertyChanged
    {
        #region Atributos

        private int puntuacionUser;
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

        #endregion

        #region Constructor
        public pantallaWantedVM()
        {  
            listadoUsuarios = new List<clsUsuario>();
            tiempoInicializacion = DateTime.Now;
            duracionTemporizador = 3;
            tiempoRestante = duracionTemporizador;
            Inicializar();
            ComenzarTemporizador();
        }

        public pantallaWantedVM(clsPersonaje personaje)
        {
            this.personajeABuscar = personaje;
            listadoUsuarios = new List<clsUsuario>();
            tiempoInicializacion = DateTime.Now;
            duracionTemporizador = 3;
            tiempoRestante = duracionTemporizador;
            Inicializar();
            ComenzarTemporizador();
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
                var elapsedTime = (DateTime.Now - tiempoInicializacion);
                tiempoRestante = (int)(duracionTemporizador - elapsedTime.TotalMilliseconds) / 1000;

                await Task.Delay(500);
            }

            await _connection.InvokeAsync("EmpezarJuego");
        }

        /// <summary>
        /// Crea la conexion
        /// </summary>
        private void Inicializar()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7120/gamehub")
                .Build();

            _connection.On<List<clsUsuario>>("ListadoDeUsuarios", RellenarListado);

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
        /// Rellena el listado de puntuación
        /// </summary>
        /// <param name="listadoUsuarios">Listado con todos los usuarios y sus puntuacioes</param>
        private void RellenarListado(List<clsUsuario> listadoUsuarios)
        {
            this.listadoUsuarios = listadoUsuarios;
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
