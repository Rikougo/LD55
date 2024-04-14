using Summoning.Combination;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Summoning.UI
{
    public class PartEntry : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image m_dragObjectPrefab;

        [SerializeField] private Image m_partDisplay;
        [SerializeField] private TextMeshProUGUI m_partAmountText;

        private Image m_dragImage;

        public CombinationPart Part { get; set; }

        public Sprite Sprite
        {
            get => m_partDisplay.sprite;
            set => m_partDisplay.sprite = value;
        }

        public int Amount
        {
            get => int.Parse(m_partAmountText.text);
            set => m_partAmountText.text = value.ToString();
        }

        public void OnBeginDrag(PointerEventData p_eventData)
        {
            if (this.Amount < 0) return;
            
            m_dragImage = Instantiate(m_dragObjectPrefab, p_eventData.position, quaternion.identity,
                                      transform);
            m_dragImage.sprite = Sprite;
        }

        public void OnDrag(PointerEventData p_eventData)
        {
            if (m_dragImage == null) return;
            
            m_dragImage.transform.position = p_eventData.position;
        }

        public void OnEndDrag(PointerEventData p_eventData)
        {
            if (m_dragImage == null) return;
            
            Destroy(m_dragImage.gameObject);
            m_dragImage = null;
        }
    }
}