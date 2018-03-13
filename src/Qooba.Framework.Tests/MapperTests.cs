using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Qooba.Framework
{
    public class MapperTests
    {
        [Fact]
        public void TestMap()
        {
            var input = new ITest
            {
                Name = "N1",
                LastName = "LN1",
                Age = 21,
                Amount = "22",
                DAmount = "2.321",
                Sex = "Female",
                NullI = "111",
                Date = "2018-01-11",
                NullDate = "2018-01-11",
                NullDouble = "2.1",
                NullDate1 = DateTime.Now,
                NullDouble1 = 1.2,
                NullInt1 = 1,
                NullDate2 = DateTime.Now,
                NullDouble2 = 1.2,
                NullInt2 = null,
                Nested = new INested
                {
                    Name = "IN",
                    LastName = "ILN"
                },
                Items = new List<INested> {
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"}
                },
                AItems = new[] {
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"},
                    new INested  {Name = "IN1", LastName = "IN2"}
                }
            };

            var o = new Mapper().Map<ITest, OTest>(input);
            Assert.NotNull(o);
        }



        public class ITest
        {
            public DateTime? NullDate1 { get; set; }

            public double? NullDouble1 { get; set; }

            public double? NullInt1 { get; set; }

            public DateTime? NullDate2 { get; set; }

            public double? NullDouble2 { get; set; }

            public double? NullInt2 { get; set; }

            public string Name { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            public string NullI { get; set; }

            public string NullDouble { get; set; }

            public string Amount { get; set; }

            public string Date { get; set; }

            public string NullDate { get; set; }

            public string DAmount { get; set; }

            public string Sex { get; set; }

            public INested Nested { get; set; }

            public IList<INested> Items { get; set; }

            public INested[] AItems { get; set; }
        }

        public class INested
        {
            public string Name { get; set; }

            public string LastName { get; set; }
        }

        public class OTest
        {

            public DateTime? NullDate2 { get; set; }

            public double? NullDouble2 { get; set; }

            public double? NullInt2 { get; set; }

            public string NullDate1 { get; set; }

            public string NullDouble1 { get; set; }

            public string NullInt1 { get; set; }


            public string Name { get; set; }

            public string LastName { get; set; }

            public DateTime Date { get; set; }

            public DateTime? NullDate { get; set; }

            public double? NullDouble { get; set; }

            public string Age { get; set; }

            public int? NullI { get; set; }

            public int Amount { get; set; }

            public double DAmount { get; set; }

            public Sex Sex { get; set; }

            public ONested Nested { get; set; }

            public IList<ONested> Items { get; set; }

            public ONested[] AItems { get; set; }
        }

        public class ONested
        {
            public string Name { get; set; }

            public string LastName { get; set; }
        }

        public enum Sex
        {
            Male,
            Female
        }

    }
}