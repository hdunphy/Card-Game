using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class StatContainerController : MonoBehaviour
    {
        [SerializeField] private TMP_Text propertyName;
        [SerializeField] private RectTransform statBarTransform;
        [SerializeField] private TMP_Text propertyValue;

        public void Setup(StatContainerProps statContainerProps)
        {
            propertyName.text = statContainerProps.Name;
            propertyValue.text = statContainerProps.Value.ToString();
            statBarTransform.localScale = new Vector3(statContainerProps.Percentage, 1, 1);
        }
    }

    public class StatContainerProps
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int MaxValue { get; set; }
        public float Percentage => (float)Value / MaxValue;

        public StatContainerProps(string name, int value, int maxValue)
        {
            Name = name;
            Value = value;
            MaxValue = maxValue;
        }
    }
}