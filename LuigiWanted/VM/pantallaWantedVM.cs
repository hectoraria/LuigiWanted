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
    public class pantallaWantedVM : INotifyPropertyChanged
    {
        #region Atributos

        private int puntuacionUser;
        private clsPersonaje personajeSeleccionado;
        private int tiempoSiguienteRonda;
        private List<clsUsuario> listaPuntuacion;
        private HubConnection _connection;
        #endregion

        #region Propiedades

        public clsPersonaje PersonajeSeleccionado
        {
            get { return personajeSeleccionado; }
            private set
            {
                personajeSeleccionado = value;
                NotifyPropertyChanged(nameof(personajeSeleccionado));
            }
        }

        public int TiempoSiguienteRonda
        {
            get { return tiempoSiguienteRonda; }
            set { tiempoSiguienteRonda = value; }
        }

        public List<clsUsuario> ListaPuntuacion
        {
            get { return listaPuntuacion; }
            set
            {
                listaPuntuacion = value;
                NotifyPropertyChanged(nameof(listaPuntuacion));
            }
        }

        #endregion

        #region Constructor

        public pantallaWantedVM()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://tu-servidor.com/hub")
                .Build();

            _connection.On<clsPersonaje, List<clsUsuario>>("EmpezarPantallaWanted", (personaje, usuarios) =>
            {
                personajeSeleccionado = personaje;
                listaPuntuacion = usuarios;
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
