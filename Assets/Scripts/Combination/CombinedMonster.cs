using System;
using TMPro;
using UnityEngine;

namespace Summoning.Combination
{
    public class CombinedMonster : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_bodyRenderer;
        [SerializeField] private SpriteRenderer m_armRenderer;

        [SerializeField] private int m_health;
        [SerializeField] private int m_damage;

        [SerializeField] private TextMeshProUGUI m_healthDisplay;
        private CombinationPart m_armorType;

        private CombinationPart m_damageType;

        private void Update()
        {
            m_healthDisplay.text = m_health.ToString();
        }

        public event Action Died;

        public void Init(CombinationData p_armData, CombinationData p_bodyData)
        {
            // body data
            m_bodyRenderer.sprite = p_bodyData.body;
            m_health = p_bodyData.health;
            m_armorType = p_bodyData.part;

            // arm data
            m_armRenderer.sprite = p_armData.arms;
            m_damage = p_armData.damage;
            m_damageType = p_armData.part;
        }

        public void TakeDamage(int p_amount)
        {
            m_health -= p_amount;
            if (m_health <= 0) OnDeath();
        }

        public void Kill()
        {
            m_health = 0;

            OnDeath();
        }

        private void OnDeath()
        {
            Died?.Invoke();
            Destroy(gameObject);
        }
    }
}