using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRoomManager : MonoBehaviour
{
    public static CRoomManager Inst
    {
        get
        {
            return _inst;
        }
    }
    private static CRoomManager _inst;

    private CRoom[] _rooms;
    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        DontDestroyOnLoad(this.gameObject); //no destruye el objeto entre escenas
        
        _rooms = gameObject.GetComponentsInChildren<CRoom>();

    }

    public CRoom GetRoom(int i)
    {
        return _rooms[i];
    }

    public int GetRoomCount()
    {
        return _rooms.Length;
    }

    public List<Transform> GetRoomSpawnPositions(int roomI)
    {
        return GetRoom(roomI).GetSpawnPositions();
    }
}
