using UnityEngine;

namespace CommonUnit
{
    public class CheckPointCollisionBehaviour : MonoBehaviour
    {

        public System.Action<CheckPointCollisionBehaviour> OnContact;

        public enum CheckPointType
        {
            RoomExitPoint,
            RevivalPoint,
            RoomExitWall,
            None
        };

        public CheckPointType Type = CheckPointType.None;

        GameObject m_other;
        Collision m_collision;
        Collider m_collider;

        Collision2D m_collision2D;
        Collider2D m_collider2D;

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

        public int TypeID { get; set; }

        void OnCollisionEnter(Collision collision)
        {
            CollisionObj = collision;
            OtherObj = collision.collider.gameObject;
            if (OnContact != null)
                OnContact(this);
        }

        void OnTriggerEnter(Collider other)
        {
            OtherCollider = other;
            OtherObj = other.gameObject;
            if (OnContact != null)
                OnContact(this);
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            OtherCollider = collisionInfo.collider;
            OtherObj = OtherCollider.gameObject;
            if (OnContact != null)
                OnContact(this);
        }

        void OnTriggerStay(Collider other)
        {
            OtherCollider = other;
            OtherObj = other.gameObject;
            if (OnContact != null)
                OnContact(this);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            OtherCollision2D = collision;
            OtherObj = collision.collider.gameObject;
            if (OnContact != null)
                OnContact(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            OtherCollider2D = other;
            OtherObj = other.gameObject;
            if (OnContact != null)
                OnContact(this);
        }

        void OnParticleCollision(GameObject other)
        {
            OtherObj = other;
            if (OnContact != null)
                OnContact(this);
        }
    }
}
