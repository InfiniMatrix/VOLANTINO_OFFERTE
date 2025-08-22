@echo off

setlocal



set PROJECT_PATH=%~1

if "%PROJECT_PATH%"=="" set PROJECT_PATH=C:\VdO_Progetto



set FILE=%PROJECT_PATH%\VdO2013Core\Global.cs



if not exist "%FILE%" (

    echo Error: File not found: %FILE%

    pause

    exit /b 1

)



echo Creating backup...

copy "%FILE%" "%FILE%.bak" /Y



echo Applying fixes...



powershell -Command "$c = Get-Content '%FILE%' -Raw; $c = $c -replace 'if \(FileLog\.Initialized\)\s+Logger\.WriteError\(args\.Exception\);', 'if (FileLog.Initialized) { Logger.WriteError(args.Exception); }'; $c = $c -replace 'ExceptionHelper\.GetMainForm \+= new ExceptionHelper\.GetMainFormDelegate\(\(_\) =^>', 'ExceptionHelper.GetMainForm += new ExceptionHelper.GetMainFormDelegate((args) =^>'; $c = $c -replace '(\s+)ExceptionHelperUI\.Initialize\(\);', '$1MPExceptionHelper.UI.ExceptionHelperUI.Initialize();'; $c = $c -replace 'FileLog\.Default\.WriteError\(DirectoryInfoExtensions\.LastError\);', 'FileLog.Default.WriteError(MPExtensionMethods.DirectoryInfoExtensions.LastError);'; $c = $c -replace '(\s+)Services\.ThumbnailServiceManager\.Initialize\(\);', '$1// Services.ThumbnailServiceManager.Initialize();'; $c = $c -replace '= PluginActivatorFactory\.CreateFactory', '= MPUtils.PluginFactory.PluginActivatorFactory.CreateFactory'; $c = $c -replace 'foreach \(var activator in factory\)', 'foreach (var activator in factory)'; $c = $c -replace 'foreach \(var activator in _ActivatorFactory\)', 'foreach (var activator in _ActivatorFactory)'; $c = $c -replace 'activator\.GetPluginTypes\(\)', '((activator as MPUtils.PluginFactory.PluginActivator)?.GetPluginTypes() ?? System.Linq.Enumerable.Empty^<Type^>())'; $c = $c -replace 'return activator\.CreatePlugin\(pluginType, args\);', 'return (activator as MPUtils.PluginFactory.PluginActivator)?.CreatePlugin(pluginType, args);'; Set-Content '%FILE%' $c -Encoding UTF8 -NoNewline"



echo.

echo Done. Backup saved as: %FILE%.bak

pause