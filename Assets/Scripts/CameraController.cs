using UnityEngine;

namespace Summoning
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private Vector2 m_xBounds;
        [SerializeField] private float m_speed;

        private void Update()
        {
            var l_position = m_camera.transform.position;
            if (Input.GetKey(KeyCode.LeftArrow)) l_position.x -= m_speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.RightArrow)) l_position.x += m_speed * Time.deltaTime;

            l_position.x = Mathf.Clamp(l_position.x, m_xBounds.x, m_xBounds.y);
            m_camera.transform.position = l_position;
        }
    }
}