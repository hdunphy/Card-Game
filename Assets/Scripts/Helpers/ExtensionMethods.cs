using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class ExtensionMethods
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static Transform DestroyAllChildren(this Transform transform)
        {
            foreach(Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }

            return transform;
        }
    }
}
