using UnityEngine;

public class sphereController : MonoBehaviour
{
    public Color friendlyColor = Color.green;
    public Color neutralColor = Color.white;
    public Color enemyColor = Color.red;


    private Material mat;
    private Renderer rend;
    private TeamManager team;


    private void Start()
    {
        // mat = GetComponent<Material>();
        rend = GetComponent<Renderer>();
        team = GetComponent<TeamManager>();
    }

    public void setColorFriendly()
    {
        // mat.color = friendlyColor;
        rend.material.color = friendlyColor;

    }

    public void setColorNeutral()
    {
        // mat.color = neutralColor;
        rend.material.color = neutralColor;
    }
    
    public void setColorEnemy()
    {
        // mat.color = enemyColor;
        rend.material.color = enemyColor;
    }

    private void Update()
    {
    }




    // Getters/setters
    public TeamManager GetTeam
    {
        get { return team; }
    }

}
