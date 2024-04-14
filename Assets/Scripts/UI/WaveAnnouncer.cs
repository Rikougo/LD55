using System;
using TMPro;
using UnityEngine;

namespace Summoning.UI
{
    public class WaveAnnouncer : MonoBehaviour
    {
        [SerializeField] private GameController m_gameController;
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private AnimationCurve m_opacityAnimation;
        [SerializeField] private float m_animationTime = 1.0f;

        private float m_animationTimer;

        private void OnEnable()
        {
            m_gameController.WaveCleared += this.OnWaveCleared;
            m_gameController.NewWaveStarted += this.WaveStarted;
        }

        private void OnDisable()
        {
            m_gameController.WaveCleared -= this.OnWaveCleared;
            m_gameController.NewWaveStarted -= this.WaveStarted;
        }

        private void Update()
        {
            if (m_animationTimer > 0)
            {
                m_animationTimer -= Time.deltaTime;
                if (m_animationTimer < 0)
                {
                    m_text.enabled = false;
                    return;
                }
                
                float l_progress = (m_animationTimer / m_animationTime);
                float l_opacity = m_opacityAnimation.Evaluate(l_progress);
                Color l_color = m_text.color;
                l_color.a = l_opacity;
                m_text.color = l_color;

            }
        }

        private void OnWaveCleared()
        {
            m_text.text = "Wave cleared !";
            this.StartAnimation();
        }

        private void WaveStarted(int p_number)
        {
            m_text.text = $"Wave {p_number}";
            this.StartAnimation();
        }

        private void StartAnimation()
        {
            m_animationTimer = m_animationTime;
            m_text.enabled = true;
        }
    }
}