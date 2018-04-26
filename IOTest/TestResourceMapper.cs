using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.IO
{
    [TestClass]
    public class TestResourceMapper
    {
        [TestMethod]
        [DeploymentItem(@"TestData\")]
        public void CSVとしての標準仕様とマッピング設定が機能しているべき()
        {
            var resources = RpvCsvReader.Instance.GetResources("Resources.csv");
            resources.Count().Is(6);
        }
    }
}
