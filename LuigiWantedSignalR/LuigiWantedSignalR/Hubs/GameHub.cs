using BL;
using DTO;
using ENT;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WantedDTO = DTO.WantedDTO;

namespace LuigiWantedSignalR.Hubs;

public class GameHub : Hub
{
    private static List<clsUsuario> listadoUsuarios = new List<clsUsuario>();
    private static List<bool> listadoUsuariosListos = new List<bool>();
    private int jugadoresListos = 0;
    private clsPersonaje personajeABuscar;
    private List<clsPersonaje> listadoPersonajes = clsListadoPersonajeBL.GetListaPersonajesBL();
    private List<clsPersonaje> listadoPersonajesBuscar = new List<clsPersonaje>();
    private static bool juegoListoEnviado;
    private static bool listaPersonajesGenerado = false;

    public async Task Registrarse(string nombre)
    {
        await ComprobarUsuariosConectados();
        clsUsuario usuario = clsListadosUsuarioBL.addUserBL(nombre);
        if (!listadoUsuarios.Contains(usuario))
        {
            listadoUsuarios.Add(usuario);
        }
        await Clients.Caller.SendAsync("Registrado", usuario);
        if (listadoUsuarios.Count < 2)
        {
            juegoListoEnviado = false;
        }
        if (listadoUsuarios.Count >= 2 && !juegoListoEnviado)
        {
            if (personajeABuscar == null)
            {
                personajeABuscar = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
            }
            WantedDTO wantedDto = new WantedDTO(personajeABuscar, listadoUsuarios);
            string jsonResponse = JsonConvert.SerializeObject(wantedDto);

            Thread.Sleep(300);
            await Clients.All.SendAsync("JuegoListo", jsonResponse);
            juegoListoEnviado = true;

        }
    }

    public async Task PersonajeEncontrado()
    {
        await ComprobarUsuariosConectados();
        personajeABuscar = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
        WantedDTO wantedDto = new WantedDTO(personajeABuscar, listadoUsuarios);
        string jsonResponse = JsonConvert.SerializeObject(wantedDto);

        await Clients.All.SendAsync("EmpezarWanted", jsonResponse);
    }

    public async Task ActualizarPuntuacion(clsUsuario usuario)
    {
        clsUsuario usuarioConPuntuacionAntigua = listadoUsuarios.FirstOrDefault(u => u.Id == usuario.Id);

        if (usuarioConPuntuacionAntigua != null)
        {
            usuarioConPuntuacionAntigua.Score = usuario.Score;
        }
    }

    public async Task EmpezarBusqueda(clsPersonaje personajeABuscar, int idUsuario)
    {
        lock (listadoUsuariosListos)
        {
            listadoUsuariosListos.Add(true);
        }

        if (listadoUsuariosListos.Count >= listadoUsuarios.Distinct().ToList().Count && idUsuario == listadoUsuarios[0].Id)
        {
            clsPersonaje personaje;
            Random random = new Random();
            List<clsPersonaje> listadoPersonajesBuscar = new List<clsPersonaje>();

            // Llenar la matriz 5x5 con personajes aleatorios distintos al que buscamos
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    do
                    {
                        personaje = listadoPersonajes[random.Next(listadoPersonajes.Count)];
                    } while (personaje.Nombre.Equals(personajeABuscar.Nombre));

                    listadoPersonajesBuscar.Add(personaje);
                }
            }

            // Escoger una posición aleatoria dentro del tablero (0-24)
            int filaAleatoria = random.Next(5); // Fila entre 0 y 4
            int columnaAleatoria = random.Next(5); // Columna entre 0 y 4
            int index = (filaAleatoria * 5) + columnaAleatoria; // Convertir coordenadas (fila, columna) a índice lineal

            // Colocar el personaje a buscar en esa posición
            listadoPersonajesBuscar[index] = personajeABuscar;

            jugadoresListos = 0;
            listadoUsuariosListos = new List<bool>();

            BuscarDTO buscarDto = new BuscarDTO(personajeABuscar, listadoPersonajesBuscar);
            string jsonResponse = JsonConvert.SerializeObject(buscarDto);

            await Clients.All.SendAsync("BusquedaLista", jsonResponse);
        }
    }


    public async Task SigueConectado(clsUsuario usuario)
    {
        listadoUsuarios.Add(usuario);
    }

    public async Task ComprobarUsuariosConectados()
    {
        listadoUsuarios = new List<clsUsuario>();
        await Clients.All.SendAsync("ComprobarConexion");
        Thread.Sleep(1000);
    }
}