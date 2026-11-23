Imports Library

Public Module CalculatorChecks
    Public Sub AddsTwoNumbers()
        Dim calculator As New Calculator()
        If calculator.Add(1, 2) <> 3 Then
            Throw New System.Exception("Expected 3")
        End If
    End Sub
End Module
