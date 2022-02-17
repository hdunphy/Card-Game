using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReferenceDictionary : MonoBehaviour
{
    public static SpriteReferenceDictionary Instance;

    [SerializeField] private List<EnumSpritePair<TargetType>> TargetTypeReferences;
    [SerializeField] private List<EnumSpritePair<CardAlignment>> CardAlignmentReferences;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public Sprite GetSpriteFromEnum<T>(T _enum)
    {
        Sprite sprite;
        object list;

        switch (_enum)
        {
            case TargetType t:
                list = TargetTypeReferences;
                break;
            case CardAlignment c:
                list = CardAlignmentReferences;
                break;
            default:
                list = null;
                break;
        }
        if(list != null)
        {
            sprite = ((List<EnumSpritePair<T>>)list).Find(x => x.Enum.Equals(_enum)).Sprite;
        }
        else
        {
            sprite = null;
        }

        return sprite;
    }

    [Serializable]
    public class EnumSpritePair<T>
    {
        public Sprite Sprite;
        public T Enum;
    }
}
