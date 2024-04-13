using System;
using Summoning.Combination;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Summoning.UI
{
    public class CombineSlot : UIBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData p_eventData)
        {
            Debug.Log($"Received {p_eventData.pointerDrag.GetComponent<PartEntry>().Part}");
            PartGiven?.Invoke(p_eventData.pointerDrag.GetComponent<PartEntry>().Part);
        }

        public event Action<CombinationPart> PartGiven;
    }
}