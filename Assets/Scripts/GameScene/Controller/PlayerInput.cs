using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController Player;

        Vector2 movementVector;

        // Update is called once per frame
        void Update()
        {
            movementVector.x = Input.GetAxisRaw("Horizontal");
            movementVector.y = Input.GetAxisRaw("Vertical");
            Player.SetMoveDirection(movementVector);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.Interact();
            }
        }
    }
}
