using BL;
using ENT;
using Microsoft.AspNetCore.SignalR;

namespace LuigiWantedSignalR.Hubs;

public class GameHub : Hub
{
    private static List<clsUsuario> listaUsuarios = new List<clsUsuario>();

<<<<<<< Updated upstream
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
        clsPersonaje personaje = clsListadosPersonajeBL.GetPersonajeAleatorio();
        await Clients.All.SendAsync("PersonajeBuscado", personaje);
=======
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
>>>>>>> Stashed changes
    }

    public async Task PersonajeEncontrado(string nombre)
    {
        await Clients.All.SendAsync("PersonajeEncontrado", nombre);
    }

<<<<<<< Updated upstream
    public async Task ScoreUpdate(int score)
    {
        // Lógica para actualizar el puntaje
        await Clients.All.SendAsync("ScoreActualizado", score);
=======
    public async Task ScoreUpdate()
    {
        await Clients.All.SendAsync("ScoreActualizado");
>>>>>>> Stashed changes
    }

    public async Task EmpezarJuego()
    {
<<<<<<< Updated upstream
        await Clients.All.SendAsync("JuegoListo", ListaUsuarios().Count);
=======
        await Clients.All.SendAsync("JuegoListo", listaUsuarios.Count);
>>>>>>> Stashed changes
    }
}