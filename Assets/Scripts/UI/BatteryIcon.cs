using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIcon : MonoBehaviour
{
    [SerializeField] private List<Image> BatteryCells;
    [SerializeField] private List<Sprite> BatteryCellSprites;
    public static readonly int FULL_CAPCITY = 3;

    public void SetBatteryColors(int numberFilled, int? _capacity = null)
    {
        int capacity = _capacity ?? numberFilled;
        if(numberFilled > FULL_CAPCITY || capacity > FULL_CAPCITY)
        {
            Debug.LogError($"Can't have more than {FULL_CAPCITY} filled. Filled {numberFilled}, Capcity {capacity}");
            return;
        }

        Sprite _sprite = BatteryCellSprites[numberFilled];

        for(int i = 0; i < BatteryCells.Count; i++)
        {
            BatteryCells[i].gameObject.SetActive(true);

            if (i < numberFilled)
                BatteryCells[i].sprite = _sprite;
            else if (i < capacity)
                BatteryCells[i].sprite = BatteryCellSprites[0];
            else
                BatteryCells[i].gameObject.SetActive(false);
        }
    }
}
