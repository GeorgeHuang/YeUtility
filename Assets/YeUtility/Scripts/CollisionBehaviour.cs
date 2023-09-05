using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

namespace YeUtility
{
    public class CollisionBehaviour : MonoBehaviour
    {
        public enum ColliderType
        {
            None,
            Rect,
            Circle
        }

        public UnityEvent<CollisionBehaviour> OnContact;
        public UnityEvent<CollisionBehaviour> OnExit;
        public ColliderType ctype = ColliderType.None;
        public Transform trans;

        GameObject m_other;
        Collision m_collision;
        Collider m_collider;

        Collision2D m_collision2D;
        Collider2D m_collider2D;
        List<Collider2D> triggerObjects = new List<Collider2D>();
        
        Rect DetectRect;
        ContactFilter2D contactFilter2D = new ContactFilter2D();

        #region Gizmos
        public bool ShowGizmos = false;
        public Color DetectColor = Color.red;
        public Color NoDetectColor = Color.green;
        #endregion

        public UnityEngine.Collider OtherCollider
        {
            get { return m_collider; }
            set { m_collider = value; }
        }

        public UnityEngine.Collider2D OtherCollider2D
        {
            get { return m_collider2D; }
            set { m_collider2D = value; }
        }
        public UnityEngine.Collision2D OtherCollision2D
        {
            get { return m_collision2D; }
            set { m_collision2D = value; }
        }
        public UnityEngine.Collision CollisionObj
        {
            get { return m_collision; }
            set { m_collision = value; }
        }

        public UnityEngine.GameObject OtherObj
        {
            get { return m_other; }
            set { m_other = value; }
        }

        public float Radius
        {
            get { return transform.localScale.x * 0.5f; }
            set
            {
                var s = Trans.localScale;
                s.x = value * 2;
                Trans.localScale = s;
            }
        }

        public int TypeID { get; set; }

        public bool IncludeEnter { get; set; } = false;

        static readonly ProfilerMarker ProfilerMarker = new ProfilerMarker("CollisionBehaviour");

        public Transform Trans
        {
            get
            {
                if (trans == null)
                    trans = transform;
                return trans;
            }
        }    

        void OnCollisionEnter(Collision collision)
        {
            if (!IncludeEnter) return;
            CollisionObj = collision;
            OtherObj = collision.collider.gameObject;
            OnContact?.Invoke(this);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!IncludeEnter) return;
            OtherCollider = other;
            OtherObj = other.gameObject;
            OnContact?.Invoke(this);
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            OtherCollider = collisionInfo.collider;
            OtherObj = OtherCollider.gameObject;
            OnContact?.Invoke(this);
        }

        void OnTriggerStay(Collider other)
        {
            OtherCollider = other;
            OtherObj = other.gameObject;
            OnContact?.Invoke(this);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IncludeEnter) return;
            OtherCollision2D = collision;
            OtherObj = collision.collider.gameObject;
            OnContact?.Invoke(this);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OtherCollision2D = collision;
            OtherObj = collision.collider.gameObject;
            OnContact?.Invoke(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IncludeEnter) return;
            OtherCollider2D = other;
            OtherObj = other.gameObject;
            OnContact?.Invoke(this);
        }
        void OnTriggerStay2D(Collider2D other)
        {
            OtherCollider2D = other;
            OtherObj = other.gameObject;
            OnContact?.Invoke(this);

            if (!triggerObjects.Contains(other))
            {
                triggerObjects.Add(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OtherCollider2D = other;
            OtherObj = other.gameObject;
            OnExit?.Invoke(this);
            triggerObjects.Remove(other);
        }

        void OnParticleCollision(GameObject other)
        {
            OtherObj = other;
            OnContact?.Invoke(this);
        }

        public void Init()
        {
            contactFilter2D.NoFilter();
            trans = trans ? trans : transform;
        }

        public int Detect(Collider2D[] results)
        {
            //contactFilter2D.NoFilter();
            using (ProfilerMarker.Auto())
            {
                switch (ctype)
                {
                    case ColliderType.Rect:
                    {
                        UpdateRect();
                        var rect = DetectRect;
                        //LayerMask layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
                        //return Physics2D.OverlapBoxAll(rect.center, rect.size, transform.eulerAngles.z, 1 << layerMask);
                        //LayerMask layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
                        //return Physics2D.OverlapBoxAll(rect.center, rect.size, transform.eulerAngles.z);
                        return Physics2D.OverlapBox(rect.center, rect.size, Trans.eulerAngles.z, contactFilter2D ,results);
                    }
                    case ColliderType.Circle:
                        //return Physics2D.OverlapCircleAll(transform.position, transform.lossyScale.x * 0.5f);
                        return Physics2D.OverlapCircle(Trans.position, Trans.lossyScale.x * 0.5f, contactFilter2D, results);
                    case ColliderType.None:
                    default:
                        results = triggerObjects.ToArray();
                        break;
                }
            }
            //return triggerObjects.ToArray();
            return triggerObjects.Count;
        }

        void UpdateRect()
        {
            DetectRect.center = trans.position;
            DetectRect.size = trans.lossyScale;
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            if (ShowGizmos == false) return;
            
            var gizmosColor = triggerObjects.Count > 0 ? DetectColor : NoDetectColor;

            switch (ctype)
            {
                case ColliderType.Rect:
                {
                    UpdateRect();
                    var m = Matrix4x4.Rotate(transform.rotation);
                    Gizmos.matrix = m;
                    Gizmos.color = gizmosColor;
                    Gizmos.DrawCube(m.inverse * trans.position, DetectRect.size);
                    break;
                }
                case ColliderType.Circle:
                    Gizmos.color = gizmosColor;
                    Gizmos.DrawSphere(transform.position, trans.lossyScale.x * 0.5f);
                    break;
                case ColliderType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}
