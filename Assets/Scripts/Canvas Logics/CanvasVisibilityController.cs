using UnityEngine;

namespace WeatherGuardian.CanvasManager 
{    
    public class CanvasVisibilityController : MonoBehaviour
    {
        [SerializeField] private CanvasObject canvasObject;

        public CanvasObject myCanvasObject 
        {
            get 
            {
                return canvasObject;
            }
        }

        public void ShowCanvas()
        {
            myCanvasObject.SetCanvasVisibility(true);
        }

        public void HideCanvas()
        {
            myCanvasObject.SetCanvasVisibility(false);
        }
    }
}