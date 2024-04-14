using System.Collections.Generic;
using Summoning.Combination;
using UnityEngine;

namespace Summoning.UI
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private CombinationAsset m_combinationAsset;
        [SerializeField] private PartEntry m_partEntryPrefab;
        [SerializeField] private List<Transform> m_slots;

        private Dictionary<CombinationPart, PartEntry> m_entries;
        private List<Transform> m_slotsInUse;

        public void UpdateDisplay(Dictionary<CombinationPart, int> p_inventory)
        {
            foreach (var (l_combinationPart, l_value) in p_inventory)
            {
                if (!m_entries.ContainsKey(l_combinationPart))
                {
                    if (m_combinationAsset.TryGetPart(l_combinationPart, out var l_data))
                    {
                        Transform l_slot = this.ClaimFirstFreeSlot();
                        var l_instance = Instantiate(m_partEntryPrefab, l_slot);
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
            m_slotsInUse = new List<Transform>();
        }

        private Transform ClaimFirstFreeSlot()
        {
            foreach (var l_transform in m_slots)
            {
                if (!m_slotsInUse.Contains(l_transform))
                {
                    m_slotsInUse.Add(l_transform);
                    return l_transform;
                }
            }

            return null;
        }
    }
}