using System;
using ActorStateTest.Data;
using ActorStateTest.Systems;
using NUnit.Framework;
using YeActorState;
using YeActorState.RuntimeCore;
using YeUtility.EditorHelper;
using Zenject;

namespace Test.MainTest
{
    [TestFixture]
    public class ActorMgrTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<ActorDataRepo>();
            Container.BindInstance(repo);
            Container.Bind<YeActorStateSys>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActorMgr>().AsSingle();
        }
        
        [Test]
        public void ActorMgrCreatePlayerErrorTest()
        {
            var actorMgr = Container.Resolve<ActorMgr>();
            var throwAssert = false;
            try
            {
                actorMgr.CreatePlayer("aaa");
            }
            catch (Exception)
            {
                throwAssert = true;
            }
            Assert.IsTrue(throwAssert, "沒丟出Assert");
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        // [UnityEngine.TestTools.UnityTest]
        // public System.Collections.IEnumerator ActorMgrTestWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // yield to skip a frame
        //     yield return null;
        // }
    }
}