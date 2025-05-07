# WPR - uwp branch
![Logo](Images/logo.png)
 
Template for future "WPR UWP". Almost empty project at now (but with some original WPR parts)

*CAUTION*: WPR is a WP7-8 XNA app runner. This is only my fork of [WPR](https://github.com/8212369/WPR), not the original. 



## Architecture
### [Microsoft.Phone](Src/Microsoft.Phone) Microsoft.Phone module
### [Microsoft.Xna.Framework.GamerServices](Src/Microsoft.Xna.Framework.GamerServices) Online Gamer Services model
### [WPR.MonoGameCompabilityPatch](Src/WPR.MonoGameCompabilityPatch) XNA / Monogame "Edit/Patch" module
### [WPR.Droid](Src/WPR.Droid) - Android module (minSdkVersion="26")
### [WPR](Src/WPR) - "Universal Windows" module (min. Win. build = 16299, for "Andromeda"-like devices...)

## Install&Dev fast intro
* Install newest Visual Studio 2022 Preview 
* Install Universal windows platform, and .NET
* Install Xamarin / MAUI, Web dev. workloads (for brave and future Xamarin Forms / MAUI experiments...)

  

## Changelog
### v1.0.*


## TODO
- Do test builds (conditional framework's builds according to platform specifics).
- Try to inject "WPR kernel" from original WPR (https://github.com/8212369/WPR) 's "zero commit"... 
- Realize another "multi-platform WPR-world"... =)
  
Provide a sample application.


## Credits
- Tyler Jaacks (https://github.com/TylerJaacks) - for net5/6 -> net8 upgrade !
- Hector47 (https://github.com/Hector47) for try to add some online services and more :)
- (To) All the people who supports this/that veery strange [W]indows[P]hone[R]unner story (see/look at [Original WPR issues](https://github.com/8212369/WPR/issues)  ) ) )

# Contribute!
There's still a TON of things missing from this proof-of-concept (MVP) and areas of improvement 
which I just haven't had the time to get to yet.
- UI Improvements (for GTK, for example, or for each one of supported mutli-platforms)))
- New features (toasts, etc..))
- Additional Language Packages
- Media Transferring Support: screenshots, etc. (for the brave)

## References
- https://github.com/8212369/WPR Original WPR project


## License
MIT License - see the [LICENSE](LICENSE) file for details.

## .
As is. No support. For geeks / devs only! DIY.

## ..
With best wishes,

  [m][e] 2025

