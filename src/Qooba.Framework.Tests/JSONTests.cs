using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Qooba.Framework
{
    public class JSONTests
    {
        [Fact]
        public void TestSerializerObject()
        {
            var num = 1;
            var ch = new List<char>();
            var b = new List<int>();
            var mc = new MyClass1
            {
                Content_type = "test",
                Datetime_value = DateTime.Now,
                Decimal_value = 1.2m,
                Double_value = 1.3,
                Enum_value = MyEnum.Two,
                Int_value = 4,
                Title = "ti",
                Stringlist_value = new List<string> { "s1", "s2", "s2", "s2", "s2", "s2", "s2", "s2", "s2", "s2", "s2", "s2" },
                Doublelist_value = new List<double> { 1.1, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2, 1.2 },
                Sublist_value = new List<MySubClass> {
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    },
                    new MySubClass
                    {
                        Subtitle = "s_titl1"
                    }
                },
                Sub_value = new MySubClass
                {
                    Subtitle = "s_titl1"
                },
                Sub_object = new MySubClass
                {
                    Subtitle = "s1"
                }
            };

            string s1 = null;
            string s2 = null;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < num; i++)
            {

                s1 = JSON.SerializeObject(mc);

            }

            sw.Stop();
            var t1 = sw.ElapsedMilliseconds;

            // sw.Restart();
            // for (int i = 0; i < num; i++)
            // {
            //     s2 = JsonConvert.SerializeObject(mc, new JsonSerializerSettings
            //     {
            //         ContractResolver = new CamelCasePropertyNamesContractResolver()
            //     });
            // }
            // sw.Stop();

            // var t2 = sw.ElapsedMilliseconds;

            Assert.NotNull(s1);
        }

        [Fact]
        public void TestDeserializeList()
        {
            var num = 1;
            var ch = new List<char>();
            var b = new List<int>();
            IList<MySubClass> mc = null;
            var json = "[{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"},{ \"subtitle\": \"subtitle_value2\"}]";
            //var json = "[{ \"subtitle\": \"subtitle_value1\"} , { \"subtitle\": \"subtitle_value1\"} ]";
            //var json = "[1,2,3,4,5,6,7,,]";
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < num; i++)
            {
                using (Stream s = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
                {
                    mc = (JSON.DeserializeObject(s, typeof(List<MySubClass>)) as List<MySubClass>).ToList();
                }
            }

            sw.Stop();
            var t1 = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < num; i++)
            {
                //mc = JsonConvert.DeserializeObject<List<MySubClass>>(json, new JsonSerializerSettings
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                //}).ToList();
            }

            sw.Stop();

            var t2 = sw.ElapsedMilliseconds;

            Assert.NotNull(mc); Assert.NotNull(mc);

        }


        [Fact]
        public void TestDeserializerObject()
        {
            var num = 1;
            var ch = new List<char>();
            var b = new List<int>();
            MyClass mc = null;
            var json = "{ \"doublelist_value\": [2.2,1.1],\"sublist_value\": [{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value1\"},{ \"subtitle\": \"subtitle_value2\"}], \"stringlist_value\": [\"test1\",\"test2\"], \"sub_value\": { \"subtitle\": \"subtitle_value\"}, \"enum_value\": \"Two\", \"int_value\": 1, \"double_value\": 1.1, \"decimal_value\": 1.1, \"datetime_value\": \"2018-02-02\", \"content_type\": \"text\", \"dupa1\": {\"t1\": \"ttt\"}, \"dupa2\": 1.1, \"title\": \"Red\", \"dupa\": \"test\", \"dupa3\": 1.2, \"sub_object\": {\"t1\": \"ttt\"} }";
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < num; i++)
            {
                using (Stream s = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
                {
                    mc = JSON.DeserializeObject(s, typeof(MyClass), true) as MyClass;
                }
            }

            sw.Stop();
            var t1 = sw.ElapsedMilliseconds;

            // sw.Restart();
            // var settings = new JsonSerializerSettings
            // {
            //     ContractResolver = new CamelCasePropertyNamesContractResolver()
            // };

            // for (int i = 0; i < num; i++)
            // {
            //     mc = JsonConvert.DeserializeObject<MyClass>(json, settings);
            // }
            // sw.Stop();

            var t2 = sw.ElapsedMilliseconds;

            Assert.NotNull(mc); Assert.NotNull(mc);
            // Assert.True(mc.content_type == "text");
            // Assert.True(mc.title == "Red");
        }

        [Fact]
        public void ParsePropertyNameAndValueTest()
        {
            var propertyName = string.Empty;
            var propertyValue = string.Empty;
            var json = "\"content_type\": \"text\",";
            using (Stream s = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                propertyName = JSON.ParsePropertyName(s);
                propertyValue = JSON.ParsePropertyValue(s);
            }

            Assert.True(propertyName == "content_type");
            Assert.True(propertyValue == "text");
        }

        public class MyClass1
        {
            public string Content_type { get; set; }

            public string Title { get; set; }

            public int? Int_value { get; set; }

            public double? Double_value { get; set; }

            public decimal? Decimal_value { get; set; }

            public DateTime Datetime_value { get; set; }

            public MyEnum Enum_value { get; set; }

            public IList<string> Stringlist_value { get; set; }

            public IList<double> Doublelist_value { get; set; }

            public IList<MySubClass> Sublist_value { get; set; }

            public MySubClass Sub_value { get; set; }

            public object Sub_object { get; set; }
        }

        public class MyClass
        {
            public string Content_type { get; set; }

            public string Title { get; set; }

            public int? Int_value { get; set; }

            public double? Double_value { get; set; }

            public decimal? Decimal_value { get; set; }

            public DateTime? Datetime_value { get; set; }

            public MyEnum Enum_value { get; set; }

            public MySubClass Sub_value { get; set; }

            public IList<string> Stringlist_value { get; set; }

            public IList<MySubClass> Sublist_value { get; set; }

            public IList<double> Doublelist_value { get; set; }

            public object Sub_object { get; set; }
        }

        public class MySubClass
        {
            public string Subtitle { get; set; }
        }

        public enum MyEnum
        {
            One,

            Two
        }
    }
}