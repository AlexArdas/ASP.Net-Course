using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;

namespace CST.Tests.Common
{
    [ExcludeFromCodeCoverage]
    public static class FixtureInitializer
    {

        public static Fixture InitializeFixture()
        {
            var fixture = new Fixture();

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());


            return fixture;
        }
    }
}
