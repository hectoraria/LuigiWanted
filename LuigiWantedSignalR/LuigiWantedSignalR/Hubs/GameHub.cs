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

    public async Task Registrarse(string nombre)
    {
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

            listadoUsuarios = new List<clsUsuario>();

            await Clients.All.SendAsync("JuegoListo", jsonResponse);
            juegoListoEnviado = true;

        }
    }

    public async Task PersonajeEncontrado()
    {
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

    public async Task EmpezarBusqueda(clsPersonaje personajeABuscar)
    {
        listadoUsuariosListos.Add(true);
        if (listadoUsuariosListos.Count >= listadoUsuarios.Count)
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
                    } while (personaje.Nombre.Equals(personajeABuscar.Nombre));
                    listadoPersonajesBuscar.Add(personaje);
                }
            }
            index = random.Next(listadoPersonajes.Count);
            listadoPersonajesBuscar[index] = personajeABuscar;
            jugadoresListos = 0;

            listadoUsuariosListos = new List<bool>();
            listadoUsuarios = new List<clsUsuario>();

            BuscarDTO buscarDto = new BuscarDTO(personajeABuscar, listadoPersonajesBuscar);
            string jsonResponse = JsonConvert.SerializeObject(buscarDto);

            await Clients.All.SendAsync("BusquedaLista", jsonResponse);
        }
    }

    public async Task SigueConectado(clsUsuario usuario)
    {
        if (!listadoUsuarios.Contains(usuario) && usuario != null)
        {
            listadoUsuarios.Add(usuario);
        }
    }

    public async Task DesconectarUsuario(int idUsuario)
    {
        clsUsuario usuario = listadoUsuarios.Find(u => u.Id == idUsuario);
        if (usuario != null)
        {
            listadoUsuarios.Remove(usuario);
        }
    }
}