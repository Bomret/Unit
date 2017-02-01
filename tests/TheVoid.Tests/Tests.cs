using NUnit.Framework;

namespace Unit.Tests {
  [TestFixture]
  class UnitTests {
    [Test]
    public void Two_instances_of_Unit_are_equal() =>
      Assert.AreEqual(TheVoid.Unit.Default, TheVoid.Unit.Default);

    [Test]
    public void Calling_ToString_on_Unit_returns_the_correct_representation() =>
      Assert.AreEqual("()", TheVoid.Unit.Default.ToString());
  }
}
