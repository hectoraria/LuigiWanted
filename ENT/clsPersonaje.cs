using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class clsPersonaje
    {
        #region Atributos
        private string nombre;
        private string foto;
        #endregion

        #region Propiedades
        public string Nombre
        {
            get { return nombre; }
        }

        public string Foto
        {
            get { return foto; }
        }
        #endregion

        #region Constructores
        public clsPersonaje(string nombre, string foto)
        {
            this.nombre = nombre;
            this.foto = foto;
        }
        #endregion
    }
}
