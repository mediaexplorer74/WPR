# WPR 0.0.12-alpha :: avalonia branch
![](Images/logo.png)

WPR is a WP7-8 XNA app runner. This is only my fork of [WPR](https://github.com/8212369/WPR), not the original. 

NOTE: *avalonia* branch used as +- "master" one. It constists of Windows target (tested) & Android (not tested yet). If you interested in development, look at (see) *dev* branch that consists also experimental things (i.e., iOS & WebBrowser target stubs, Tests, etc.).  

## Screenshots
![](Images/sshot01.png)
![](Images/sshot02.png)

## Status
- I started experimenting with .NET 8 & Avalonia 11. I started to repair Android part of solution (WPR.UI.Android). So, *avalonia* branch consists of 2 targets: Windows & Android at now :)
- With help of Trial mode of WindSurf (and ChatGPT 4 AI) I repaired Android-related parts of WPR code... (but still work-in-progress!).
- Some experimental "UI improvements applied ("Two small Run and Uninstall" icons added to main/larg icon in app/game list). Run & App also added to popup/context menu.
- All AI-generated things not tested yet (no Android device, and Android emulator errors because of my veeeey poor hardware; also, I never haved any iOS device ... so, help needed)
- For Android target, I changed Min. Supported Android Api version from 21 to 26 in project (.csproj) files. 


## Tech. details
- Newest VS 2022 Preview used to "assemble" (build) this "dev branch"
- I think that WPR "dev edition" incompatible with Win10 because of .NET 8... So, fresh Windows 11 OS needed to run WPR (however, some reduced Windows 11 Tiny is goode choice even for 15-year-old retro-notebooks... Sony Vaio, etc.)))


## ToDo
- Test Android target
- Actualize Wiki section
- Transtale Readme to RU and CN
- Fix resolution scaling...
- Port this "app creature" into Xamarin Forms or Uno "multi-platform engine" :)

## Credits
- Tyler Jaacks (https://github.com/TylerJaacks) - for net5/6 -> net8 upgrade !
- Hector47 (https://github.com/Hector47) for try to add some online services and more :)

## Another cool forks I noticed over 3 years 
-  https://github.com/TylerJaacks/WPR (branches *net8_upgrade* & *dotnet_upgrade* are very interesting & useful!)
-  https://github.com/Hector47/WPR (master branch: some GameServices ideas)

## :: ::
AS IS. No support. Developers / Geeks only. "DIY mode"

## ::
[m][e] 2025

![](Images/footer.png)
