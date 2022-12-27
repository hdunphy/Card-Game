using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RandomEncounter : MonoBehaviour, IEncounter
{
    [Header("Drop Table")]
    [SerializeField] private DropTableInstance DropTable;
    [SerializeField, Range(0, 1)] private float EncounterChance;
    [SerializeField, Range(0, 3)] private int MaxMingmings;
    [SerializeField, Range(1, 98)] private int MinMingmingLevel;
    [SerializeField, Range(2, 99)] private int MaxMingmingLevel;

    [Header("Events")]
    [SerializeField] private UnityEvent OnStartEncounter;

    List<MingmingInstance> mingmings;

    private void Start()
    {
        mingmings = new List<MingmingInstance>();
    }

    public void GetEncounter()
    {
        bool hasEncounter = Rules.GetRandomFloat() <= EncounterChance;
        if (!hasEncounter) return;

        mingmings.Clear();

        int mingmingSpawns = Rules.GetRandomInt(1, MaxMingmings + 1);

        for (int i = 0; i < mingmingSpawns; i++)
        {
            var _drop = DropTable.GetDrop();

            if (_drop != null)
            {
                int mingmingLevel = Rules.GetRandomInt(MinMingmingLevel, MaxMingmingLevel + 1);
                var _mingming = new MingmingInstance((MingmingData)_drop, mingmingLevel);
                _mingming.Name = "Wild " + _mingming.Name;
                mingmings.Add(_mingming);
            }
        }

        if (mingmings.Any())
        {
            OnStartEncounter?.Invoke();
            var player = FindObjectOfType<PlayerController>();
            
            var thisScene = new LevelSceneData(gameObject.scene.name, this, player);
            var battleScene = new BattleSceneData(
                GameSceneController.BattleScene,
                thisScene,
                new(player.DevController.DeckHolder.CurrentDeck, player.DevController.PlayableMingmings, player.PlayerInventory),
                new(new List<CardData>(), mingmings, new PlayerInventory()));

            GameSceneController.Singleton.SwapScenes(thisScene, battleScene);
        }
    }

    public List<CardData> GetRewards()
    {
        return mingmings.Select(x => x.GetCardDrop()).ToList();
    }
}