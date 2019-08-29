using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRoom : MonoBehaviour
{
    private List<Transform> _tfs;
    // Start is called before the first frame update
    void Awake()
    {
        _tfs = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _tfs.Add(transform.GetChild(i));            
        }
    }

    public List<Transform> GetSpawnPositions()
    {
        return _tfs;
    }
   
}
