using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Transformalize.Configuration;
using Transformalize.Context;
using Transformalize.Impl;
using Transformalize.Providers.Console;
using Transformalize.Providers.Internal;
using Transformalize.Transform.Fluid;

namespace Test.Unit.Core {
   [TestClass]
   public class Basic {

      [TestMethod]
      public void CombineName() {

         var cfg = @"<cfg name='process' read-only='true'>
   <entities>
      <add name='entity'>
         <rows>
            <add FirstName='Dale' LastName='Newman' />
         </rows>
         <fields>
            <add name='FirstName' />
            <add name='LastName' />
         </fields>
         <calculated-fields>
            <add name='FullName'>
               <transforms>
                  <add method='razor' template='Hello {{ FirstName }} {{ LastName }}' >
                     <parameters>
                        <add field='FirstName' />
                        <add field='LastName' />
                     </parameters>
                  </add>
               </transforms>
            </add>
         </calculated-fields>
      </add>
   </entities>
</cfg>";
         var process = new Process(cfg);

         Assert.AreEqual(0, process.Errors().Length);

         // manually build out test without autofac to make it more of a unit test
         var logger = new ConsoleLogger();
         var entity = process.Entities.First();
         var field = entity.CalculatedFields.First();
         var operation = field.Transforms.First();
         var context = new PipelineContext(logger, process, entity, field, operation);
         var transform = new FluidTransform(context);
         var input = new InputContext(context);
         var reader = new InternalReader(input, new RowFactory(input.RowCapacity, entity.IsMaster, false));

         var rows = transform.Operate(reader.Read()).ToArray();

         Assert.AreEqual("Hello Dale Newman", rows[0][field]);

      }
   }
}
