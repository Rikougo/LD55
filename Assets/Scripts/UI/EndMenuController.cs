using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Summoning.UI
{
    public class EndMenuController : MonoBehaviour
    {
        [SerializeField] private Button m_replayButton;

        private void OnEnable()
        {
            m_replayButton.onClick.AddListener(this.Replay);
        }

        private void Replay()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}