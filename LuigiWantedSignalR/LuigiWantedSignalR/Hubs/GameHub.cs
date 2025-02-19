using BL;
using ENT;
using Microsoft.AspNetCore.SignalR;

namespace LuigiWantedSignalR.Hubs;

public class GameHub : Hub
{
    private static List<clsUsuario> listaUsuarios = new List<clsUsuario>();

    public static List<clsUsuario> ListaUsuarios()
    {
        return listaUsuarios;
    }

    public async Task Unirse(string nombre)
    {
        // Lógica para unirse al juego
        clsUsuario nuevoUsuario = new clsUsuario(nombre);
        listaUsuarios.Add(nuevoUsuario);
        await Clients.All.SendAsync("UsuarioUnido", nuevoUsuario);
    }

    public async Task PersonajeABuscar()
    {
        clsPersonaje personaje = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
        await Clients.All.SendAsync("PersonajeBuscado", personaje);
    }

    public async Task Registrarse(string nombre)
    {
        clsUsuario usuario = clsListadosUsuarioBL.addUserBL(nombre);
        listaUsuarios.Add(usuario);
        await Clients.All.SendAsync("UsuarioUnido", nombre);
    }

    public async Task EmpezarPantallaWanted()
    {
        clsPersonaje personaje = clsListadoPersonajeBL.ObtenerPersonajeAleatorio();
        await Clients.All.SendAsync("EmpezarPantallaWanted", personaje);
    }

    public async Task PersonajeEncontrado(string nombre)
    {
        await Clients.All.SendAsync("PersonajeEncontrado", nombre);
    }

    public async Task ScoreUpdate(int score)
    {
        // Lógica para actualizar el puntaje
        Clients.All.SendAsync("ScoreActualizado", score);
    }

    public async Task ScoreUpdate()
    {
        await Clients.All.SendAsync("ScoreActualizado");
    }

    public async Task EmpezarJuego()
    {
        await Clients.All.SendAsync("JuegoListo", ListaUsuarios().Count);
    }
}