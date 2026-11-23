  Imports Library
'         ^^^^^^^ test reference scip-dotnet nuget Library 1.0.0.0 Library/

  Public Module CalculatorChecks
'               ^^^^^^^^^^^^^^^^ test definition scip-dotnet nuget . . ``/CalculatorChecks#
'                                documentation ```vb\nModule CalculatorChecks\n```
      Public Sub AddsTwoNumbers()
'                ^^^^^^^^^^^^^^ test definition scip-dotnet nuget . . ``/CalculatorChecks#AddsTwoNumbers().
'                               documentation ```vb\nPublic Sub CalculatorChecks.AddsTwoNumbers()\n```
          Dim calculator As New Calculator()
'             ^^^^^^^^^^ test definition local 0
'                        documentation ```vb\ncalculator As Class Calculator\n```
'                               ^^^^^^^^^^ test reference scip-dotnet nuget Library 1.0.0.0 Library/Calculator#
          If calculator.Add(1, 2) <> 3 Then
'            ^^^^^^^^^^ test reference local 0
'                       ^^^ test reference scip-dotnet nuget Library 1.0.0.0 Library/Calculator#Add().
              Throw New System.Exception("Expected 3")
'                       ^^^^^^ test reference scip-dotnet nuget . . System/
'                              ^^^^^^^^^ test reference scip-dotnet nuget System.Runtime 8.0.0.0 System/Exception#
          End If
      End Sub
  End Module
