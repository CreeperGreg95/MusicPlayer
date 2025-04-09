Public Class TooltipsApp
    Private tooltip As ToolTip

    Public Sub New()
        Me.tooltip = New ToolTip()
    End Sub

    Public Sub SetTooltip(control As Control, text As String)
        tooltip.SetToolTip(control, text)
    End Sub
End Class
