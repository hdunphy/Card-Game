using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ScrollableUILog : MonoBehaviour, IDisplayLog
    {
        [SerializeField] private GameObject ScrollableContent;
        [SerializeField] private TMPro.TMP_Text LogEntryPrefab;

        public void AddMessageToLog(string message)
        {
            //todo update scrollable content's height;
            var entry = Instantiate(LogEntryPrefab, ScrollableContent.transform);
            entry.text = message;
            entry.transform.SetAsFirstSibling();
        }
    }
}