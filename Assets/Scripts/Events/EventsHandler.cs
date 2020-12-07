using System.Collections.Generic;

/// <summary>
/// Manejador de eventos
/// </summary>
public class EventsHandler
{
    public delegate void EventReceiver();
    private static Dictionary<string, EventReceiver> _events;

    /// <summary>
    /// Llamamos a este método para suscribirnos a eventos
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void SubscribeToEvent(string eventName, EventReceiver listener)
    {
        //Si el diccionario no está inicializado, lo inicializa. Lazy initialization.
        if (_events == null)
            _events = new Dictionary<string, EventReceiver>();

        //Si existe un evento con el mismo nombre, usa ese, de lo contrario lo crea.
        if (!_events.ContainsKey(eventName))
            _events.Add(eventName, null);

        //Carga el método al evento
        _events[eventName] += listener;
    }

    /// <summary>
    /// Llamamos a este método para desuscribirnos de eventos
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void UnsubscribeToEvent(string eventName, EventReceiver listener)
    {
        //Busca en el diccionario de eventos el método. Si lo encuentra lo elimina.
        if (_events != null)
        {
            if (_events.ContainsKey(eventName))
                _events[eventName] -= listener;
        }
    }

    /// <summary>
    /// Llamamos a esta función para disparar un evento
    /// </summary>
    /// <param name="eventName"></param>
    public static void TriggerEvent(string eventName)
    {
        //Si no existe dicho evento tira un Warning y no se ejecuta lo siguiente
        if(_events == null)
        {
            UnityEngine.Debug.LogWarning("No events subscribed");
            return;
        }

        //Si existe el evento con dicho nombre, y el método del cual forma parte, lo ejecuta.
        if (_events.ContainsKey(eventName))
            _events[eventName]?.Invoke();
    }
}