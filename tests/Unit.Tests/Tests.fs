module Unit.Tests

open TheVoid
open NUnit.Framework

[<Test>]
let ``Two instances of unit are equal`` () =
  Assert.AreEqual(Unit.Default, Unit.Default)

[<Test>]
let ``Calling ToString on Unit returns '()'`` () =
  Assert.AreEqual("()", Unit.Default.ToString ())