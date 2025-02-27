﻿using ENT;
using Microsoft.AspNetCore.SignalR.Client;

namespace LuigiWanted.VM;

public class pantallaBuscarVM
{
    #region Atributos

    private readonly clsUsuario usuario;
    private string personajeSeleccionado;
    private readonly string personajeCorrecto;
    private List<clsPersonaje> listadoPersonajes;
    private HubConnection _connection;

    #endregion

    #region Propiedades

    public List<clsPersonaje> ListadoPersonajes
    {
        get { return listadoPersonajes; }
    }

    public string PersonajeSeleccionado
    {
        set
        {
            personajeSeleccionado = value;

            //Comprobacion de que la persona seleccionada es la correcta
            if (personajeSeleccionado == personajeCorrecto)
            {
                usuario.Score += 1;

                //Notifico al hub que se ha descubierto al personaje
                PersonajeEncontrado();
            }

            else
            {
                usuario.Score -= 1;
            }

            ActulizarPuntuacion();

        }
    }

    public int TiempoRestante { get; }

    #endregion

    #region Constructores

    public pantallaBuscarVM(clsUsuario usuario, List<clsPersonaje> listadoPersonajes, string personajeCorrecto)
    {
        this.usuario = usuario;
        this.listadoPersonajes = listadoPersonajes;
        this.personajeCorrecto = personajeCorrecto;
        Inicializar();
    }

    #endregion

    #region Metodos

    /// <summary>
    /// Crea la conexion
    /// </summary>
    private void Inicializar()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5297/chathub")
            .Build();

        StartConnection();
    }

    /// <summary>
    /// Empieza la conexión con el Hub
    /// </summary>
    private async void StartConnection()
    {
        try
        {
            await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al iniciar la conexión: {ex.Message}");
        }
    }

    /// <summary>
    /// Funcion para notificar al resto de usuarios que le usuario ha sido encontrado para que no sigan buscando
    /// </summary>
    private async void PersonajeEncontrado()
    {
        await _connection.InvokeAsync("PersonajeEncontrado");
    }

    /// <summary>
    /// Funcion para actualizar la puntuacion del usuario
    /// </summary>
    private async void ActulizarPuntuacion()
    {
        await _connection.InvokeCoreAsync("ActualizarPuntuacion", args: new[] { usuario });
    }

    #endregion
}