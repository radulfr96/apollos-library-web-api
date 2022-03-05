using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.IDP.IntegrationTests
{
    public class TestBase : IDisposable
    {
        private readonly TestFixture _fixture;

        public TestBase(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {

            _fixture.ResetCheckpoint();
        }
    }
}
