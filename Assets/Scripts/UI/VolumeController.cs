using System;
using UnityEngine;
using UnityEngine.UI;

namespace Summoning.UI
{
    public class VolumeController : MonoBehaviour
    {
        [SerializeField] private Slider m_slider;

        private void Awake()
        {
            AudioListener.volume = 0.5f;
        }
        
        private void OnEnable()
        {
            m_slider.SetValueWithoutNotify(AudioListener.volume);
            m_slider.onValueChanged.AddListener(this.ValueChanged);
        }

        private void OnDisable()
        {
            m_slider.onValueChanged.RemoveListener(this.ValueChanged);
        }

        private void ValueChanged(float p_value)
        {
            AudioListener.volume = p_value;
        }
    }
}