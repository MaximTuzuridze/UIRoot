using UnityEngine;

    public class GameplayExample : MonoBehaviour
    {
        private void Start()
        {
            UIRoot.Instance.Init();
        }

        [ContextMenu("lose")]
        private void Lose()
        {
            UIRoot.Instance.FinishGame(false);
        }

        [ContextMenu("win")]
        private void Win()
        {
            UIRoot.Instance.FinishGame(true);
        }
    }
