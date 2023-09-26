using System;
using ActorStateTest.Element;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ShootingRageSys : ITickable, IInitializable
    {
        [Inject] private InputState inputState;
        [Inject] private ShootingRageConfig config;
        [Inject] private ActorMgr actorMgr;

        private ActorHandler mainActorStateHandler;

        public void Initialize()
        {
            Setup();
        }

        // private async UniTaskVoid AAA()
        // {
        //     var task1 = UniTask.Delay(TimeSpan.FromSeconds(1));
        //     var source = new UniTaskCompletionSource<Collision>();
        //     var r = await UniTask.WhenAny(task1, source.Task);
        // }

        private async void Setup()
        {
            await UniTask.WaitForSeconds(0.25f);
            mainActorStateHandler = actorMgr.CreatePlayer(config.PlayerDataName);
            inputState.MovePress.Subscribe(InputMovePress);
        }

        private void InputMovePress(Vector2 dir)
        {
            mainActorStateHandler.Move(dir);
        }


        public void Tick()
        {
        }
    }
}