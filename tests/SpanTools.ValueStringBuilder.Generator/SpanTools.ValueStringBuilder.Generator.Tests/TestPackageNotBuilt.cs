using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SpanTools.Generator.Tests
{
    public class TestPackageNotBuilt
    {
        public void NoGeneratedContent()
        {
            Assert.Fail("The generator package was not referenced during this build. Ensure the NuGet package was built and $(_PackageVersionPropsFilePath) exists.");
        }
    }
}
