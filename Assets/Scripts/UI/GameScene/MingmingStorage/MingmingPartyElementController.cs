using Assets.Scripts.Entities.Mingmings;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingPartyElementController : MingmingBaseController
    {
        [SerializeField] private MingmingStatsPanel statsPanel;
        [SerializeField] private GameObject statContainerParent;
        [SerializeField] private TMP_Text buttonText;

        private bool showStatsPanel = true;
        const string SHOW_TEXT = "SHOW";
        const string HIDE_TEXT = "HIDE";

        public override void Setup(MingmingInstance mingming)
        {
            base.Setup(mingming);

            statsPanel.Setup(mingming);

            ToggleShowStatsPanel();
        }

        public void ToggleShowStatsPanel()
        {
            showStatsPanel = !showStatsPanel;
            statContainerParent.SetActive(showStatsPanel);
            buttonText.text = showStatsPanel? HIDE_TEXT : SHOW_TEXT;
        }
    }
}