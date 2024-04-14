using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Summoning.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button m_playButton;
        [SerializeField] private Button m_creditButton;
        [SerializeField] private Button m_closeCreditButton;
        [SerializeField] private Image m_creditImage;

        private void OnEnable()
        {
            m_playButton.onClick.AddListener(this.OnStartGame);
            m_creditButton.onClick.AddListener(this.ShowCredit);
            m_closeCreditButton.onClick.AddListener(this.CloseCredit);
        }

        private void OnStartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        private void ShowCredit()
        {
            m_creditImage.gameObject.SetActive(true);
        }

        private void CloseCredit()
        {
            m_creditImage.gameObject.SetActive(false);
        }
    }
}