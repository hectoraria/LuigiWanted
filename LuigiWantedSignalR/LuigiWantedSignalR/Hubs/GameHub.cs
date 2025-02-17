﻿using BL;
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
        clsPersonaje personaje = clsListadosPersonajeBL.GetPersonajeAleatorio();
        await Clients.All.SendAsync("PersonajeBuscado", personaje);
    }

    public async Task PersonajeEncontrado(string nombre)
    {
        await Clients.All.SendAsync("PersonajeEncontrado", nombre);
    }

    public async Task ScoreUpdate(int score)
    {
        // Lógica para actualizar el puntaje
        await Clients.All.SendAsync("ScoreActualizado", score);
    }

    public async Task EmpezarJuego()
    {
        await Clients.All.SendAsync("JuegoListo", ListaUsuarios().Count);
    }
}