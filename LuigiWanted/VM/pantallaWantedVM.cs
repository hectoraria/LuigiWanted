using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT;


namespace LuigiWanted.VM
{
    public class pantallaWantedVM
    {
        #region Atributos

        private int tiempoSiguienteRonda;
        private List<clsUsuario> listaPuntuacion;

        #endregion

        #region Propiedades

        public int TiempoSiguienteRonda
        {
            get { return tiempoSiguienteRonda; }
            set { tiempoSiguienteRonda = value; }
        }

        public List<clsUsuario> ListaPuntuacion
        {
            get { return listaPuntuacion; }
            set { listaPuntuacion = value; }
        }

        #endregion

        #region Constructor

        public pantallaWantedVM()
        {
            listaPuntuacion = new List<clsUsuario>();
        }

        #endregion




    }
}
