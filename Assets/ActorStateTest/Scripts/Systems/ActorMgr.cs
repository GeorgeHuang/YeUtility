using System;
using System.Collections.Generic;
using System.ComponentModel;
using ActorStateTest.Data;
using ActorStateTest.Element;
using UnityEngine;
using UnityEngine.Assertions;
using YeActorState;
using Zenject;
using Object = UnityEngine.Object;

namespace ActorStateTest.Systems
{
    public class ActorMgr
    {
        [Inject] private ActorDataRepo actorDataRepo;
        [Inject] private YeActorStateSys yeActorStateSys;
        [Inject] private DiContainer container;
        private bool bind = false;

        public ActorHandler CreatePlayer(string name)
        {
            var actorData = actorDataRepo.GetData(name);
            Assert.IsNotNull(actorData, $"沒找到角色 {name}");

            var yeActorHandler = yeActorStateSys.AddActor(actorData.yeActorBaseData);

            var perimeter = new List<object> { yeActorHandler };

            // container.Bind<MoveHandler>().FromSubContainerResolve().ByInstaller<PlayerInstaller>()
            //     .WithArguments(perimeter).WhenInjectedInto<Player>().NonLazy();
            // Debug.Log($"{container.HasBinding<Player>()}");
            // if (!bind)
            // {
            //     bind = true;
            //     //container.BindInterfacesAndSelfTo<Player>().FromComponentSibling().AsTransient();
            //     container.BindInterfacesAndSelfTo<MoveHandler>().FromSubContainerResolve()
            //         .ByMethod(c => PlayerInstallerMethod(c, yeActorHandler)).WhenInjectedInto<Player>();
            // }
            //
            //var player = container.InstantiatePrefabForComponent<Player>(actorData.modelPrefab, perimeter);
            var gameObject = Object.Instantiate(actorData.modelPrefab);
            var player = gameObject.GetComponent<Player>();
            var playerInstaller = player.GetComponent<PlayerMonoInstaller>();
            playerInstaller.ActorStateHandler = yeActorHandler;

            var gameContext = player.GetComponent<GameObjectContext>();
            gameContext.Install(container);
            
            player.Initialize();

            // var gameObject = Object.Instantiate(actorData.modelPrefab);
            // var player = gameObject.GetComponent<Player>();
            // container.Inject(player);

            ActorHandler rv = new();
            perimeter.Add(player.gameObject);
            perimeter.Add(player);
            container.Inject(rv, perimeter);

            return rv;
        }

        private void PlayerInstallerMethod(DiContainer diContainer, ActorStateHandler actorStateHandler)
        {
            //diContainer.Bind<Player>().FromComponentOnRoot().AsSingle();
            diContainer.BindInstance(actorStateHandler);
            diContainer.BindInterfacesAndSelfTo<MoveHandler>().AsSingle();
        }
    }
}