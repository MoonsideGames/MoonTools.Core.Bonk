# MoonTools.Core.Bonk

[![NuGet Badge](https://buildstats.info/nuget/MoonTools.Core.Bonk)](https://www.nuget.org/packages/NUnit/)
[![CircleCI](https://circleci.com/gh/MoonsideGames/MoonTools.Core.Bonk.svg?style=svg)](https://circleci.com/gh/MoonsideGames/MoonTools.Core.Bonk)

Bonk is a fast and modular collision detection system for MonoGame that is part of the MoonTools suite.

Bonk is designed to help you figure out if two shapes are overlapping and by how much.

Bonk is not a physics simulator and it will not help you execute collision responses.

Bonk is designed for performance and memory efficiency. Defining most shapes and performing collision tests require no heap allocations and thus put no pressure on the garbage collector. If you reuse spatial hashes and avoid using arbitrary polygons, Bonk will never cause garbage collection.

Bonk is licensed under the LGPL-3 license. In summary: feel free to include it in your closed-source game and modify it internally at will, but if you make changes that you intend to redistribute, you must freely publish your changes.

## Documentation
https://moontools-docs.github.io/bonk/
