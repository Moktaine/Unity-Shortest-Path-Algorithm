using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMoveCamera(InputValue value)
    {
        var v = value.Get<Vector2>();
        v = v * moveSpeed;
        this.transform.position += new Vector3(v.x,v.y,0);
    }
}
