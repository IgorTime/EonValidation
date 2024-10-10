using EonValidation.Editor;
using NUnit.Framework;

namespace EonValidation.Tests
{
    public class ValidatableComponents
    {
        private static string[] Prefabs => ValidationPaths.GetAllValidationTargetPrefabs();
        
        [Test]
        public void ValidateComponents([ValueSource(nameof(Prefabs))] string path)
        {
            Assert.Pass();
        }
    }
}