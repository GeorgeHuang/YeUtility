﻿using UnityEngine;

namespace YeUtility
{
    public class YeTrackingObj : MonoBehaviour
    {
        public Transform targetObj;

        public float smoothTime = 0.2f;

        private Vector3 posVelocity = Vector3.zero;
        private Transform trans;

        public bool WithRotate { get; set; } = false;

        public bool Is2D { get; set; }
        public bool IsTrackPos { get; private set; }
        public Vector3 TargetPos { get; private set; }

        private void Awake()
        {
            trans = transform;
        }

        private void Start()
        {
            if (targetObj == null) return;
            IsTrackPos = false;
        }

        public void Tick()
        {
            //transform.position = targetObj.position;
            UpdatePos();


            if (targetObj == null) return;
            if (!WithRotate) return;
            if (Is2D)
                trans.right = targetObj.right;
            else
                trans.forward = targetObj.forward;
        }

        private void UpdatePos()
        {
            var pos = TargetPos;

            if (IsTrackPos == false)
            {
                if (targetObj != null)
                    pos = targetObj.transform.position;
                else
                    return;
            }

            trans.position = smoothTime < 0.0001f
                ? pos
                : Vector3.SmoothDamp(trans.position, pos, ref posVelocity, smoothTime);
        }

        public void SetTarget(Transform target, bool needResetPos = true)
        {
            IsTrackPos = false;
            targetObj = target;
            if (needResetPos && target) ResetPos(target.transform.position);
            posVelocity = Vector3.zero;
        }

        public void ResetPos(Vector3 pos)
        {
            transform.position = targetObj.position;
        }

        public void SetTrackPos(Vector3 targetPos)
        {
            IsTrackPos = true;
            TargetPos = targetPos;
        }
    }
}