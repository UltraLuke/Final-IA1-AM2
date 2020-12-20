using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Win")]
    public GameObject winContainer;
    public Text textTeam;
    public string textT1Win;
    public string textT2Win;
    public Color team1Color;
    public Color team2Color;

    private void Start()
    {
        SubscribeEvents();
    }
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
    void SubscribeEvents()
    {
        EventsHandler.SubscribeToEvent("EVENT_TEAM1WINS", LitTeam1Win);
        EventsHandler.SubscribeToEvent("EVENT_TEAM2WINS", LitTeam2Win);
    }
    void UnsubscribeEvents()
    {
        EventsHandler.UnsubscribeToEvent("EVENT_TEAM1WINS", LitTeam1Win);
        EventsHandler.UnsubscribeToEvent("EVENT_TEAM2WINS", LitTeam2Win);
    }

    private void LitTeam1Win()
    {
        winContainer.SetActive(true);
        textTeam.text = textT1Win;
        textTeam.color = team1Color;
    }
    private void LitTeam2Win()
    {
        winContainer.SetActive(true);
        textTeam.text = textT2Win;
        textTeam.color = team2Color;
    }
}
