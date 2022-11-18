Set objShell = WScript.CreateObject("WScript.Shell")
WshShell.Run "build-atlas.bat"
WScript.Sleep 3000
objShell.sendKeys "{Enter}"