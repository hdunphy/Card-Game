using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCouple : MonoBehaviour
{
    [SerializeField] private BatteryIcon batteryIcon;
    private List<BatteryIcon> Batteries;
    public static readonly int FULL_CAPCITY = 6;

    private void Awake()
    {
        Batteries = new List<BatteryIcon>
        {
            Instantiate(batteryIcon, transform),
            Instantiate(batteryIcon, transform)
        };

        foreach (BatteryIcon icon in Batteries)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void SetBatteryCapacity(int cellsToFill, int? _capacity = null)
    {
        int capacity = _capacity ?? cellsToFill;
        if (cellsToFill > FULL_CAPCITY || capacity > FULL_CAPCITY)
        {
            Debug.LogError($"Can't have more than {FULL_CAPCITY} filled. Filled {cellsToFill}, Capcity {capacity}");
            return;
        }

        int batteryNumber = Mathf.CeilToInt((float)capacity / BatteryIcon.FULL_CAPCITY);

        for(int i = 0; i < Batteries.Count; i++)
        {
            BatteryIcon _icon = Batteries[i];
            if(i < batteryNumber)
            {
                _icon.gameObject.SetActive(true);
                int currentCellsToFill = cellsToFill % BatteryIcon.FULL_CAPCITY;
                currentCellsToFill = currentCellsToFill == 0 && cellsToFill > 0 ? BatteryIcon.FULL_CAPCITY : currentCellsToFill;
                int currentCapacity = capacity % BatteryIcon.FULL_CAPCITY;
                currentCapacity = currentCapacity == 0 && capacity > 0 ? BatteryIcon.FULL_CAPCITY : currentCapacity;

                if ((capacity - currentCapacity) - (cellsToFill - currentCellsToFill) >= BatteryIcon.FULL_CAPCITY)
                    currentCellsToFill = 0;

                _icon.SetBatteryColors(currentCellsToFill, currentCapacity);
                cellsToFill -= currentCellsToFill;
                capacity -= currentCapacity;
            }
            else
            {
                _icon.gameObject.SetActive(false);
            }
        }

    }
}
