using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyManager : MonoBehaviour
{
    public static CEnemyManager Inst
    {
        get
        {
            if (_inst == null)
                return new GameObject("Enemy Manager").AddComponent<CEnemyManager>();
            return _inst;
        }
    }
    private static CEnemyManager _inst;

    public GameObject _enemyAsset;
    private List<CEnemy> _enemies;

    void Awake()
    {
        if(_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        DontDestroyOnLoad(this.gameObject); //no destruye el objeto entre escenas

        _enemies = new List<CEnemy>();
    }

    public int GetCount()
    {
        return _enemies.Count;
    }
    private void Start()
    {
        /*
        for (int i = 0; i < 5; i++)
        {
           
        }
        */
    }

    public void LateUpdate()
    {
        for(int i = _enemies.Count-1;i >= 0; i--)
        {
            if(_enemies[i] == null)
            {
                _enemies.RemoveAt(i);
            }
        }
    }

    public void SpawnEnemy(Vector3 pos)
    {
        GameObject enemy = GameObject.Instantiate(_enemyAsset);
        enemy.transform.position = pos;
        _enemies.Add(enemy.GetComponent<CEnemy>());
    }

}
