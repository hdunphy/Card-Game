using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class BattleCameraController : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<CameraController>().gameObject.SetActive(false);
        }
    }
}
