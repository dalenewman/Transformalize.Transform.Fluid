using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Transformalize.Configuration;
using Transformalize.Containers.Autofac;
using Transformalize.Contracts;
using Transformalize.Logging;
using Transformalize.Providers.Bogus.Autofac;
using Transformalize.Providers.Console.Autofac;
using Transformalize.Transform.Fluid.Autofac;

namespace Test.Integration.Core {
   [TestClass]
   public class UnitTest1 {
      [TestMethod]
      public void TestMethod1() {

         var logger = new NullLogger();
         using (var outer = new ConfigurationContainer(new FluidTransformModule()).CreateScope(@"files\bogus-with-transform.xml", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new Container(new FluidTransformModule(), new BogusModule(), new ConsoleProviderModule(process)).CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }

      }
   }
}
