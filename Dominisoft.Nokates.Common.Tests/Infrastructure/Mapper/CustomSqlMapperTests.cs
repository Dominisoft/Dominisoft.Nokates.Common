using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Mapper;
using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.Mapper
{
    [TestFixture]
    public class CustomSqlMapperTests
    {
        [Test]
        public void Should_map_by_property()
        {
            var sqlResult = new
            {
                a = 1,
                b = "test",
                c = "TestMap",
                ds = "{\"sp1\":2,\"sp2\":\"TestSP\"}",
                ids = "[\"4587BE83-B011-4710-B763-903D672F23DD\",\"C2DEAAE6-FD7E-4290-9EBD-9D8BE5314C80\",\"03F0EED6-8AAB-47FF-A24D-85152DEA0D9C\"]"

            };

            var mapperResult = CustomSqlMapper.Map<TestSqlObject>(sqlResult);

            Assert.AreEqual(sqlResult.a, mapperResult.a);
            Assert.AreEqual(sqlResult.b, mapperResult.b);
            Assert.AreEqual(sqlResult.c, mapperResult.ColumnC);
            Assert.AreEqual(2, mapperResult.SubObject.sp1);
            Assert.AreEqual("TestSP", mapperResult.SubObject.sp2);
            Assert.AreEqual(3, mapperResult.IdList.Count);
        }

        [Test]
        public void Should_reverse_map_by_property()
        {
            var mapperResult = new TestSqlObject
            {
                a = 3,
                b = "TEST",
                ColumnC = "asdf",
                SubObject = new TestSqlObject2
                {
                    sp1 = 44,
                    sp2 = "qwerty"

                },
                IdList = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                }
            };
            var sqlResult = CustomSqlMapper.ReverseMap(mapperResult);

            Assert.AreEqual(mapperResult.a, sqlResult.a);
            Assert.AreEqual(mapperResult.b, sqlResult.b);
            Assert.AreEqual(mapperResult.ColumnC, sqlResult.c);
            Assert.AreEqual(mapperResult.SubObject.Serialize(), sqlResult.ds);
            Assert.AreEqual(mapperResult.IdList.Serialize(), sqlResult.ids);
        }
    }

    [Table("Test")]
public class TestSqlObject:Entity
    {
        [ReadOnly(true)]
        public int a { get; set; }
        public string b { get; set; }
        [Column("c")]
        public string ColumnC { get; set; }
        [JsonColumn("ds")]
        public TestSqlObject2 SubObject { get; set; }
        [JsonColumn("ids")]
        public List<Guid> IdList { get; set; }
    }

    public class TestSqlObject2
    {
        public int sp1 { get; set; }
        public string sp2 { get; set; }
    }
}
