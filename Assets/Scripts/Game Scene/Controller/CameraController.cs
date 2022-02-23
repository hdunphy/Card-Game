using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float MoveSpeedFactor;
    [SerializeField] private int Zvalue;
    private Transform player;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>()?.transform;
        }
        else
        {
            Vector3 target = new Vector3(player.position.x, player.position.y, Zvalue);
            float distance = Vector2.Distance(target, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target, distance * distance * MoveSpeedFactor);
        }
    }
}