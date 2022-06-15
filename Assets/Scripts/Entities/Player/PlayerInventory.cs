using Assets.Scripts.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerInventory
    {
        private Dictionary<IItem, int> _items;

        public event Action OnChange;

        public PlayerInventory()
        {
            _items = new Dictionary<IItem, int>();
        }

        public int Count(IItem item) => _items.ContainsKey(item) ? _items[item] : 0;

        public int Add(IItem item, int count)
        {
            if (_items.ContainsKey(item)) {
                _items[item] += count;
            }
            else
            {
                _items.Add(item, count);
            }

            OnChange?.Invoke();

            return Count(item);
        }

        public int Remove(IItem item, int count)
        {
            _items[item] -= count;

            if(_items[item] <= 0)
            {
                _items.Remove(item);
            }

            OnChange?.Invoke();

            return Count(item);
        }
    }
}