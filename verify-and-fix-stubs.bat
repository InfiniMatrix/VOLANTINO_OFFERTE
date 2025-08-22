@echo off

echo Verifying all stub files are included in project...



echo.

echo Checking which files are in MPLibraryStubs folder:

dir C:\VdO_Progetto\MPLibraryStubs\*.cs /b



echo.

echo ========================================

echo Making sure all files are included in the project.

echo.

echo In Visual Studio, you need to:

echo 1. Right-click on MPLibraryStubs project

echo 2. Select "Aggiungi" (Add) -^> "Elemento esistente" (Existing Item)

echo 3. Select ALL these .cs files if any are missing:

echo    - AdditionalClasses.cs

echo    - FileLog.cs

echo    - ImageFormatClass.cs

echo    - MPCommonRes.cs

echo    - MPExceptionHelper.cs

echo    - MPExtensionMethods.cs

echo    - MPFingerPrint.cs

echo    - MPLogHelper.cs

echo    - MPUtils.cs

echo    - OverwriteSetting.cs

echo    - ProductPrivilege.cs

echo 4. Click "Aggiungi" (Add)

echo ========================================

echo.

echo After adding all files, rebuild MPLibraryStubs.

echo.

echo If some files are missing from the folder, run the next part...

pause



echo.

echo Creating any missing stub files...



if not exist "C:\VdO_Progetto\MPLibraryStubs\AdditionalClasses.cs" (

    echo Creating AdditionalClasses.cs...

    (

echo using System;

echo using System.IO;

echo using System.Collections.Generic;

echo using System.Collections;

echo.

echo public static class DirectoryInfoExtensions

echo {

echo     public static void Copy^(this DirectoryInfo source, string destDirName, bool recursive^) { }

echo     public static void Copy^(this DirectoryInfo source, string destDirName, bool recursive, OverwriteSetting overwrite, OverwriteTimeStampField timeField, CopySelection selection^) { }

echo     public static string GetExceptionMessage^(Exception ex^) { return ex?.Message ?? ""; }

echo }

echo.

echo public static class HttpHelper

echo {

echo     public static string Get^(string url^) { return ""; }

echo }

echo.

echo public class HttpRedirectNotAllowedException : Exception 

echo {

echo     public HttpRedirectNotAllowedException^(^) : base^(^) { }

echo     public HttpRedirectNotAllowedException^(string message^) : base^(message^) { }

echo }

echo.

echo public static class UACHelper

echo {

echo     public static bool IsRunAsAdmin^(^) { return false; }

echo     public static bool IsUACEnabled^(^) { return true; }

echo     public static void RestartAsAdmin^(^) { }

echo     public static bool CanWriteToDirectory^(string path^) { return true; }

echo }

echo.

echo public class PluginActivatorFactory 

echo {

echo     public static IPluginActivatorFactory Create^(^) { return new PluginActivatorFactoryImpl^(^); }

echo }

echo.

echo public interface IPluginActivatorFactory : IEnumerable^<IPluginActivator^> 

echo {

echo }

echo.

echo public class PluginActivatorFactoryImpl : IPluginActivatorFactory

echo {

echo     public IEnumerator^<IPluginActivator^> GetEnumerator^(^) { yield break; }

echo     IEnumerator IEnumerable.GetEnumerator^(^) { return GetEnumerator^(^); }

echo }

echo.

echo public interface IPluginActivator 

echo { 

echo     Type[] GetPluginTypes^(^);

echo     object CreatePlugin^(Type type^);

echo }

echo.

echo public class DummyPluginActivator : IPluginActivator

echo {

echo     public Type[] GetPluginTypes^(^) { return new Type[0]; }

echo     public object CreatePlugin^(Type type^) { return null; }

echo }

echo.

echo public enum OverwriteSetting { Never, Always, NewerOnly, Ask }

echo public enum OverwriteTimeStampField { None, Modified, Created }

echo public enum CopySelection { All, FilesOnly, DirectoriesOnly }

echo.

echo public class VdO2013Main { }

echo public class Services { }

echo public class Suite { }

echo.

echo public class VDataStorage 

echo {

echo     public VDataStorage^(VDataStorageKind kind^) { }

echo }

echo.

echo public enum VDataStorageKind { Local, Network }

echo.

echo public class ExceptionHelperUI 

echo { 

echo     public static void Initialize^(^) { }

echo }

    ) > C:\VdO_Progetto\MPLibraryStubs\AdditionalClasses.cs

    echo Created AdditionalClasses.cs

)



echo.

echo Done! Now:

echo 1. Make sure all .cs files are included in the MPLibraryStubs project

echo 2. Rebuild MPLibraryStubs

echo 3. Rebuild VdO2013Core

pause