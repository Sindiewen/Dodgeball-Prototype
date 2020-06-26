using UnityEngine;

[System.Serializable]
public class Cube
{
    public Vector3 cube;

    public float x
    {
        get { return cube.x; }
        set { cube.x = value;}
    }
    public float y
    {
        get { return cube.y; }
        set { cube.y = value;}
    }
    public float z
    {
        get { return cube.z; }
        set { cube.z = value;}
    }

    public Vector3 center
    {
        get { return new Vector3(x/2, y/2, z/2); }
    }
    
    public Vector3 size
    {
        get { return new Vector3(x, y, z); }
    }
}

