using UnityEngine;

public class TeamObject: MonoBehaviour
{
    public enum Team
    {
        RED,
        BLU,
        SPECTATOR,
        CIVILIAN
    };
    
    public Team currentTeam;
}
