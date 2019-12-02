# MoonTools.Core.Bonk

[![NuGet Badge](https://buildstats.info/nuget/MoonTools.Core.Bonk)](https://www.nuget.org/packages/MoonTools.Core.Bonk/)
[![CircleCI](https://circleci.com/gh/MoonsideGames/MoonTools.Core.Bonk.svg?style=svg)](https://circleci.com/gh/MoonsideGames/MoonTools.Core.Bonk)

Bonk is a fast and modular collision detection system for .NET that is part of the MoonTools suite. It can be used with any .NET-based game engine.

Bonk is designed to help you figure out if two shapes are overlapping and by how much.

Bonk is not a physics simulator and it will not help you execute collision responses.

Bonk is designed for performance and memory efficiency. Defining shapes and performing collision tests require no heap allocations and put no pressure on the garbage collector. If you reuse spatial hashes, Bonk will never cause garbage collection.

Bonk is licensed under the LGPL-3 license. In summary: feel free to include it in your closed-source game and modify it internally at will, but if you make changes that you intend to redistribute, you must freely publish your changes.

## Documentation
https://moontools-docs.github.io/bonk/
