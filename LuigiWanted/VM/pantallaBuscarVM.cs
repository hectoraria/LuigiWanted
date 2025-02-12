using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT;

namespace LuigiWanted.VM
{
    public class pantallaBuscarVM
    {
        #region Atributos
        private clsUsuario usuario;
        private List<clsPersonaje> listadoPersonajes;
        private string personajeSeleccionado;
        private string personajeCorrecto;
        private int tiempoRestante;
        #endregion

        #region Propiedades
        public List<clsPersonaje> ListadoPersonajes
        {
            get { return listadoPersonajes; }
        }

        public string PersonajeSeleccionado
        {
            set
            {
                personajeSeleccionado = value;

                //Comprobacion de que la persona seleccionada es la correcta
                if (personajeSeleccionado == personajeCorrecto)
                {
                    usuario.Score += 1;
                    //Notifico al hub que se ha descubierto al personaje
                }
                else
                {
                    usuario.Score -= 1;
                }

                //Notifico al hub  que ha cambiado la puntuacion
            }
        }

        public int TiempoRestante
        {
            get { return tiempoRestante; }
        }
        #endregion

        #region Constructores
        public pantallaBuscarVM(clsUsuario usuario, List<clsPersonaje> listadoPersonajes, string personajeCorrecto)
        {
            this.usuario = usuario;
            this.listadoPersonajes = listadoPersonajes;
            this.personajeCorrecto = personajeCorrecto;
        }
        #endregion

        #region Métodos
        
        #endregion
    }
}
