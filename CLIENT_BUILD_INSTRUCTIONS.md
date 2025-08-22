# VdO Project - Client Build Instructions for Windows

## Quick Fix Instructions

Tell your client to follow these steps:

### 1. Unblock Downloaded Files (REQUIRED)
Since the files were downloaded from the internet, Windows blocks them. Fix this first:

**Option A - PowerShell (Recommended)**:
1. Open PowerShell as Administrator
2. Navigate to the extracted project folder
3. Run:
```powershell
Get-ChildItem -Path . -Recurse | Unblock-File
```

**Option B - Manual**:
1. Right-click the extracted folder
2. Select Properties
3. Check "Unblock" at the bottom
4. Click Apply

### 2. Run Automated Fix Script
We've included an automated script. In PowerShell (as Administrator):
```powershell
.\fix-build-errors.ps1 -RemoveCodeSigning -CreateStubLibraries
```

This script will:
- Unblock all files
- Set up NuGet package restore
- Remove code signing requirements
- Create stub files for missing MP libraries

### 3. Open in Visual Studio
1. Open `vdo_base.sln` in Visual Studio
2. When prompted about NuGet packages, click "Restore"
3. If not prompted, go to Tools → NuGet Package Manager → Package Manager Console and run:
   ```powershell
   Update-Package -reinstall
   ```

### 4. Handle Missing MP Libraries
The MP libraries (MPUtils, MPLogHelper, etc.) appear to be proprietary. You have three options:

**Option A**: If you have the original MP library DLLs:
1. Create a "lib" folder in the solution root
2. Copy all MP*.dll files there
3. Add references to each project

**Option B**: Use the stub implementations created by the script:
1. In Visual Studio, right-click the solution
2. Add → New Project → Class Library
3. Name it "MPLibraryStubs"
4. Add the generated stub files from the MPLibraryStubs folder
5. Add project references to this library

**Option C**: Contact the original developers for the MP library package

### 5. Build the Solution
1. Build → Clean Solution
2. Build → Rebuild Solution

## If Build Still Fails

Check these items:
- [ ] .NET Framework 4.5.2 is installed
- [ ] All files are unblocked
- [ ] NuGet packages are restored
- [ ] MP library references are resolved

## Complete Command Sequence

For your convenience, here's the complete sequence to copy/paste:

```powershell
# 1. Unblock files (run as Administrator)
Get-ChildItem -Path . -Recurse | Unblock-File

# 2. Run fix script
.\fix-build-errors.ps1 -RemoveCodeSigning -CreateStubLibraries

# 3. Restore packages (in Package Manager Console in VS)
Update-Package -reinstall

# 4. Build
msbuild vdo_base.sln /p:Configuration=Release
```

## Expected Outcome

After following these steps:
- All security warnings should be gone
- NuGet packages should be restored
- Code signing errors should be resolved
- Only MP library references might need manual resolution
- The project should build successfully

If the client still has issues after these steps, they should provide:
1. The specific error messages remaining
2. Whether they have access to the original MP libraries
3. The Visual Studio version they're using