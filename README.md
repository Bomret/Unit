[![Nuget](https://img.shields.io/nuget/v/TheVoid.svg)](https://www.nuget.org/packages/TheVoid/)

|CI Provider|Status|
|----------|------------|
|Travis CI|[![Travis CI](https://img.shields.io/travis/Bomret/Unit/master.svg)](https://travis-ci.org/Bomret/Unit)|
|AppVeyor|[![AppVeyor](https://img.shields.io/appveyor/ci/stefanreichel/unit/master.svg)](https://ci.appveyor.com/project/StefanReichel/unit)|

# Unit

The Unit type allows only one value which is empty. This is similar to the `System.Void` type (`void` keyword) which can sadly not be used by user code. Using the Unit type as return value instead of `void` offers some benefits, such as the deprecation of non-generic or special classes for the purpose of communicating the absence of a return value (`Task<Unit>` would replace `Task`, `Func<Unit>` et al. would make `Action` et al. obsolete).

Other languages, such as F#, Haskell or Scala already provide a Unit type, but Microsoft chose to prevent using the existing `System.Void` type in C# user code in favor of the `void` keyword.

## Usage
Use `Unit` as return value instead of the `void` keyword.

```csharp
// use
using TheVoid;

public Unit DoSomething() {
  //...
}

// instead of
public void DoSomething() {
  //...
}
```

To "return" Unit simply use one of the following
  * `Unit.Default` *(recommended)*
  * `default(Unit)`
  * `new Unit()`

## Similar projects
* [@gregoryyoung](https://github.com/gregoryyoung) created a similar type which he called `Nothing` but it missed some things.
* [@Reactive-Extensions](https://github.com/Reactive-Extensions/Rx.NET) the .net project contains a `System.Unit` type which is not available as a standalone package.

## Planned deprecation
This project is deprecated as soon as any Unit type is included in C#.

## Maintainer(s)
* [@bomret](https://github.com/bomret)