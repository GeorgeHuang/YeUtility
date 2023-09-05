using UnityEngine;

namespace CommonUnit
{
    public class SMovableObject : MonoBehaviour , IMoveObj
    {

        public bool blockInput = false;
        public bool Is2D = false;
        public float mMoveRatio = 0.1f;
        public float mTurnSpeed = 0.1f;

        protected Vector3 mInputValue = Vector3.zero;
        protected Vector3 mCurTempPos = Vector3.zero;
        protected Vector3 mMoveDelta = Vector3.zero;
        protected Vector3 mLastFramePosDelta = Vector3.zero;

        protected Transform mTrans;
        //參考軸向，通常是因為Camera面向不同
        protected Transform mReferenceTrans;

        private bool needOnGround = false;
        private bool posDirty = false;
        private bool dirDirty = false;
        private bool useFaceDir = false;
        private float lastX = 0;
        private float effecSpeedRatio = 1;
        private float dotSpeedRatio = 1;
        private Vector3 mFaceDir;
        private Vector3 cameraForward;
        private Vector3 mLastPos = Vector3.zero;
        private Vector3 mCurTempDir = Vector3.zero;
        private Vector3 mNextTempPos = Vector3.zero;
        private Transform cameraTrans;
        private IMoveGround moveGround = null;
        private bool parallelProc = false;

        #region Get Set
        public bool UseFaceDir { get => useFaceDir; set => useFaceDir = value; }
        public bool NeedOnGround { get => needOnGround; set => needOnGround = value; }
        public float EffectSpeedRatio { get => effecSpeedRatio; set => effecSpeedRatio = value; }
        public Vector3 LastFramePosDelta { get { return mLastFramePosDelta; } set { mLastFramePosDelta = value; } }
        public Vector3 CurPos { get { return mCurTempPos; } }
        public Vector3 InputValue { get { return mInputValue; } }
        public Vector3 CurTempDir { get => mCurTempDir; set => mCurTempDir = value; }
        public IMoveGround MoveGround { get => moveGround; set => moveGround = value; }
        public bool ParallelProc { get => parallelProc; set => parallelProc = value; }
        public float DotSpeedRatio { get => dotSpeedRatio; set => dotSpeedRatio = value; }
        #endregion

        #region MonoBehaviour
        public virtual void Awake()
        {
            mTrans = transform;
        }
        public virtual void Start()
        {
        }

        #endregion

        #region public method
        public virtual void update()
        {
            ApplyInput();
            UpdateDir(mInputValue);
            UpdateTransFromInfo();
        }
        virtual public void init(Vector3 pos)
        {
            effecSpeedRatio = 1;
            cameraTrans = Camera.main.transform;
            cameraForward = cameraTrans.forward;
            mTrans.position = pos;
            mCurTempPos = pos;
            posDirty = false;
            dirDirty = false;
            mCurTempDir = Vector3.zero;
        }

        virtual public void setSpeed(float value)
        {
            mMoveRatio = value;
        }

        public float getSpeed()
        {
            return mMoveRatio;
        }

        virtual public void Stop()
        {
            mInputValue = Vector3.zero;
        }

        public void Input(Vector3 input)
        {
            Move(input);
        }

        public bool HasPosDelta()
        {
            return mLastFramePosDelta.magnitude > 0.001f;
        }

        public void setReferenceTrans(Transform trans)
        {
            mReferenceTrans = trans;
        }

        public void clearLastDelta()
        {
            mLastFramePosDelta.x = 0;
            mLastFramePosDelta.y = 0;
            mLastFramePosDelta.z = 0;
        }
        public void Move(Vector2 value)
        {
            mInputValue = value;
            if (mReferenceTrans != null)
            {
                mInputValue = value;
                if (Is2D == false)
                {
                    Vector3 temp = mInputValue;
                    temp.z = temp.y; temp.y = 0;
                    temp = mReferenceTrans.rotation * temp;
                    temp.y = temp.z; temp.z = 0;
                    mInputValue = temp;
                }
            }
        }
        public void Move(float x, float y)
        {
            mInputValue.x = x;
            mInputValue.y = y;
        }

        virtual public void setEnable(bool enable)
        {

        }

        public void FaceDir(Vector3 dir)
        {
            mFaceDir = dir;
        }

        virtual public void reset()
        {
        }

        virtual public void MoveOffset(Vector3 offset)
        {
            mInputValue = offset;
            //print($"offset {offset} cur {mCurTempPos}");
            SetPos(offset + mCurTempPos);
        }
        public void MoveDelta(Vector3 deltaPos)
        {
            //print("1");
            var nextPos = mCurTempPos += deltaPos;
            if (needOnGround && moveGround != null)
            {
                if (moveGround.HasGround(nextPos))
                {
                    SetPos(nextPos);
                }
            }
            else
            {
                SetPos(nextPos);
            }
        }
        public void SetKinematic(bool val)
        {
            GetComponent<Rigidbody>().isKinematic = val;

            if (GetComponent<Collider>() != null)
            {
                GetComponent<Collider>().enabled = !val;
            }

            Collider[] array = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider cd in array)
            {
                cd.enabled = !val;
            }
        }
        public void UpdateDir(Vector3 inputDir)
        {
            if (Is2D == false)
            {
                SetDir(inputDir);
            }
            else
            {
                var useValue = inputDir.x;
                if (useFaceDir)
                {
                    useValue = mFaceDir.x;
                }
                else
                {
                    //沒有輸入就維持上一次的結果
                    if (inputDir.sqrMagnitude < 0.0001f)
                    {
                        useValue = lastX;
                    }
                }

                lastX = useValue;

                if (useValue > 0)
                {
                    if ((mCurTempDir + cameraForward).sqrMagnitude > 0.001f)
                    {
                        SetDir(-cameraForward);
                    }
                }
                else
                {
                    if ((mCurTempDir - cameraForward).sqrMagnitude > 0.001f)
                    {
                        SetDir(cameraForward);
                    }
                }

            }
        }

        public void UpdateInfoFromTrans(Transform trans = null)
        {
            //print("2");
            if (posDirty)
                return;
            mCurTempPos = trans?trans.position:mTrans.position;
        }
        //給外面平行處理用
        public void UpdateInfoFromTrans(Vector3 pos)
        {
            //print("3");
            mCurTempPos = pos;
        }
        public void UpdateTransFromInfo()
        {
            //如果給外面平行處理就不做
            if (posDirty && !parallelProc)
            {
                //print($"apply pos {mCurTempPos}");
                mTrans.position = mCurTempPos;
                posDirty = false;
            }

            if (dirDirty)
            {
                dirDirty = false;
                mTrans.forward = mCurTempDir;
            }
            mLastFramePosDelta = mCurTempPos - mLastPos;
            mLastPos = mCurTempPos;
        }
        public void SetPosImmediate(Vector3 newPos)
        {
            //print($"Set Pos Immediate {newPos}");
            mCurTempPos = newPos;
            mTrans.position = newPos;
        }
        public void SetPos(Vector3 newPos)
        {
            //print($"Set Pos {newPos}");
            mCurTempPos = newPos;
            posDirty = true;
        }
        public Vector3 GetPos()
        {
            return mCurTempPos;
        }
        #endregion

        #region private method
        virtual protected void ApplyInput()
        {
            if (blockInput) return;
            if (mInputValue.magnitude < 0.1f)
            {
                return;
            }

            mInputValue = mInputValue.normalized;

            mMoveDelta = mInputValue * Time.deltaTime;


            mNextTempPos.x = mCurTempPos.x + mMoveDelta.x * mMoveRatio * effecSpeedRatio * dotSpeedRatio;

            if (Is2D)
            {
                mNextTempPos.y = mCurTempPos.y + mMoveDelta.y * mMoveRatio * effecSpeedRatio * dotSpeedRatio;
            }
            else
            {
                mNextTempPos.z = mCurTempPos.z + mMoveDelta.y * mMoveRatio * effecSpeedRatio * dotSpeedRatio;
            }

            if (needOnGround && moveGround != null)
            {
                if (moveGround.HasGround(mNextTempPos))
                {
                    mLastFramePosDelta = mNextTempPos - mTrans.position;
                    SetPos(mNextTempPos);
                }
            }
            else
            {
                mLastFramePosDelta = mCurTempPos - mTrans.position;
                SetPos(mNextTempPos);
            }

        }
        void SetDir(Vector3 dir)
        {
            mCurTempDir = dir;
            dirDirty = true;
        }
        #endregion
    }
}