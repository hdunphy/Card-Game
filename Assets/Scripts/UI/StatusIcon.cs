using Assets.Scripts.Entities.Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text StatusCount;
    [SerializeField] private Image StatusSprite;

    private BaseStatus Status;
    public int Count { get; private set; }

    private TooltipTrigger TooltipTrigger;

    public void SetStatus(BaseStatus status, int _count)
    {
        Status = status;
        TooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
        SetCount(_count);
    }

    public void SetCount(int _count)
    {
        Count = _count;
        StatusSprite.sprite = Status.GetSprite(_count);
        StatusCount.text = Count.ToString();
        UpdateTooltip();
    }

    public int AddCount(int _count)
    {
        SetCount(Count + _count);
        return Count;
    }

    public void UpdateTooltip()
    {
        TooltipTrigger.SetText(Status.GetTooltip(Count), Status.GetTooltipHeader(Count));
    }
}
