using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayer : MonoBehaviour
{
    private Camera _cam;
    void Start()
    {
        _cam = GetComponentInChildren<Camera>();
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CAudioManager.Inst.PlaySFX("fire", false, transform,true);
            RaycastHit hitInfo;

            Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hitInfo, 1000);
            //Physics.SphereCast(_cam.transform.position, .5f, _cam.transform.forward, out hitInfo); //raycast con ancho
            //Debug.DrawRay(_cam.transform.position, _cam.transform.forward * 10, Color.cyan,1f);
            if (hitInfo.collider != null)
            {
                object obj = hitInfo.collider.gameObject.GetComponent(typeof(IDamageable));
                if (obj != null)
                {
                    (obj as IDamageable).OnHit(1);
                    Debug.DrawLine(_cam.transform.position, hitInfo.point, Color.green, 1);
                }
                else
                {
                    Debug.DrawLine(_cam.transform.position, hitInfo.point, Color.red, 1);
                }
            }
            else
                Debug.DrawRay(_cam.transform.position, _cam.transform.forward * 10,Color.cyan);
        }
    }
}
