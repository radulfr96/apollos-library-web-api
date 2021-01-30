using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.XUnitTestProject
{
    public class TestBase : IDisposable
    {
        protected TestFixture _fixture;

        public TestBase(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            GC.SuppressFinalize(this);
        }
    }
}
