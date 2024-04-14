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
        
        public void OnDrop(PointerEventData p_eventData)
        {
            if (m_dropAsset.TryGetDrop(p_eventData.pointerDrag.GetComponent<PartEntry>().Part, out Sprite l_sprite))
            {
                m_image.sprite = l_sprite;
                m_image.color = Color.white;
            }
            PartGiven?.Invoke(p_eventData.pointerDrag.GetComponent<PartEntry>().Part);
        }

        public void OnReset()
        {
            m_image.sprite = null;
            m_image.color = Color.clear;
        }

        public event Action<CombinationPart> PartGiven;
    }
}