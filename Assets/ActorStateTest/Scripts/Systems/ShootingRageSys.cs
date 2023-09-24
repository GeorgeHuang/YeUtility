using ActorStateTest.Element;
using Cysharp.Threading.Tasks;
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

        private async void Setup()
        {
            await UniTask.WaitForSeconds(1);
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