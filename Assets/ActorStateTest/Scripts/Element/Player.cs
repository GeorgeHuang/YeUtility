using System.Collections.Generic;
using ActorStateTest.Data;
using ActorStateTest.Scripts.UI;
using ActorStateTest.Systems;
using UniRx;
using UnityEngine;
using Zenject;

namespace ActorStateTest.Element
{
    public class Player : MonoBehaviour, IInitializable
    {
        [SerializeField] private Vector3 gizmosHpOffset;
        [Inject] private List<Collider> colliders;
        [Inject] private HpBar hpbar;
        [Inject] private MoveHandler moveHandler;
        [Inject] private TimeSys timeSys;

        public IPropertyProvider PropertyProvider { get; set; }

        public Transform Trans { get; private set; }

        public void Initialize()
        {
            Trans = transform;
        }

        public void Move(Vector3 dir)
        {
            moveHandler.Move(dir);
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

        public void Setup(ActorHandler actorHandler)
        {
            PropertyProvider = actorHandler;
            PropertyProvider.ObserveEveryValueChanged(x => x.GetRuntimeProperty("CurHp")).Subscribe(v =>
            {
                var maxHp = PropertyProvider.GetRuntimeProperty("Hp");
                hpbar.SetPercent(v / maxHp);
            }).AddTo(this);
        }
    }
}