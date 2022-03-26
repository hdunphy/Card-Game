using Assets.Scripts.Entities;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Entities;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SharedController sharedController;

    IMovement Movement;
    private IPlayerInteractable Interactable;

    public SharedController SharedController => sharedController;

    // Start is called before the first frame update
    void Start()
    {
        if (Movement == null)
            Movement = GetComponent<IMovement>();
    }

    public void SetMoveDirection(Vector2 movementVector) => Movement.SetMoveDirection(movementVector);

    public void Interact() => Interactable?.Interact(this);

    /// <summary>
    /// Move player to new room
    /// </summary>
    /// <param name="loadPosition">The Vector3 for where the player should move to in new room</param>
    public void EnterRoom(Vector3 loadPosition)
    {
        transform.position = loadPosition; //Move player to position
        Camera.main.transform.position = new Vector3(loadPosition.x, loadPosition.y, Camera.main.transform.position.z); //Move camera to position

        if (Movement == null)
            Movement = GetComponent<IMovement>();

        Movement.SetCanMove(true); //re-enable player movement
    }

    public void SavePlayerData()
    {
        SaveData.Current.PlayerPosition = transform.position;
        SaveData.Current.DeckHolder = new DeckHolderSaveModel((PlayerDeckHolder)SharedController.DeckHolder);
        SaveData.Current.PlayerMonsters = SharedController.Monsters.Select(m => new MonsterSaveModel(m)).ToList();
    }

    /// <summary>
    /// When a saved game is loaded, call this function to restore player to last loaded state
    /// </summary>
    /// <param name="loadPosition">position player will load in at</param>
    public void OnLoad(Vector3 loadPosition)
    {
        EnterRoom(loadPosition);

        SharedController.SetDeckHolder(SaveData.Current.DeckHolder?.GetDeckHolder());
        SharedController.SetMonsters(SaveData.Current.PlayerMonsters?.Select(m => new MingmingInstance(m)).ToList());
    }

    public void SetInteraction(IPlayerInteractable interactable)
    {
        Interactable = interactable;
    }
}
