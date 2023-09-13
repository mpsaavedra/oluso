# Oluso SDK Documentations
<h3 style="color:#5283b5">Everything you need to know about Oluso SDK</h3>

The **Oluso** SDK and has the intention to put together as many packages and utilities as possible that solve many of the common problems i' ve been facing during this years of development. I hope that helps some other developers out there.

![Oluso SDK](./images/logo.png)

**What Oluso means?**: Well, the word is from the `Vodoo` language and the real translation is **guardian, keeper** or **celador** on spanish, i am cuban so `Vodoo` language words are part of our common slang, that's why not to use it to honor my heritage. Also because the SDK should work as a process guardian that provides the desire stability in code, and simplify the development process.

> This documentation it's been written on the go so it could change without previous notification.

## How the documentation is organized

Oluso has a lot to documentation. A high-level of how it's organized will help you to know here to look for certain things:

* [Introduction](./intro.md) is the first place you must dive into if you want to use Oluso SDK, check it to get a general idea about how is organized and how the packages interact between them and how to use this functionalities to work for you.
* [Tutorials](./tutorial/index.md) take you by the hand through a series of steps to create applications. Start here if you are new to Oluso SDK usage. Also look at ["First steps"](#first-steps)
* [Reference guides](./developers/reference-guides.md) contains technical references for APIs and other aspects of Oluso's design and implementations. Thry describe how it works and how to use it but assume that you have basic understanding of key concepts.
* [How-to guides](./how-to/index.md) are recipes. They guide you through the steps to involve addressing some common use-cases. They are more advanced than tutorials and asume some knoledge of how Oluso is designed and implemented.
* [Developers documentation](./developers/index.md) is the starting point for those that want to contribute to the Oluso SDK.

## First steps

If you are new to Oluso SDK usage, This is the place to start! There are several important things you must know before start using Oluso.

* **From scratch:** [Overview](./intro.md#from-scratch) | [Installation](./intro.md#installation)
* **Packages introduction:** [Core package](./developers/core/index.md) | [Data package](./developers/data/index.md)
* **Tutorials:** [How to write applications](./tutorial/application-development.md) | [How to structure microservices](./tutorial/microservices.md)

## Core package

This package is used by almost every other package in the Oluso SDK, because it contains a huge set of [Extensions](./developers/core/reference.md#extensions), [Helpers](./developers/core/reference.md#helpers) and other [Functionalities](./developers/core/reference.md) that simplify developers work, by reduceing the amount of duplicated code while developing any solution.

## Data package

By far this could be the most used package of all, because the increasent necesity of data store of any kind. Almost every software save data in some form, this means that we have the necesity to save that data into databases of some mind. The data package provide some tested patterns, [Repository](https://wikipedia.org/wiki/repository_pattern_software), [Unit of work](https://wikipedia.org/wiki/unit_of_work_pattern_software) and the [Specification](https://wikipedia.org/wiki/specification_pattern_software) are some of the provided examples. You as developer could use of this patterns, check the [Data package section](./developers/data/index.md) for a more detailed information about how to use them.