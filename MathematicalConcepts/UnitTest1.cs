using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace MathematicalConcepts
{
    [TestClass]
    public class LazyTests
    {
        //
        // Compute the topological order for this graph:
        //
        //    A
        //   ^ ^
        //  /   \
        // B     |
        //  ^   /
        //   \ /
        //    C
        //
        [TestMethod]
        public void LazyComputesTopologicalOrder()
        {
            var output = new StringBuilder();
            Lazy<int> a = null, b = null, c = null;
            c = new Lazy<int>(() => Evaluate(output, "C", () => a.Value + b.Value));
            b = new Lazy<int>(() => Evaluate(output, "B", () => a.Value + 3));
            a = new Lazy<int>(() => Evaluate(output, "A", () => 2));

            c.Value.Should().Be(7);
            output.ToString().Should().Be("<C><A></A><B></B></C>");
        }

        [TestMethod]
        public void LazyComputesTopologicalOrderDifferently()
        {
            var output = new StringBuilder();
            Lazy<int> a = null, b = null, c = null;
            c = new Lazy<int>(() => Evaluate(output, "C", () => b.Value + a.Value));
            b = new Lazy<int>(() => Evaluate(output, "B", () => a.Value + 3));
            a = new Lazy<int>(() => Evaluate(output, "A", () => 2));

            c.Value.Should().Be(7);
            output.ToString().Should().Be("<C><B><A></A></B></C>");
        }

        //
        // Find that this graph contains a cycle:
        //
        //    A
        //   ^ |
        //   | v
        //    B
        //
        [TestMethod]
        public void LazyDetectsCycles()
        {
            var output = new StringBuilder();
            Lazy<int> a = null, b = null;
            b = new Lazy<int>(() => Evaluate(output, "B", () => a.Value + 1));
            a = new Lazy<int>(() => Evaluate(output, "A", () => b.Value - 1));

            b.Invoking(l => { int v = l.Value; }).Should().Throw<InvalidOperationException>();
            output.ToString().Should().Be("<B><A>");
        }

        private static int Evaluate(StringBuilder output, string name, Func<int> function)
        {
            output.Append($"<{name}>");
            int result = function();
            output.Append($"</{name}>");
            return result;
        }
    }
}
