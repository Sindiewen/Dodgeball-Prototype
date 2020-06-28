using UnityEngine;

public class sphereController : MonoBehaviour
{
    public Color friendlyColor = Color.green;
    public Color neutralColor = Color.white;
    public Color enemyColor = Color.red;
    public float swtichToNeutral = 0.25f;


    private Material mat;
    private Renderer rend;
    private TeamObject team;
    private float switchToNeutralTimer = 0;


    private void Awake()
    {
        rend = GetComponent<Renderer>();
        team = GetComponent<TeamObject>();
    }

    public void setColorFriendly()
    {
        rend.material.color = friendlyColor;
        switchToNeutralTimer = swtichToNeutral;
    }

    public void setColorNeutral()
    {
        rend.material.color = neutralColor;
    }
    
    public void setColorEnemy()
    {
        rend.material.color = enemyColor;
        switchToNeutralTimer = swtichToNeutral;
    }

    public void setTeam(string color)
    {
        if(color == "red")
        {
            team.currentTeam = TeamObject.Team.RED;
        }
        else if(color == "blu")
        {
            team.currentTeam = TeamObject.Team.BLU;
        }
    }

    private void Update()
    {
        if (switchToNeutralTimer > 0)
        {
            switchToNeutralTimer -= Time.deltaTime;
            
            if(switchToNeutralTimer <= 0)
            {
                setColorNeutral();
            }
        }
    }




    // Getters/setters
    public TeamObject GetTeam
    {
        get { return team; }
    }

}
