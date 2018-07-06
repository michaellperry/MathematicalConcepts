using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace MathematicalConcepts
{
    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void LazyComputesTopologicalOrder()
        {
            StringBuilder output = new StringBuilder();
            Lazy<int> a = null, b = null, c = null;
            c = new Lazy<int>(() => Evaluate(output, "C", () => a.Value + b.Value));
            b = new Lazy<int>(() => Evaluate(output, "B", () => a.Value + 3));
            a = new Lazy<int>(() => Evaluate(output, "A", () => 2));

            c.Value.Should().Be(7);
            output.ToString().Should().Be("<C><A></A><B></B></C>");
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
