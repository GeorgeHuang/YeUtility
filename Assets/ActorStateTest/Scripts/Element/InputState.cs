﻿using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace ActorStateTest.Element
{
    public class InputState : ITickable
    {
        private Subject<Vector2> movePress = new();
        public IObservable<Vector2> MovePress => movePress;
        
        private Subject<Unit> spaceKeyPress = new();
        public IObservable<Unit> SpaceKeyPress => spaceKeyPress;

        public void Tick()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A))
                movePress.OnNext(Vector2.left);
            if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
                movePress.OnNext(Vector2.right);
            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
                spaceKeyPress.OnNext(Unit.Default);
        }
    }
}