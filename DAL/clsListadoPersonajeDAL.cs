using ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class clsListadoPersonajeDAL
    {
        private static List<clsPersonaje> listadoPersonajes = new List<clsPersonaje>
        {
            new clsPersonaje("Huesitos", "huesitos.png"),
            new clsPersonaje("Luigi", "luigi.png"),
            new clsPersonaje("Mario", "mario.png"),
            new clsPersonaje("Shy Guy", "shyguy.png"),
            new clsPersonaje("Toad", "toad.png"),
            new clsPersonaje("Waluigi", "waluigi.png"),
            new clsPersonaje("Wario", "wario.png"),
            new clsPersonaje("Yoshi", "yoshi.png")
        };

        /// <summary>
        /// Obtiene la lista de personajes.
        /// </summary>
        /// <returns>Una lista de los personajes</returns>
        public static List<clsPersonaje> GetListaPersonajesDAL()
        {
            return listadoPersonajes;
        }

        /// <summary>
        /// Obtiene la imagen de un personaje dado su nombre.
        /// </summary>
        /// <param name="nombre">El nombre del personaje.</param>
        /// <returns>La foto del personaje si se encuentra; de lo contrario, null.</returns>
        public static string ObtenerImagenPersonajeDAL(string nombre)
        {
            return listadoPersonajes.Find(p => p.Nombre == nombre).Foto;
        }
    }
}
