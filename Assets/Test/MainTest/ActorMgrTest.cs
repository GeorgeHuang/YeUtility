using ActorStateTest.Data;
using ActorStateTest.Systems;
using NUnit.Framework;
using Zenject;

namespace Test.MainTest
{
    [TestFixture]
    public class ActorMgrTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            var repo = OdinUnit.OdinEditorHelpers.GetScriptableObject<ActorDataRepo>();
            Container.BindInstance(repo);
            Container.BindInterfacesAndSelfTo<ActorMgr>().AsSingle();
        }
        
        [Test]
        public void ActorMgrTestSimplePasses()
        {
            var actorMgr = Container.Resolve<ActorMgr>();
            var handler = actorMgr.CreatePlayer("aaa");
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