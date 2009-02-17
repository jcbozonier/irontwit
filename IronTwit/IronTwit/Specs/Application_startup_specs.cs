using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronTwit.ViewModels;
using NUnit.Framework;
using SpecUnit;
using StructureMap;

namespace Specs
{
    [TestFixture]
    public class When_main_view_is_shown_for_the_first_time : context
    {
        [Test]
        public void It_should_ask_for_user_name_and_password()
        {
            Application_Asked_For_User_Name_And_Password.ShouldBeTrue();
        }

        protected override void Because()
        {
            Model.ApplicationStarting();
        }

        protected override void Context()
        {
            //throw new System.NotImplementedException();
        }
    }

    [TestFixture]
    public abstract class context
    {
        protected MainView Model;

        protected bool Application_Asked_For_User_Name_And_Password
        {
            get
            {
                return !String.IsNullOrEmpty(Model.UserName) && !String.IsNullOrEmpty(Model.Password);
            }
        }
        

        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerBootstrapper.BootstrapStructureMap();

            Model = ObjectFactory.GetInstance<MainView>();
            
            Context();
            Because();
        }

        protected abstract void Because();

        protected abstract void Context();
    }
}
