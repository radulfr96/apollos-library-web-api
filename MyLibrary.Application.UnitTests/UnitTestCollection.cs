using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.UnitTests
{
    [CollectionDefinition("UnitTestCollection")]
    public class UnitTestCollection : ICollectionFixture<TestFixture>
    {
    }
}
