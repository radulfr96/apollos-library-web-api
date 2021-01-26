using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.XUnitTestProject
{
    [CollectionDefinition("UnitTestCollection")]
    public class UnitTestCollection : ICollectionFixture<TestFixture>
    {
    }
}
