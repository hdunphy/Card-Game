using Assets.Scripts.Entities.Mingmings;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class StoredMingmingController : MingmingBaseController
    {
        [SerializeField] private RectTransform experienceTransform;
        [SerializeField] private MingmingStatsPanel statsPanel;

        public override void Setup(MingmingInstance mingming, Action<MingmingInstance> onSelected)
        {
            base.Setup(mingming, onSelected);
            statsPanel.Setup(mingming);

            experienceTransform.localScale = new Vector3(mingming.GetExperiencePercentage(), 1, 1);
        }
    }
}