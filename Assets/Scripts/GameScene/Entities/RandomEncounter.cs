using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using Assets.Scripts.References;
using Assets.Scripts.UI.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RandomEncounter : MonoBehaviour, IEncounter
{
    [Header("Drop Table")]
    [SerializeField] private DropTableInstance DropTable;
    [SerializeField, Range(0, 3)] private int MaxMonsters;
    [SerializeField, Range(1, 98)] private int MinMonsterLevel;
    [SerializeField, Range(2, 99)] private int MaxMonsterLevel;

    [Header("Events")]
    [SerializeField] private UnityEvent OnStartEncounter;

    List<MingmingInstance> mingmings;

    private void Start()
    {
        mingmings = new List<MingmingInstance>();
    }

    public void GetEncounter()
    {
        mingmings.Clear();

        int monsterSpawns = Rules.GetRandomInt(0, MaxMonsters + 1);

        for (int i = 0; i < monsterSpawns; i++)
        {
            var _drop = DropTable.GetDrop();

            if (_drop != null)
            {
                int monsterLevel = Rules.GetRandomInt(MinMonsterLevel, MaxMonsterLevel + 1);
                var _monster = new MingmingInstance((MingmingData)_drop, monsterLevel);
                _monster.Name = "Wild " + _monster.Name;
                mingmings.Add(_monster);
            }
        }

        if (mingmings.Any())
        {
            OnStartEncounter?.Invoke();
            var player = FindObjectOfType<PlayerController>();
            
            var thisScene = new LevelSceneData(gameObject.scene.name, this, player);
            var battleScene = new BattleSceneData(GameSceneController.BattleScene, player.DevController.DeckHolder.CurrentDeck, new List<CardData>(),
                thisScene, player.DevController.PlayableMonsters, mingmings);

            GameSceneController.Singleton.SwapScenes(thisScene, battleScene);
        }
    }

    public List<CardData> GetRewards()
    {
        return mingmings.Select(x => x.GetCardDrop()).ToList();
    }
}