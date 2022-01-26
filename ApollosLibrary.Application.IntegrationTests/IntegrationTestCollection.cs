using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests
{
    [CollectionDefinition("IntegrationTestCollection")]
    public class IntegrationTestCollection : ICollectionFixture<TestFixture>
    {
    }
}
