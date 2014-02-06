using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MockMetrics.Eating.MetricMeasure;

namespace MockMetrics.Tests
{
    public class SnapshotDump
    {
        private readonly char _sep = ';';

        public SnapshotDump()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DumpPath = Path.Combine(desktop, "mm_dump", DateTime.Now.ToString("yyyy-M-d HH-mm-ss"));
        }

        public string DumpPath { get; set; }

        public void Dump(IEnumerable<ISnapshot> snapshots, string testSet)
        {
            string csv = string.Format("Test{0} Librarians{0} Stubs{0} Mocks{0} Targets{0} Services{0} FakeProperties{0} FakeMethods{0} FakeCallbacks{0} FakeExceptions\n)", _sep);
            foreach (var snapshot in snapshots)
            {
                csv += WriteSnapshot(snapshot);
                csv += "\n";
            }
            csv = csv.Remove(csv.Count() - 1, 1);

            Directory.CreateDirectory(DumpPath);
            File.Create(Path.Combine(DumpPath, testSet + ".csv")).Dispose();
            File.WriteAllText(Path.Combine(DumpPath, testSet + ".csv"), csv);
        }

        public void DumpAssert(IEnumerable<ISnapshot> snapshots, string testSet)
        {
            var assertText = string.Empty;
            foreach (var snapshot in snapshots)
            {
                var metrics = snapshot.Metrics();
                var assert = new StringBuilder().Append("Assert.That(snapshots[0].Metrics().SequenceEqual(new[] {")
                                                .Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", metrics[0], metrics[1], metrics[2],
                                                        metrics[3], metrics[4], metrics[5], metrics[6], metrics[7], metrics[8]))
                                                .Append(" }), ")
                                                .Append(string.Format("\"{0}\");\r\n", snapshot.UnitTest.DeclaredName))
                                                .ToString();
                assertText += assert;
            }

            Console.WriteLine("//------------------{0}----------------------------------------------//", testSet);
            Console.WriteLine(assertText);
            Console.WriteLine("//-------------------------------------------------------------------//");
        }

        public void DumpWithDetail(IEnumerable<ISnapshot> snapshots, string testSet)
        {
            Dump(snapshots, testSet);
            foreach (var snapshot in snapshots)
            {
                DetailDump(snapshot);
            }
        }

        private string WriteSnapshot(ISnapshot snapshot)
        {
            return new StringBuilder().Append(snapshot.UnitTest.NameIdentifier.Name)
                                      .Append(_sep).Append(snapshot.Librarians.Count)
                                      .Append(_sep).Append(snapshot.Stubs.Count)
                                      .Append(_sep).Append(snapshot.Mocks.Count)
                                      .Append(_sep).Append(snapshot.Targets.Count)
                                      .Append(_sep).Append(snapshot.Services.Count)
                                      .Append(_sep).Append(snapshot.FakeProperties.Count)
                                      .Append(_sep).Append(snapshot.FakeMethods.Count)
                                      .Append(_sep).Append(snapshot.FakeCallbacks.Count)
                                      .Append(_sep).Append(snapshot.FakeExceptions.Count)
                                      .ToString();
        }

        private void DetailDump(ISnapshot snapshot)
        {
            var csv = string.Format("Node{0} Type\n)", _sep);
            foreach (var metricVariable in snapshot.Variables.OrderBy(t => t.GetVarType()))
            {
                csv += WriteVariable(metricVariable);
                csv += "\n";
            }

            foreach (var metricMockOption in snapshot.FakeOptions.OrderBy(t => t.FakeOption))
            {
                csv += WriteFakeOption(metricMockOption);
                csv += "\n";
            }
            csv = csv.Remove(csv.Count() - 1, 1);

            Directory.CreateDirectory(DumpPath);
            File.Create(Path.Combine(DumpPath, snapshot.UnitTest.NameIdentifier.Name + ".csv")).Dispose();
            File.WriteAllText(Path.Combine(DumpPath, snapshot.UnitTest.NameIdentifier.Name + ".csv"), csv);


        }

        private string WriteVariable(IMetricVariable metricVariable)
        {
            return string.Format("{0}{1} {2}\n", metricVariable.Node.GetText(), _sep, metricVariable.GetVarType());
        }

        private string WriteFakeOption(IMetricMockOption metricMockOption)
        {
            return string.Format("{0}{1} {2}\n", metricMockOption.Node.GetText(), _sep, metricMockOption.FakeOption);
        }
    }
}
