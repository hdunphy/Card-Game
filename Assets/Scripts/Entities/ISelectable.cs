using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}