using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConexionBDTMAUI.VM.Utils;
using ENT;
using Microsoft.AspNetCore.SignalR.Client;

namespace LuigiWanted.VM
{
    
    public class pantallaRegisterVM : INotifyPropertyChanged
    {
        #region Atributos
        // Nombre ingresado por el usuario
        private string nombre;
        // Indica si se puede modificar el nombre
        private bool modificarNombre;
        // Almacena la información del usuario registrado
        private clsUsuario usuario;
        // Conexión con el servidor 
        private HubConnection _connection;
        // Comando para el botón de enviar
        private DelegateCommand enviar;
        // Controla si el usuario está en proceso de entrada para deshabilitar el botón
        private bool isEntrando = false;
        #endregion

        #region Propiedades
        // Propiedad para el nombre del usuario
        public string Nombre
        {
            set
            {
                nombre = value;
                // Notifica cambios en el estado del botón
                enviar.RaiseCanExecuteChanged();
            }
        }

        // Indica si se puede modificar el nombre 
        public bool ModificarNombre
        {
            get { return modificarNombre; }
        }

        // Comando para el botón enviar 
        public DelegateCommand Enviar
        {
            get { return enviar; }
        }

        // Propiedad para controlar si se está en proceso de entrada
        public bool IsEntrando
        {
            get { return isEntrando; }
            set
            {
                if (isEntrando != value)
                {
                    isEntrando = value;
                    // Notifica el cambio de la propiedad y actualiza el botón
                    NotifyPropertyChanged();
                    enviar.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Constructor
        // Constructor que inicializa los valores y configura la conexión
        public pantallaRegisterVM()
        {
            this.modificarNombre = true;
            // Configura el comando con los métodos para ejecutar y verificar si se puede ejecutar
            enviar = new DelegateCommand(ExecuteEnviar, CanExecuteEnviar);
            Inicializar();
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Inicializa la conexión con el servidor usando SignalR
        /// </summary>
        private void Inicializar()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7120/gamehub")
                .AddNewtonsoftJsonProtocol()
                .Build();

            // Suscribe al evento "Registrado" para asignar el usuario
            _connection.On<clsUsuario>("Registrado", AsignarUsuario);

            StartConnection();
        }

        /// <summary>
        /// Inicia la conexión con el servidor de forma asincrónica
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
                IsEntrando = false;  
            }
        }

        /// <summary>
        /// Cambia de pantalla y envía los datos necesarios
        /// </summary>
        private void CambiarWanted(string wantedDTO)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var queryParams = new Dictionary<string, object>
                    {
                        { "wantedDTO", wantedDTO },
                        { "usuario", usuario },
                    };

                    await Shell.Current.GoToAsync("///Wanted", queryParams);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cambiar de pantalla: {ex.Message}");
                    IsEntrando = false;  
                }
            });
        }

        /// <summary>
        /// Asigna el usuario recibido y suscribe a nuevos eventos
        /// </summary>
        private void AsignarUsuario(clsUsuario usuario)
        {
            this.usuario = usuario;
            // Suscribe al evento "ComprobarConexion" para comprobar la conexion
            _connection.On("ComprobarConexion", VerificarConexion);
            // Suscribe al evento "JuegoListo" para pasar a la pagina wanted 
            _connection.On<string>("JuegoListo", CambiarWanted);
            // Suscribe al evento "EmpezarWanted" para empezar wanted 
            _connection.On<string>("EmpezarWanted", CambiarWanted);
            _connection.Remove("Registrado");  
        }

        /// <summary>
        /// Verifica si el usuario sigue conectado
        /// </summary>
        private async void VerificarConexion()
        {
            await _connection.InvokeCoreAsync("SigueConectado", args: new[] { usuario });
        }
        #endregion

        #region Events

        /// <summary>
        /// Determina si el botón enviar puede ejecutarse
        /// </summary>
        private bool CanExecuteEnviar()
        {
            // Solo permite ejecutar si hay nombre y no se está en proceso de entrada
            return !string.IsNullOrWhiteSpace(nombre) && !IsEntrando;
        }

        /// <summary>
        /// Ejecuta el registro del usuario
        /// </summary>
        public async void ExecuteEnviar()
        {
            // Evita múltiples llamadas si ya se está procesando
            if (IsEntrando) return;  
            IsEntrando = true;       
            modificarNombre = false; 
            // Notifica lso cambios 
            NotifyPropertyChanged(nameof(ModificarNombre));

            try
            {
                // Envía el nombre del usuario al servidor para registrarse
                await _connection.InvokeCoreAsync("Registrarse", args: new[] { nombre });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrarse: {ex.Message}");
                IsEntrando = false;  
            }
        }
        #endregion

        #region Notify
        
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifica a la interfaz que una propiedad ha cambiado
        /// </summary>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
