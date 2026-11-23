  namespace Library
//          ^^^^^^^ reference scip-dotnet nuget . . Library/
  {
      // Library.Tests also compiles this file. It is still library code, not test code.
      internal static class Guard
//                          ^^^^^ definition scip-dotnet nuget . . Library/Guard#
//                                documentation ```cs\nclass Guard\n```
      {
          public static int NotNegative(int value) =>
//                          ^^^^^^^^^^^ definition scip-dotnet nuget . . Library/Guard#NotNegative().
//                                      documentation ```cs\npublic static int Guard.NotNegative(int value)\n```
//                                          ^^^^^ definition scip-dotnet nuget . . Library/Guard#NotNegative().(value)
//                                                documentation ```cs\nint value\n```
              value < 0 ? throw new System.ArgumentOutOfRangeException(nameof(value)) : value;
//            ^^^^^ reference scip-dotnet nuget . . Library/Guard#NotNegative().(value)
//                                  ^^^^^^ reference scip-dotnet nuget . . System/
//                                         ^^^^^^^^^^^^^^^^^^^^^^^^^^^ reference scip-dotnet nuget System.Runtime 8.0.0.0 System/ArgumentOutOfRangeException#
//                                                                            ^^^^^ reference scip-dotnet nuget . . Library/Guard#NotNegative().(value)
//                                                                                      ^^^^^ reference scip-dotnet nuget . . Library/Guard#NotNegative().(value)
      }
  }
