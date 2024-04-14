using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Summoning
{
    public class PostProcessController : MonoBehaviour
    {
        [SerializeField] private GameController m_gameController;
        [SerializeField] private AnimationCurve m_saturationCurve;
        [SerializeField] private PostProcessVolume m_volume;

        private float m_animationTimer;

        private void OnEnable()
        {
            m_gameController.GameOver += this.OnGameOver;
        }
        
        private void OnDisable()
        {
            m_gameController.GameOver += this.OnGameOver;
        }

        private void OnGameOver()
        {
            m_animationTimer = 10.0f;
        }

        private void Update()
        {
            if (m_animationTimer > 0)
            {
                m_animationTimer -= Time.deltaTime;
                if (m_animationTimer < 0)
                {
                    return;
                }
                
                float l_progress = (m_animationTimer / 10.0f);
                float l_opacity = m_saturationCurve.Evaluate(l_progress);
                m_volume.profile.GetSetting<ColorGrading>().saturation.value = l_opacity;
            }
        }
    }
}