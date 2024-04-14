using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Summoning.Combination
{
    public class CombinedMonster : MonoBehaviour
    {
        public enum MonsterState
        {
            IDLE,
            MOVE,
            ATTACK
        }
        
        [Header("Configuration")]
        [SerializeField] private int m_health;
        [SerializeField] private int m_damage;
        [SerializeField] private float m_attackRate = 0.5f;
        [SerializeField] private AnimationCurve m_attackAnimation;
        [SerializeField] private CombinationPart m_part;
        [SerializeField] private CombinationPart m_armorType;
        [SerializeField] private CombinationPart m_damageType;

        [Header("References")]
        [SerializeField] private SpriteRenderer m_bodyRenderer;
        [SerializeField] private SpriteRenderer m_armRenderer;
        [SerializeField] private Animator m_bodyAnimator;
        [SerializeField] private Transform m_armJoint;
        [SerializeField] private TextMeshProUGUI m_healthDisplay;
        [SerializeField] private AudioSource m_damageSource;
        [SerializeField] private ParticleSystem m_healParticles;
        [SerializeField] private ParticleSystem m_damageParticles;
        [SerializeField] private ParticleSystem m_critDamageParticles;
        
        [Header("Resources")]
        [SerializeField] private AudioClip m_regularDamageClip;
        [SerializeField] private AudioClip m_criticalDamageClip;
        
        [Header("Props")]
        [SerializeField] private AudioSource m_audioSourcePrefab;
        [SerializeField] private AudioClip m_deathClip;

        private float m_attackTime;
        private bool m_isAttacking;
        private float m_slowTime;
        private float m_animationProgress;

        public CombinationPart Part => m_part;
        public CombinationPart DamageType => m_damageType;
        public CombinationPart BodyType => m_armorType;
        
        public MonsterState CurrentState { get; set; }

        public void Tick(float p_deltaTime)
        {
            if (m_healthDisplay != null) m_healthDisplay.text = m_health.ToString();

            if (m_isAttacking)
            {
                m_animationProgress += p_deltaTime * (1.0f / m_attackRate);
                Vector3 l_rotation = m_armJoint.eulerAngles;
                l_rotation.z = m_attackAnimation.Evaluate(m_animationProgress);
                m_armJoint.eulerAngles = l_rotation;

                if (m_animationProgress >= 1.0f)
                {
                    m_isAttacking = false;
                }
            }

            if (m_slowTime > 0)
            {
                m_slowTime -= p_deltaTime;
            }
        }

        public event Action Died;

        public void Init(CombinationData p_armData, CombinationData p_bodyData)
        {
            // body data
            m_bodyRenderer.sprite = p_bodyData.body;
            m_health = p_bodyData.health;
            m_armorType = p_bodyData.part;
            m_bodyAnimator.runtimeAnimatorController = p_bodyData.animatorController;

            // arm data
            m_armRenderer.sprite = p_armData.arms;
            m_damage = p_armData.damage;
            m_damageType = p_armData.part;

            m_attackTime = 0;
            m_isAttacking = false;
            
            m_slowTime = 0;
        }

        public void TakeDamage(int p_amount, bool p_isCrit = false)
        {
            m_health -= p_amount;

            m_damageSource.clip = p_isCrit ? m_criticalDamageClip : m_regularDamageClip;
            m_damageSource.Play();
            if (p_isCrit) m_critDamageParticles.Play();
            else m_damageParticles.Play();
            if (m_health <= 0) OnDeath();
        }

        public void Heal(int p_amount)
        {
            m_health += p_amount;
            m_healParticles.Play();
        }

        public void TickAttack(CombinedMonster p_target, float p_deltaTime)
        {
            float l_attackRate = m_slowTime > 0 ? m_attackRate * 2.0f : m_attackRate;
            m_attackTime += p_deltaTime;
            if (m_attackTime > l_attackRate)
            {
                m_attackTime = 0.0f;
                bool l_isCrit = CombinedMonster.IsCrit(m_damageType, p_target.m_armorType);
                int l_damage = l_isCrit ? m_damage * 2 : m_damage;
                p_target.TakeDamage(l_damage, l_isCrit);

                // SLOW 5 SECONDS 
                if (m_damageType == CombinationPart.WATER)
                {
                    p_target.m_slowTime = 5.0f;
                }

                if (p_target.m_armorType == CombinationPart.WATER)
                {
                    this.m_slowTime = 5.0f;
                }
                this.StartAttackAnimation();
            }
        }

        public void Kill()
        {
            m_health = 0;

            OnDeath();
        }

        private void StartAttackAnimation()
        {
            m_isAttacking = true;
            m_animationProgress = 0.0f;
        }

        private void OnDeath()
        {
            AudioSource l_source = GameObject.Instantiate(m_audioSourcePrefab, this.transform.position, quaternion.identity);
            l_source.clip = m_deathClip;
            l_source.Play();
            Destroy(l_source.gameObject, 1.5f);
            Died?.Invoke();
            Destroy(gameObject);
        }

        public static bool IsCrit(CombinationPart p_origin, CombinationPart p_target)
        {
            return (p_origin == CombinationPart.WATER && p_target is CombinationPart.EARTH or CombinationPart.FIRE) ||
                   (p_origin == CombinationPart.FIRE && (p_target == CombinationPart.PLANT)) ||
                   (p_origin == CombinationPart.EARTH && (p_target == CombinationPart.ELECTRICITY)) ||
                   (p_origin == CombinationPart.ELECTRICITY && (p_target == CombinationPart.ELECTRICITY));
        }
    }
}