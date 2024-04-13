using System;
using Summoning.Combination;
using UnityEngine;

namespace Summoning.UI
{
    public class CombineController : MonoBehaviour
    {
        [SerializeField] private CombineSlot m_bodySlot;
        [SerializeField] private CombineSlot m_armSlot;
        private CombinationPart? m_registeredArm;

        private CombinationPart? m_registeredBody;

        private void OnEnable()
        {
            m_registeredArm = null;
            m_registeredBody = null;

            m_bodySlot.PartGiven += OnBodyPartGiven;
            m_armSlot.PartGiven += OnArmPartGiven;
        }

        private void OnDisable()
        {
            m_bodySlot.PartGiven -= OnBodyPartGiven;
            m_armSlot.PartGiven -= OnArmPartGiven;
        }

        public event Action<CombinationPart, CombinationPart> Combined;

        private void OnBodyPartGiven(CombinationPart p_part)
        {
            m_registeredBody = p_part;
            CombineIfPossible();
        }

        private void OnArmPartGiven(CombinationPart p_part)
        {
            m_registeredArm = p_part;
            CombineIfPossible();
        }

        private void CombineIfPossible()
        {
            if (m_registeredArm.HasValue && m_registeredBody.HasValue)
            {
                Combined?.Invoke(m_registeredArm.Value, m_registeredBody.Value);
                m_registeredArm = null;
                m_registeredBody = null;
            }
        }
    }
}