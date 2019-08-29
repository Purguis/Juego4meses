using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLevelManager : MonoBehaviour
{
    public static CLevelManager Inst
    {
        get
        {
            return _inst;
        }
    }
    private static CLevelManager _inst;

    public static int STATE_PLAY = 0;
    public static int STATE_WIN = 1;

    private int _state;
    private int _roomIndex = 0;

    void Awake()
    {
        if(_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        DontDestroyOnLoad(this.gameObject); //no destruye el objeto entre escenas
    }

    private void Start()
    {
        InitRoom(_roomIndex);
        CAudioManager.Inst.PlayMusic("theme");
    }

    public void SetState(int aState)
    {
        _state = aState;
        if (_state == STATE_WIN)
        {
            //NO SE HACEEE
            GameObject.FindObjectOfType<Light>().enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log(CEnemyManager.Inst.GetCount());

        if(_state == STATE_PLAY)
        {
            if(CEnemyManager.Inst.GetCount() == 0) //win condition
            {
                _roomIndex += 1;
                if(_roomIndex >= CRoomManager.Inst.GetRoomCount()) //chequeo si pase todos los cuartos
                {
                    SetState(STATE_WIN);
                }
                else
                {
                    InitRoom(_roomIndex);
                }
            }
        }
        else if(_state == STATE_WIN)
        {

        }
    }

    private void InitRoom(int room)
    {
        List<Transform> spawnPos = CRoomManager.Inst.GetRoomSpawnPositions(room);
        for (int i = 0; i < spawnPos.Count; i++)
        {
            CEnemyManager.Inst.SpawnEnemy(spawnPos[i].position);
        }
    }

}
