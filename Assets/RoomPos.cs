using UnityEngine;

public class RoomPos : MonoBehaviour
{
    public static RoomPos instance;
    public float waitingRoomX = 0f;
    public float auditoriumRoomX = 20f;
    public float minigameX = 40f;
    
    void Awake()
    {
        instance = this;
    }

}
