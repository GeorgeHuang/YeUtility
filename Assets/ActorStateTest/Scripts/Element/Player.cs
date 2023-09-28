using System;
using System.Collections.Generic;
using ActorStateTest.Data;
using ActorStateTest.Systems;
using UnityEditor;
using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Element
{
    public class Player : MonoBehaviour, IInitializable
    {
        [Inject] private TimeSys timeSys;
        [Inject] private MoveHandler moveHandler;
        [Inject] private List<Collider> colliders;

        [SerializeField] private Vector3 gizmosHpOffset;

        public IPropertyProvider PropertyProvider { get; set; }

        public Transform Trans { get; private set; }

        public void Move(Vector3 dir)
        {
            moveHandler.Move(dir);
        }

        public void Initialize()
        {
            Trans = transform;
        }

        public float GetProperty(string propertyName)
        {
            return PropertyProvider.GetRuntimeProperty(propertyName);
        }

        public void SetPos(Vector3 pos)
        {
            Trans.position = pos;
        }

        public Vector3 GetPos()
        {
            return Trans.position;
        }

        // private void OnDrawGizmos()
        // {
        //     if (PropertyProvider == null) return;
        //     var hp = PropertyProvider.GetRuntimeProperty("CurHp");
        //     Handles.Label(transform.position + gizmosHpOffset, $"HP: {hp}");
        // }
        public IEnumerable<Collider> GetColliders()
        {
            return colliders;
        }
    }
}