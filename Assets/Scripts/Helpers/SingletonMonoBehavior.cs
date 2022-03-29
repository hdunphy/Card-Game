using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public abstract class SingletonMonoBehavior<T> : MonoBehaviour
    {
        public static T Singleton { get; private set; }

        protected void OnAwake(T _instance)
        {
            //Singleton pattern On Awake set the singleton to this.
            //There should only be one GameLayer that can be accessed statically
            if (Singleton == null)
            {
                Singleton = _instance;
            }
            else
            { //if BattleManager already exists then destory this. We don't want duplicates
                Destroy(this);
            }
        }

        protected void DestroySingleton() => Singleton = default;
    }
}
