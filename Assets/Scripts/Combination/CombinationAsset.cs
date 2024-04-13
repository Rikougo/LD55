using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Summoning.Combination
{
    [Serializable]
    public struct CombinationData
    {
        public Sprite icon;
        public Sprite body;
        public Sprite arms;
        public RuntimeAnimatorController animatorController;

        public int health;
        public int damage;

        [HideInInspector] public CombinationPart part;
    }

    [CreateAssetMenu(menuName = "Summoning/Combination Asset", fileName = "Combinations")]
    public class CombinationAsset : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<CombinationPart, CombinationData> m_combinationParts;

        public bool TryGetPart(CombinationPart p_part, out CombinationData p_data)
        {
            var l_found = m_combinationParts.TryGetValue(p_part, out p_data);
            if (!l_found) return false;
            p_data.part = p_part;
            return true;
        }
    }
}