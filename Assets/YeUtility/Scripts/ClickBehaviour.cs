using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonUnit
{
    public class ClickBehaviour : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public System.Action<ClickBehaviour> OnClick;
        public System.Action OnPress;
        public System.Action OnUp;
        public GameObject m_object;
        public string m_method;
        public PointerEventData pointerEventData;

        #region MonoBehaviour
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnMouseDown()
        {
            if (OnPress != null)
            {
                OnPress();
            }
        }

        void OnMouseUp()
        {
            if (OnUp != null)
            {
                OnUp();
            }
        }

        void OnMouseUpAsButton()
        {
            //Common.sysPrint("OnMouseUpAsButton");
            if (OnClick != null)
            {
                OnClick(this);
            }

            if (m_object != null && string.IsNullOrEmpty(m_method) == false)
            {
                m_object.SendMessage(m_method);
            }

            OnMouseUp();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);

            if (m_object != null && string.IsNullOrEmpty(m_method) == false)
            {
                m_object.SendMessage(m_method);
            }
            pointerEventData = eventData;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPress?.Invoke();
            pointerEventData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
            pointerEventData = eventData;
        }
        #endregion
    }
}
