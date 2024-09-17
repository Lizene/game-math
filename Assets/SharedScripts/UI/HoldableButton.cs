using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMath.UI
{
    public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool isPointerDown;
        Button button;
        
        public bool IsHeldDown => isPointerDown;
        void Awake()
        {
            button = GetComponent<Button>();
        }
        void Update()
        {
            if (IsHeldDown)
            {
                button.onClick.Invoke();
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            isPointerDown = false;
        }
    }
}