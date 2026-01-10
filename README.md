# WPR 0.0.15-alpha :: avalonia branch
![](Images/logo.png)

WPR is a WP7-8 XNA app runner. This is only my fork of [WPR](https://github.com/8212369/WPR), not the original. 

NOTE: previous *avalonia* branch copied to "avalonia-win" one. In my new "avalonia" branch I planned to repair Android target. 

## Screenshots

![](Images/sshot01.png)

_Windows 11 Tiny_


## Status
- I started experimenting with .NET 8 & Avalonia 11.3.9. I started to repair Android part of solution (WPR.UI.Android) & fix Avalonia 11.3.9 for WPR.UI.Desktop to. So, *avalonia* branch consists of 2 targets: Windows & Android at now :)
- With help of Trial mode of WindSurf (and ChatGPT 4 AI) I partially repaired Android-related parts of WPR code... But thiis still work-in-progress: many game "patches" lost.
- Experimental "UI improvements applied ("Two small Run and Uninstall" icons added to main/larg icon in app/game list) lost. No "Run & App at popup/context menu".
- All AI-generated things not tested yet 
- For Android target, I changed Min. Supported Android Api version from 21 to 26 in project (.csproj) files. 


## Tech. details
- Newest VS 2022 Preview used to "assemble" (build) this "avaonia branch"
- I think that WPR "dev edition" incompatible with Win10 because of .NET 8... So, fresh Windows 11 OS needed to run WPR (however, some reduced Windows 11 Tiny is goode choice even for some very retro-notebooks)


## ToDo
- Fix bug "System.TypeInitializationException: The type initializer for 'DialogHost.DialogHost' threw an exception." (install progress indicator error)
- Repair lost game patches (use "avalonia-win" branch) 
- Test Desktop (Windows) target
- Test Android target
- Actualize Wiki section
- Transtale Readme to RU and CN
- Fix resolution scaling...
- Try to port this "app creature" (in)to modern "multi-platform engine" such as MAUI (far future)

## Credits
- Tyler Jaacks (https://github.com/TylerJaacks) - for net5/6 -> net8 upgrade !
- Hector47 (https://github.com/Hector47) for try to add some online services and more :)

## Another cool forks I noticed over 3 years 
-  https://github.com/TylerJaacks/WPR (branches *net8_upgrade* & *dotnet_upgrade* are very interesting & useful!)
-  https://github.com/Hector47/WPR (master branch: some GameServices ideas)

## :: ::
AS IS. No support. Developers / Geeks only. "DIY mode"

## ::
[m][e] 2026

![](Images/footer.png)
