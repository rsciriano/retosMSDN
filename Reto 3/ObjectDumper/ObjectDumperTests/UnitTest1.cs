using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectDumper;

namespace ObjectDumperTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Only_Properties_With_Getter_Are_Dump()
        {
            var dumper = new ObjectDumper<Test1Class>();
            var desc = dumper.Dump(new Test1Class());
            Assert.AreEqual(1, desc.Count());
        }

        [TestMethod]
        public void No_Crash_With_Null_Properties()
        {
            var obj = new Test2Class.Test2Inner();
            obj.Name = null;
            obj.Value = 25;

            var dumper = new ObjectDumper<Test2Class.Test2Inner>();
            var desc1 = dumper.Dump(obj).Select(kvp => kvp.Key);
            var desc2 = dumper.Dump(obj).Select(kvp => kvp.Value);

            CollectionAssert.AreEqual(desc1.ToList(), new List<string>() { "Name", "Value"});
            CollectionAssert.AreEqual(desc2.ToList(), new List<string>() { null, "25"});
        }

        [TestMethod]
        public void Dump_Is_Sorted_By_Property_Name()
        {
            var dumper = new ObjectDumper<Test3Class>();
            var desc = dumper.Dump(new Test3Class()).Select(kvp => kvp.Key);

            CollectionAssert.AreEqual(desc.ToList(), new List<string>() { "AProperty", "BProperty", "ZProperty" });
        }

        [TestMethod]
        public void Default_Template_Is_To_String()
        {

            var dumper = new ObjectDumper<Test2Class>();
            var desc = dumper.Dump(new Test2Class());

            var firstProperty = desc.First();
            Assert.AreEqual("InnerClass", firstProperty.Key);
            Assert.AreEqual(new Test2Class.Test2Inner().ToString(), firstProperty.Value);
        }

        [TestMethod]
        public void Template_For_Simple_Type_Is_Applied()
        {
            const string IS_42 = "Answer to everything";
            const string IS_NOT_42 = "not meaningful";

            var dumper = new ObjectDumper<Test2Class.Test2Inner>();
            
            // Probar que si se añaden dos plantillas para el mismo campo , se usa la última
            dumper.AddTemplateFor(o => o.Value, v => "Duplicated key");

            dumper.AddTemplateFor(o => o.Value, v => v == 42 ? IS_42 : IS_NOT_42);

            var data = new Test2Class.Test2Inner()
            {
                Name = "Some name",
                Value = 42
            };

            var desc = dumper.Dump(data);

            //Con esto no se comprueba si relamente funciona porque KeyValuePair<string, string> nunca va a ser null
            Assert.IsNotNull(desc.SingleOrDefault(kvp => kvp.Key == "Value" && kvp.Value == IS_NOT_42));

            Assert.AreNotEqual(
                default(KeyValuePair<string, string>),
                desc.SingleOrDefault(kvp => kvp.Key == "Value" && kvp.Value == IS_42));
        }

        [TestMethod]
        public void Template_For_Complex_Type_Is_Applied()
        {
            var ufo = new Ufo()
            {
                Name = "Conqueror III",
                Speed = 10,
                Origin = new Planet()
                {
                    Name = "Alpha Centauri 3",
                    DaysPerYear = 452
                }
            };
            var dumper = new ObjectDumper<Ufo>();
            dumper.AddTemplateFor(u => u.Origin, o => string.Format("Planet: {0}", o.Name));
            var desc = dumper.Dump(ufo);

            //Con esto no se comprueba si relamente funciona porque KeyValuePair<string, string> nunca va a ser null
            Assert.IsNotNull(desc.SingleOrDefault(kvp =>
                kvp.Key == "Origin" && kvp.Value == string.Format("Planet: {0}", ufo.Origin.Name)));


            Assert.AreNotEqual(
                default(KeyValuePair<string, string>), 
                desc.SingleOrDefault(kvp => kvp.Key == "Origin" && kvp.Value == string.Format("Planet: {0}", ufo.Origin.Name)));
        }

        [TestMethod]
        public void Not_Listed_Property_Is_Not_Invoked()
        {
            var dumper = new ObjectDumper<CrashedUfo>();
            var crashed = new CrashedUfo()
            {
                Name = "Conqueror III",
                Speed = 10,
                Origin = new Planet()
                {
                    Name = "Alpha Centauri 3",
                    DaysPerYear = 452
                }
            };

            var desc = dumper.Dump(crashed);
            var twoPropertiesList = desc.Take(2).ToList();
            // No exception at this point because ZLastProperty is *never* invoked
            Assert.AreEqual(2, twoPropertiesList.Count);
        }
    }
}
