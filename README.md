# WPR 0.0.12-alpha :: commit0 branch
![](Images/logo.png)

WPR is a WP7-8 XNA app runner. This is only my fork of [WPR](https://github.com/8212369/WPR), not the original. 

*CAUTION*: this *commit0* branch is for internal dev use. Main goal is research of "minimal MONOGAME NET8 multiptatform app sample" only. It's not for production. 

## Details (steps) of my experiments
- I found Fruit_Ninja_v1.1.0.0.xap and renamed it (in)to Fruit_Ninja_v1.1.0.0.zip
- I created c:\Test folder , then I unpack Fruit_Ninja_v1.1.0.0.zip to c:\Test
- I copied FNWP72.dll as FNWP72.dll.original
- I opened WPR at/in VS 2022, then I started debugging. I noticed that app patched FNWP72.dll and did some attempt 
to run "Fruit Ninja" game via XNA Monogame runtime... =)
- Game start failed, but I noticed that some dll call "redirected" to special "services" ("compatibility layers") such as WPR.WindowsCompability, Microsoft.Xna.Framework.GamerServices, Microsoft.Phone (look at solution structure -- this is special projects-re-translators)

## Status
- Experimenting with original [WPR](https://github.com/8212369/WPR) ' s commit #0 [Fruit Ninja working](https://github.com/8212369/WPR/commit/6219ebcab10a2638d93a1fc0bc15323112b7fd99).

- One Big Onsolved (sinse 2022) Dev question about "commit #0" : https://github.com/8212369/WPR/issues/6

## :: ::
AS IS. No support. Developers / Geeks only. "DIY mode"


## ::
[m][e] 2025

![](Images/footer.png)
