using Moq;
using Xunit;
using Qooba.Framework.Db.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Db.Tests
{
    public class DbTests
    {
        private readonly IDb db;
        
        private readonly Mock<IDbConfig> configMock;

        public DbTests()
        {
            this.configMock = new Mock<IDbConfig>();
            this.configMock.Setup(x => x.ConnectionString).Returns("flight.json");
            this.db = new Db(this.configMock.Object);
        }

        [Fact]
        public void ExtractTest()
        {
            
        }
    }
}
