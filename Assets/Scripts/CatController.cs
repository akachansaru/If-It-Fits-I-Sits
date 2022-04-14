using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public float speed = 1;

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }

}
