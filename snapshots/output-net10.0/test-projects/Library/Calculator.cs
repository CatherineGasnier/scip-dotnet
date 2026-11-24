  namespace Library
//          ^^^^^^^ reference scip-dotnet nuget . . Library/
  {
      public class Calculator
//                 ^^^^^^^^^^ definition scip-dotnet nuget . . Library/Calculator#
//                            documentation ```cs\nclass Calculator\n```
      {
          public int Add(int a, int b) => Guard.NotNegative(a) + Guard.NotNegative(b);
//                   ^^^ definition scip-dotnet nuget . . Library/Calculator#Add().
//                       documentation ```cs\npublic int Calculator.Add(int a, int b)\n```
//                           ^ definition scip-dotnet nuget . . Library/Calculator#Add().(a)
//                             documentation ```cs\nint a\n```
//                                  ^ definition scip-dotnet nuget . . Library/Calculator#Add().(b)
//                                    documentation ```cs\nint b\n```
//                                        ^^^^^ reference scip-dotnet nuget . . Library/Guard#
//                                              ^^^^^^^^^^^ reference scip-dotnet nuget . . Library/Guard#NotNegative().
//                                                          ^ reference scip-dotnet nuget . . Library/Calculator#Add().(a)
//                                                               ^^^^^ reference scip-dotnet nuget . . Library/Guard#
//                                                                     ^^^^^^^^^^^ reference scip-dotnet nuget . . Library/Guard#NotNegative().
//                                                                                 ^ reference scip-dotnet nuget . . Library/Calculator#Add().(b)
      }
  }
