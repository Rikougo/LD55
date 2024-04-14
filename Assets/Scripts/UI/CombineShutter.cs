using UnityEngine;
using UnityEngine.EventSystems;

namespace Summoning.UI
{
    public class CombineShutter : UIBehaviour
    {
        [SerializeField] private AnimationCurve m_animationCurve;
        [SerializeField] private float m_animationTime;

        private float m_animationStart;
        private bool m_isOpen;
        private bool m_isAnimating;

        protected override void Start()
        {
            base.Start();
            m_animationStart = Time.time;
            m_isOpen = false;
            m_isAnimating = false;
        }

        public void ToggleOpen()
        {
            m_isOpen = !m_isOpen;
            m_isAnimating = true;
            m_animationStart = Time.time;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !m_isAnimating)
            {
                this.ToggleOpen();
            }
            
            if (m_isAnimating)
            {
                float l_progress = (Time.time - m_animationStart) / m_animationTime;
                float l_yPos = m_animationCurve.Evaluate(m_isOpen ? 1.0f - l_progress : l_progress);
                Vector2 l_pos = (this.transform as RectTransform).anchoredPosition;
                l_pos.y = l_yPos;
                (this.transform as RectTransform).anchoredPosition = l_pos;

                if (Time.time - m_animationStart > m_animationTime)
                {
                    m_isAnimating = false;
                }
            }
        }
    }
}