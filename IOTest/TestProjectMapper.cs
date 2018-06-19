using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.IO
{
    [TestClass]
    public class TestProjectMapper
    {
        [TestMethod]
        [DeploymentItem(@"TestData\")]
        public void CSVとしての標準仕様とマッピング設定が機能しているべき()
        {
            var projects = RpvCsvReader.Instance.GetProjects("Projects.csv");
            projects.Count().Is(6);
        }
    }
}
