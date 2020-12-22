using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Transformalize.Configuration;
using Transformalize.Context;
using Transformalize.Contracts;
using Transformalize.Impl;
using Transformalize.Providers.Console;
using Transformalize.Providers.Internal;
using Transformalize.Transforms.Fluid;

namespace Test.Unit.Core {
   [TestClass]
   public class Basic {

      public const string cfg = @"<cfg name='process' read-only='true'>
   <entities>
      <add name='entity' >
         <rows >
            <add FirstName='Dale' LastName='Newman' />
         </rows>
         <fields>
            <add name='FirstName' />
            <add name='LastName' />
         </fields>
         <calculated-fields>
            <add name='FullName'>
               <transforms>
                  <add method='fluid' template='Hello {{ FirstName }} {{ LastName | downcase | append: &apos;.com&apos; }}' />
               </transforms>
            </add>
         </calculated-fields>
      </add>
   </entities>
</cfg>";

      [TestMethod]
      public void CombineNameDownCaseAndAppend() {

         var process = new Process(cfg);

         Assert.AreEqual(0, process.Errors().Length);

         var context = GetContext(process, "FullName", 0);
         var reader = GetReader(context);
         var rows = new FluidTransform(context).Operate(reader.Read()).ToArray();

         Assert.AreEqual("Hello Dale newman.com", rows[0][context.Field]);

      }

      private IContext GetContext(Process process, string fieldAlias, int transformIndex) {
         // normally a context is made by transformalize (container)
         var logger = new ConsoleLogger();
         var entity = process.Entities.First();
         var field = entity.CalculatedFields.First(f => f.Alias == fieldAlias);
         var operation = field.Transforms[transformIndex];
         return new PipelineContext(logger, process, entity, field, operation);
      }

      private IRead GetReader(IContext context) {
         // normally a reader is made by transformalize (container)
         var input = new InputContext(context);
         return new InternalReader(input, new RowFactory(input.RowCapacity, input.Entity.IsMaster, false));
      }

   }


}
