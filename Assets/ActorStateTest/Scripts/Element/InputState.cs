using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace ActorStateTest.Element
{
    public class InputState : ITickable
    {
        private readonly Subject<Vector2> movePress = new();
        private readonly Subject<Unit> spaceKeyPress = new();
        private readonly Subject<Unit> debugKeyPress = new();
        public IObservable<Vector2> MovePress => movePress;
        public IObservable<Unit> SpaceKeyPress => spaceKeyPress;

        public IObservable<Unit> DebugKeyPress => debugKeyPress;

        public void Tick()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A))
                movePress.OnNext(Vector2.left);
            if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
                movePress.OnNext(Vector2.right);
            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
                spaceKeyPress.OnNext(Unit.Default);
            if (Input.GetKeyUp(KeyCode.F1))
                debugKeyPress.OnNext(Unit.Default);
        }
    }
}