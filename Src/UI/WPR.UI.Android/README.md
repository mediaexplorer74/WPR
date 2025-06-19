# WPR.UI.Android

This is the Android target for the WPR app using Avalonia. Compatible with Android 8.0 (API 26) and above.

## Build Instructions
- Requires .NET 8.0 SDK and Android workload installed.
- Open the solution in Visual Studio 2022+ or JetBrains Rider.
- Select `WPR.UI.Android` as the startup project.
- Build and deploy to an Android device or emulator.

## Notes
- This project references the cross-platform UI logic in `WPR.UI`.
- Android-specific code should be placed here if needed.
- Ensure all runtime permissions are managed according to Android requirements.
