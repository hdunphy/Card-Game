using Assets.Scripts.Entities;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.GameScene.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public IEnumerable<MonsterInstance> PlayableMonsters { get => monsters.Where( m => m.CurrentHealth > 0); }
    //Replace
    public List<MonsterData> PlayerData;
    [SerializeField] private List<CardData> PlayerCards;

    IMovement Movement;

    private IPlayerInteractable Interactable;
    private List<MonsterInstance> monsters;

    public IDeckHolder DeckHolder { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Movement = GetComponent<IMovement>();
        monsters = PlayerData.Select(d => new MonsterInstance(d, 10)).ToList();

        DeckHolder = new PlayerDeckHolder(PlayerCards, new List<List<CardData>> { new List<CardData>(PlayerCards) });
    }

    public void SetMoveDirection(Vector2 movementVector) => Movement.SetMoveDirection(movementVector);

    public void HealMonsters() => monsters.ForEach((monster) => monster.CurrentHealth = monster.Health);

    public void Interact() => Interactable?.Interact(this);

    /// <summary>
    /// Move player to new room
    /// </summary>
    /// <param name="loadPosition">The Vector3 for where the player should move to in new room</param>
    public void EnterRoom(Vector3 loadPosition)
    {
        transform.position = loadPosition; //Move player to position
        Camera.main.transform.position = new Vector3(loadPosition.x, loadPosition.y, Camera.main.transform.position.z); //Move camera to position
        Movement.SetCanMove(true); //re-enable player movement
    }

    public void SavePlayerData()
    {
        SaveData.Current.PlayerPosition = transform.position;
        SaveData.Current.PlayerSceneName = gameObject.scene.name;
        SaveData.Current.DeckHolder = DeckHolder;
        SaveData.Current.PlayerMonsters = monsters;
    }

    /// <summary>
    /// When a saved game is loaded, call this function to restore player to last loaded state
    /// </summary>
    /// <param name="loadPosition">position player will load in at</param>
    public void OnLoad(Vector3 loadPosition)
    {
        EnterRoom(loadPosition);
    }

    public void SetInteraction(IPlayerInteractable interactable)
    {
        Interactable = interactable;
    }
}
