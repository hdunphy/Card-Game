using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyHolder : MonoBehaviour
{
    public int EnergyAvailable, TotalEnergy;
    private int _avail, _tot;

    [SerializeField] private TextMeshProUGUI EnergyValue;
    [SerializeField] private BatteryCouple BatteryCouple;

    private List<BatteryCouple> Couples;

    private void Awake()
    {
        Couples = new List<BatteryCouple>();
    }

    public void SetEnergy(int energyAvailable, int totalEnergy)
    {
        EnergyValue.text = $"{energyAvailable}/{totalEnergy}";

        int couplesNeeded = Mathf.CeilToInt((float)totalEnergy / BatteryCouple.FULL_CAPCITY);

        if (couplesNeeded > Couples.Count)
        {
            for (int i = Couples.Count; i < couplesNeeded; i++)
            {
                Couples.Add(Instantiate(BatteryCouple, transform));
                Couples[i].transform.SetAsFirstSibling();
            }
        }
        else if (couplesNeeded < Couples.Count)
        {
            for (int i = couplesNeeded; i < Couples.Count; i++)
                Destroy(Couples[i].gameObject);

            Couples.RemoveRange(couplesNeeded, Couples.Count - couplesNeeded);
        }

        foreach (BatteryCouple _batteryCouple in Couples)
        {
            int currentCellsToFill = energyAvailable % BatteryCouple.FULL_CAPCITY;
            currentCellsToFill = currentCellsToFill == 0 && energyAvailable > 0 ? BatteryCouple.FULL_CAPCITY : currentCellsToFill;
            int currentCapacity = totalEnergy % BatteryCouple.FULL_CAPCITY;
            currentCapacity = currentCapacity == 0 && totalEnergy > 0 ? BatteryCouple.FULL_CAPCITY : currentCapacity;

            if ((totalEnergy - currentCapacity) - (energyAvailable - currentCellsToFill) >= BatteryCouple.FULL_CAPCITY)
                currentCellsToFill = 0;

            _batteryCouple.SetBatteryCapacity(currentCellsToFill, currentCapacity);
            energyAvailable -= currentCellsToFill;
            totalEnergy -= currentCapacity;
        }

        EnergyValue.transform.SetAsLastSibling();
    }
}
