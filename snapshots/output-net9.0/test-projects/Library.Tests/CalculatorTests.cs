  namespace Library.Tests
//          ^^^^^^^ test reference scip-dotnet nuget . . Library/
//                  ^^^^^ test reference scip-dotnet nuget . . Tests/
  {
      // scip-dotnet marks test projects, not test attributes, so this fixture needs no test framework.
      public class CalculatorTests
//                 ^^^^^^^^^^^^^^^ test definition scip-dotnet nuget . . Tests/CalculatorTests#
//                                 documentation ```cs\nclass CalculatorTests\n```
      {
          public void AddsTwoNumbers()
//                    ^^^^^^^^^^^^^^ test definition scip-dotnet nuget . . Tests/CalculatorTests#AddsTwoNumbers().
//                                   documentation ```cs\npublic void CalculatorTests.AddsTwoNumbers()\n```
          {
              var calculator = new Calculator();
//                ^^^^^^^^^^ test definition local 0
//                           documentation ```cs\nCalculator? calculator\n```
//                                 ^^^^^^^^^^ test reference scip-dotnet nuget . . Library/Calculator#
              AssertEqual(TestData.Three, calculator.Add(1, 2));
//            ^^^^^^^^^^^ test reference scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().
//                        ^^^^^^^^ test reference scip-dotnet nuget . . Tests/TestData#
//                                 ^^^^^ test reference scip-dotnet nuget . . Tests/TestData#Three.
//                                        ^^^^^^^^^^ test reference local 0
//                                                   ^^^ test reference scip-dotnet nuget . . Library/Calculator#Add().
          }

          public void AcceptsZero()
//                    ^^^^^^^^^^^ test definition scip-dotnet nuget . . Tests/CalculatorTests#AcceptsZero().
//                                documentation ```cs\npublic void CalculatorTests.AcceptsZero()\n```
          {
              AssertEqual(0, Guard.NotNegative(0));
//            ^^^^^^^^^^^ test reference scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().
//                           ^^^^^ test reference scip-dotnet nuget . . Library/Guard#
//                                 ^^^^^^^^^^^ test reference scip-dotnet nuget . . Library/Guard#NotNegative().
          }

          private static void AssertEqual(int expected, int actual)
//                            ^^^^^^^^^^^ test definition scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().
//                                        documentation ```cs\nprivate static void CalculatorTests.AssertEqual(int expected, int actual)\n```
//                                            ^^^^^^^^ test definition scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().(expected)
//                                                     documentation ```cs\nint expected\n```
//                                                          ^^^^^^ test definition scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().(actual)
//                                                                 documentation ```cs\nint actual\n```
          {
              if (expected != actual)
//                ^^^^^^^^ test reference scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().(expected)
//                            ^^^^^^ test reference scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().(actual)
              {
                  throw new System.Exception($"Expected {expected}, got {actual}");
//                          ^^^^^^ test reference scip-dotnet nuget . . System/
//                                 ^^^^^^^^^ test reference scip-dotnet nuget System.Runtime 9.0.0.0 System/Exception#
//                                                       ^^^^^^^^ test reference scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().(expected)
//                                                                       ^^^^^^ test reference scip-dotnet nuget . . Tests/CalculatorTests#AssertEqual().(actual)
              }
          }
      }
  }
