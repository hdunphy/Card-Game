using UnityEngine;

namespace Assets.Scripts.Controller.References
{
    public class ScriptableObjectReferenceSingleton : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectReference scriptableObjectReference;

        public static ScriptableObjectReferenceSingleton Singleton;

        //TODO FIX THIS. Shouldn't be a singleton
        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else if (Singleton != this)
            {
                Debug.Log("instance already exists, destroying object!");
                Destroy(this);
            }
        }

        public T GetScriptableObject<T>(string name) where T : ScriptableObject => 
            scriptableObjectReference.GetScriptableObject<T>(name);
    }
}
