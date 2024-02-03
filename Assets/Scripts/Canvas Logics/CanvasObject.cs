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
            canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);           
        }

        public void SetCanvasVisibility(bool isVisible) 
        {
            canvas.gameObject.SetActive(isVisible);
        }
    }
}