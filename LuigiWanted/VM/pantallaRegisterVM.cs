using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConexionBDTMAUI.VM.Utils;
using Microsoft.AspNetCore.SignalR.Client;

namespace LuigiWanted.VM
{
    public class pantallaRegisterVM : INotifyPropertyChanged
    {

        #region Atributos

        private String nombre;

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

        public DelegateCommand Enviar
        {
            get { return enviar; }
        }

        #endregion

        #region Constructor

        public pantallaRegisterVM()
        {
            //_connection = new HubConnectionBuilder().WithUrl("").Build();

            enviar = new DelegateCommand(ExecuteEnviar, CanExecuteEnviar);
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

        public void ExecuteEnviar()
        {
            
            //mensajeUser = new MensajeUsuario(grupo, usuario, mensaje);
            //await _connection.InvokeCoreAsync("SendMessage", args: new[] { mensajeUser });
            //mensaje = String.Empty;

            //NotifyPropertyChanged("Mensaje");
            

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
