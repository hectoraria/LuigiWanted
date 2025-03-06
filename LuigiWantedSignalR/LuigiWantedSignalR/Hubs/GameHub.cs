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
    private int jugadoresListos = 0;
    private clsPersonaje personajeABuscar;
    private List<clsPersonaje> listadoPersonajes = clsListadoPersonajeBL.GetListaPersonajesBL();
    private List<clsPersonaje> listadoPersonajesBuscar = new List<clsPersonaje>();

    public async Task Unirse(string nombre)
    {
        // Lógica para unirse al juego
        clsUsuario nuevoUsuario = new clsUsuario(nombre);
        listadoUsuarios.Add(nuevoUsuario);
        await Clients.All.SendAsync("UsuarioUnido", nuevoUsuario);
    }

    public async Task Registrarse(string nombre)
    {
        clsUsuario usuario = clsListadosUsuarioBL.addUserBL(nombre);
        listadoUsuarios.Add(usuario);
        await Clients.Caller.SendAsync("Registrado", usuario);
        if (listadoUsuarios.Count>=1)
        {
            if (personajeABuscar == null)
            {
                personajeABuscar = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
            }
            WantedDTO wantedDto = new WantedDTO(personajeABuscar, listadoUsuarios);
            string jsonResponse = JsonConvert.SerializeObject(wantedDto);

            await Clients.All.SendAsync("JuegoListo", jsonResponse);
        }
    }

    public async Task EmpezarPantallaWanted()
    {
        personajeABuscar = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
        await Clients.All.SendAsync("EmpezarPantallaWanted", personajeABuscar);
    }

    public async Task PersonajeEncontrado()
    {
        await Clients.All.SendAsync("PersonajeEncontrado");
    }

    public async Task ObtenerPuntuaciones()
    {
        await Clients.All.SendAsync("ListadoDeUsuarios", listadoUsuarios);
    }

    public async Task ActualizarPuntuacion(clsUsuario usuario)
    {
        // Find the user by ID
        clsUsuario usuarioConPuntuacionAntigua = listadoUsuarios.FirstOrDefault(u => u.Id == usuario.Id);

        if (usuarioConPuntuacionAntigua != null)
        {
            usuarioConPuntuacionAntigua.Score = usuario.Score;
        }
    }

    public async Task EmpezarBusqueda(clsPersonaje personajeABuscar)
    {
        jugadoresListos++;
        if (jugadoresListos >= 1) //listadoUsuarios.Count
        {
            clsPersonaje personaje;
            int index;
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    do
                    {
                        index = random.Next(listadoPersonajes.Count);
                        personaje = listadoPersonajes[index];
                    } while (personaje.Equals(personajeABuscar));
                    listadoPersonajesBuscar.Add(personaje);
                }
            }
            index = random.Next(listadoPersonajes.Count);
            listadoPersonajesBuscar[index] = personajeABuscar;
            jugadoresListos = 0;

            BuscarDTO buscarDto = new BuscarDTO(personajeABuscar.Nombre, listadoPersonajesBuscar);
            string jsonResponse = JsonConvert.SerializeObject(buscarDto);

            await Clients.All.SendAsync("BusquedaLista", jsonResponse);
        }
    }
}