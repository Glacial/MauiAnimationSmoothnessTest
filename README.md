# MauiAnimationSmoothnessTest
A quick test app the judge the smoothness of MAUI's animations.
I knocked this together to highlight that animation in MAUI is often not as smooth as I would like it to be.

The app displays the standard MAUI "Hello World" template, only some of the text now outputs the average time in milliseconds between the last 100 animation updates. The lowest and highest times are also shown.
Some stuff is also animating for visual effect.

Things look pretty good on iOS most of the time and the app is able to maintain 60fps most of the time. Note that when you interact with MAUI, the animation rate can be affected and you may see stutters or some disturbingly large pauses.
The Windows version is pretty choppy all of the time.
