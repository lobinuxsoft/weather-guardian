using UnityEngine;

namespace WeatherGuardian.CanvasManager
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasObject : MonoBehaviour
    {
        [SerializeField] bool startsVisible;

        private Canvas canvas;

        public bool IsVisible
        {
            get 
            {
                return canvas.gameObject.activeSelf;
            }
        }

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            canvas.gameObject.SetActive(startsVisible);
        }

        public void SwitchCanvasVisibility()
        {
            if (canvas == null) //This is to fix a bug if the canvas is disable when starting the game and the awake is not called
            {
                canvas = GetComponent<Canvas>();
            }

            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);           
        }

        public void SetCanvasVisibility(bool isVisible) 
        {
            canvas.gameObject.SetActive(isVisible);
        }
    }
}