using BL;
using ENT;
using Microsoft.AspNetCore.SignalR;

namespace LuigiWantedSignalR.Hubs;

public class GameHub : Hub
{
    private static List<clsUsuario> listadoUsuarios = new List<clsUsuario>();
    private int jugadoresListos = 0;
    private clsPersonaje personajeABuscar;
    private List<clsPersonaje> listadoPersonajes;

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
        if (listadoUsuarios.Count>=2)
        {
            if (personajeABuscar == null)
            {
                personajeABuscar = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
            }
            await Clients.All.SendAsync("JuegoListo", personajeABuscar);
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
        await Clients.Caller.SendAsync("ListadoDeUsuarios", listadoUsuarios);
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

    public async Task EmpezarJuego()
    {
        jugadoresListos++;
        if (jugadoresListos >= listadoUsuarios.Count)
        {
            jugadoresListos = 0;
            await Clients.All.SendAsync("JuegoListo", listadoUsuarios.Count);
        }
    }
}