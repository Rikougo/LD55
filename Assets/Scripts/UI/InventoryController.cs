using System.Collections.Generic;
using Summoning.Combination;
using UnityEngine;

namespace Summoning.UI
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private CombinationAsset m_combinationAsset;
        [SerializeField] private PartEntry m_partEntryPrefab;

        private Dictionary<CombinationPart, PartEntry> m_entries;

        public void UpdateDisplay(Dictionary<CombinationPart, int> p_inventory)
        {
            foreach (var (l_combinationPart, l_value) in p_inventory)
            {
                if (!m_entries.ContainsKey(l_combinationPart))
                {
                    if (m_combinationAsset.TryGetPart(l_combinationPart, out var l_data))
                    {
                        var l_instance = Instantiate(m_partEntryPrefab, transform);
                        l_instance.Sprite = l_data.icon;
                        l_instance.Part = l_combinationPart;
                        m_entries.Add(l_combinationPart, l_instance);
                    }
                    else
                    {
                        continue;
                    }
                }

                m_entries[l_combinationPart].Amount = l_value;
            }
        }

        public void OnReset()
        {
            if (m_entries != null)
                foreach (var (l_combinationPart, l_gameObject) in m_entries)
                    GameObject.Destroy(l_gameObject.gameObject);

            m_entries = new Dictionary<CombinationPart, PartEntry>();
        }
    }
}