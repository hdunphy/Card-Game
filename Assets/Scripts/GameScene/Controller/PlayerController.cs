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
    [SerializeField] private List<CardData> PlayerCards;

    IMovement Movement;
    Vector2 movementVector;

    private IInteractable Interactable;
    public IDeckHolder DeckHolder { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<IMovement>();
        Monsters = PlayerData.Select(d => new MonsterInstance(d, 10)).ToList();

        DeckHolder = new PlayerDeckHolder(PlayerCards, new List<List<CardData>> { new List<CardData>(PlayerCards) });
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

    public void SetInteraction(IInteractable interactable)
    {
        Interactable = interactable;
    }
}
