using UnityEngine;
using System.Collections;

namespace CommonUnit
{
    public class NaviMovable : SMovableObject
    {

        bool useVelocity = false;
        protected UnityEngine.AI.NavMeshAgent agent;

        public bool UseVelocity { get { return useVelocity; } set { useVelocity = value; } }

        public override void Awake()
        {
            base.Awake();
            agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public override void Start()
        {
            base.Start();
            if (Is2D)
            {
                agent.updateRotation = false;
                agent.updateUpAxis = false;
            }
        }


        public override void update()
        {
            base.update();
            //mLastFramePosDelta = mlastPos - mTrans.position;
            //mlastPos = mTrans.position;
            if (useVelocity)
            {
                mLastFramePosDelta = agent.velocity;
            }
        }

        public override void init(Vector3 pos)
        {
            reset();
        }

        //public override void MoveToWithDir(Vector3 pos, Vector3 dir)
        //{
        //    if (Common.compareVector3(pos, mTrans.position, 0.00001f)) return;
        //    mLastFramePosDelta = pos - mTrans.position;
        //    agent.velocity = mLastFramePosDelta;
        //    mTrans.position = pos;
        //    mTrans.forward = dir;
        //}

        public override void MoveOffset(Vector3 offset)
        {
            if (agent.enabled == false) return;
            agent.Move(offset);
            mLastFramePosDelta = offset;
        }

        public void MoveTo(Vector3 pos)
        {
            if (agent.enabled == false) return;
            agent.destination = pos;
        }

        public override void Stop()
        {
            if (agent.enabled == false) return;
            if (agent.hasPath)
            {
                agent.ResetPath();
            }
            agent.velocity = Vector3.zero;
        }

        public override void setSpeed(float value)
        {
            base.setSpeed(value);
            agent.speed = value;
        }

        public override void reset()
        {
            if (gameObject.activeInHierarchy == false) return;
            mLastFramePosDelta = Vector3.zero;
            StartCoroutine(resetProc());
        }

        public override void setEnable(bool enable)
        {
            base.setEnable(enable);
            agent.enabled = enable;
        }

        IEnumerator resetProc()
        {
            if (agent.enabled == true)
            {
                if (agent.hasPath)
                {
                    agent.ResetPath();
                }
                agent.velocity = Vector3.zero;
                agent.angularSpeed = mTurnSpeed;
                agent.enabled = false;
                yield return new WaitForEndOfFrame();
            }
            agent.enabled = true;
        }
    }
}
