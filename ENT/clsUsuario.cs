using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENT
{
    public class clsUsuario
    {
        #region Atributos

        private int id;

        private string nombre;

        private int score;

        #endregion

        #region Propiedades

        public int Id
        {
            get { return id; }
        }

        public string Nombre
        {

            get { return nombre; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        #endregion

        #region Contructores

        public clsUsuario(int id,string nombre)
        {
            this.id=id;
            this.nombre=nombre;
        }

        #endregion
    }
}
