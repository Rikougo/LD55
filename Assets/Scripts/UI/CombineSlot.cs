using System;
using Summoning.Combination;
using Summoning.Monster;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Summoning.UI
{
    public class CombineSlot : UIBehaviour, IDropHandler
    {
        [SerializeField] private MonsterDropAsset m_dropAsset;
        [SerializeField] private Image m_image;

        private PartEntry m_registeredEntry;
        
        public void OnDrop(PointerEventData p_eventData)
        {
            PartEntry l_entry = p_eventData.pointerDrag.GetComponent<PartEntry>();
            if (l_entry.Amount < 0) return;
            if (m_registeredEntry != null)
            {
                m_registeredEntry.Amount += 1;
            }
            m_registeredEntry = l_entry;
            m_registeredEntry.Amount -= 1;
            
            if (m_dropAsset.TryGetDrop(l_entry.Part, out Sprite l_sprite))
            {
                m_image.sprite = l_sprite;
                m_image.color = Color.white;
            }
            PartGiven?.Invoke(p_eventData.pointerDrag.GetComponent<PartEntry>().Part);
        }

        public void OnReset()
        {
            m_registeredEntry = null;
            m_image.sprite = null;
            m_image.color = Color.clear;
        }

        public event Action<CombinationPart> PartGiven;
    }
}