using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class ButtonManager : MonoBehaviour
    {
        public void LoadAndStartLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
