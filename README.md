# WPR - master branch
 
WPR is a WP7-8 XNA app runner. This is only my fork of [WPR](https://github.com/8212369/WPR), not the original. 

This is  my little "Avalonia Android/Desktop framework" research + micro-development that I made in 2023 year.

*CAUTION*: after 2-3 years, the scr code is obsolete in *master* branch (great problems with VS 2022Preview compatibility! ).

More modern src code is in another branches:

- Avalonia (W+A, but mulfuntion)
- avalonia-win  (Windows only, ok)
- avalonia-and  (Android only, damaged)
- dev, xf, uwp and uno (Experimental / draft / internal dev use only)  

## "User" Features
- Installing WP7-8 **decrypted** XNA XAPs locally on your machine. Most encripted XAP files are unusable :(
- Earning achievements locally for Xbox Live games, with a pop-up appear everytime achievement is unlocked. :)

## "Dev" Features
- Extended error logging (sorry, there an no any "Dev UI" switch-offs at now!)
- Some code stability improved 
- System.Security.Cryptography "emulation" added (I noticed it needed for Countre Jour game...)
- Microsoft.Xna.Framework.GamerServicesExtensions project started (my experimentation without any documentation))

## Screenshot(s)
![](Images/intro.png)
![](Images/feedmeoil.png)
![](Images/kinectimals.png)
![](Images/contrejour.png)
![](Images/jetcarstunts.png)
![](Images/monkey.png)
![](Images/pac-man.png)
![](Images/outro.png)

+ "My "Atoms gameplay" (Youtube): https://www.youtube.com/watch?v=oFZza0Iw9K8


## My little RnD / experiments
- More installed mini-games can start now... but not all... ehhhh! )
- Last WPR Desktop "dirty bugfixes" transferred (sync-ed) to WPR Android
- Zuma and Earthworm Jim game running ok at now... but some artifacts still present ;)
- Feed Me Oil game fixed a little (starts normally... but game process fails when I click the pipe!)  
- Silverlight games exploring (sample: CutTheRope)

    
## Building 
### Windows (Desktop) target  
- I used the newest VS 2022 Preview to build this src code
- Add https://pkgs.dev.azure.com/AvaloniaUI/AvaloniaUI/_packaging/avalonia-all/nuget/v3/index.json to Nuget-package list (it allows to auto-download packages Avalonia.xxx v11.0.999-cibuild0023504-beta on the solution rebuilding)
- Build these libraries and place them under the same folder as the executable
- Place FFMPEG executable (you can download from their website or make a custom version with WMA->OGG conversion supported)
- Beside submodules included in this repostiory, this application depends on these native DLLs:
    * FNA3D
    * FAudio
    * libtheorafile 

### Android target 
- I also used the newest VS 2022 Preview to build this mind-blowing code. Plus I installed "Avalonia" extension from VS Marketplace (maybe, it could help to "auto-bind/auto-recompile java deals".. who knows?)))
- I "lost" all my code fixes because of I repeat to fork the original WPR (with ALL java bindings). I use this command (at cmd line):
```
git clone --recursive https://github.com/mediaexplorer74/WPR
```
- I specially deleted .gitmodules file for reducing the problems when I'll be upload the result onto GitHub repo.
- Before building I check that .NET 6 installed (recovered by VS) in my system.


## TODO
- Stabilize app running
- Add XBOX Live "emulation" to XNA
- Fix Butterfly patching
- Fix FeelMeOil & Contre Jour (and other similar games) running because of only "start screen" appearing at now 
- ? (idk what is in your brave mind))  
    
## This runner existence :: words from the [author/owner/main developer](https://github.com/8212369/) 
" It's for fun. If you are nostaglia mostly about achievements earning like me, you can try it out. There are some old games that is not released on Android or iOS, or some games that seems superior than Android or iOS version (I prefer Skulls of the Shogun on WP actually).
 However, resolution scaling is not yet implemented (game renders either in 480x800 or so...), but it's fun!" :: Lin Yan (8212369)


## Credits
- Tyler Jaacks (https://github.com/TylerJaacks) - for cool attempt to complete "net5/6 -> net8" upgrade 
- Hector47 (https://github.com/Hector47) for try to add some online services and more 
- All the people who supports this/that veery strange [W]indows[P]hone[R]unner story (see/look at [Original WPR issues](https://github.com/8212369/WPR/issues)  :)


## :: ::
AS IS. No support. Developers / Geeks only. "DIY mode"


## ::
[m][e] 2023 -> 2025

