using Respawn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.UnitTests
{
    public class TestBase : IDisposable
    {
        protected TestFixture _fixture;

        private readonly Checkpoint _checkpoint;

        public TestBase(TestFixture fixture)
        {
            _fixture = fixture;
            _checkpoint = new Checkpoint();

            _checkpoint.SchemasToInclude = new string[] { };
            _checkpoint.TablesToInclude = new string[] { };
        }

        public void Dispose()
        {
            _checkpoint.Reset(_fixture.configuration.ConnectionString);

            // ... clean up test data from the database ...
            GC.SuppressFinalize(this);
        }
    }
}
