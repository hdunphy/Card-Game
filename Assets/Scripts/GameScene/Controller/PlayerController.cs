using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<MonsterInstance> Monsters { get; private set; }
    //Replace
    public List<MonsterData> PlayerData;
    public List<CardData> PlayerCards;

    IMovement Movement;
    Vector2 movementVector;

    private IInteractable Interactable;

    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<IMovement>();
        Monsters = PlayerData.Select(d => new MonsterInstance(d, 10)).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");
        Movement.SetMoveDirection(movementVector);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable.GetInteraction();
        }
    }

    public void AddCards(CardData selectedCard)
    {
        PlayerCards.Add(selectedCard);
    }

    public void SetInteraction(IInteractable interactable)
    {
        Interactable = interactable;
    }
}
