for /d /r . %%d in (bin,obj,debug) do @if exist "%%d" rd /s /q "%%d"
