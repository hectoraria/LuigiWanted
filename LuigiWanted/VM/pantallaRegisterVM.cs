﻿using System;
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

        private String nombre;
        private bool modificarNombre;
        private HubConnection _connection;
        private DelegateCommand enviar;

        #endregion

        #region Propiedades

        public String Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value; 
                enviar.RaiseCanExecuteChanged(); 
            }
        }

        public bool ModificarNombre
        {
            get { return modificarNombre; }
        }

        public DelegateCommand Enviar
        {
            get { return enviar; }
        }

        #endregion

        #region Constructor

        public pantallaRegisterVM()
        {
            this.modificarNombre = true;
            enviar = new DelegateCommand(ExecuteEnviar, CanExecuteEnviar);
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

            _connection.On<clsPersonaje>("JuegoListo", CambiarWanted);

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
        /// Funcion para pasar a la siguiente pagina y mandar el personaje a buscar
        /// </summary>
        /// <param name="personaje">Personaje aleatorio de la lista de personajes</param>
        /// <returns></returns>
        private async void CambiarWanted(clsPersonaje personaje)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var queryParams = new Dictionary<string, object>
                    {
                        { "personaje", personaje }
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

        #region Events

        private bool CanExecuteEnviar()
        {
            bool canExecute = false;

            if (nombre != null && nombre!="" )
            {
                canExecute = true;
            }

            return canExecute;
        }

        public async void ExecuteEnviar()
        {
            this.modificarNombre = false;
            await _connection.InvokeCoreAsync("Registrarse", args: new[] { nombre });

            NotifyPropertyChanged(nameof(ModificarNombre));

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
