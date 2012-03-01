using System;

namespace Coypu.Queries
{
    internal class HasContentQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string text;
        public override object ExpectedResult { get { return true; } }

        internal HasContentQuery(Driver driver, DriverScope scope, string text) : base(scope)
        {
            this.driver = driver;
            this.text = text;
        }

        public override void Run()
        {
            Result = driver.HasContent(text, DriverScope);
        }
    }
}