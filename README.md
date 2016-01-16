[![Issue Stats](http://issuestats.com/github/bomret/Unit/badge/issue)](http://issuestats.com/github/bomret/Unit)
[![Issue Stats](http://issuestats.com/github/bomret/Unit/badge/pr)](http://issuestats.com/github/bomret/Unit)

|CI Provider|Status|
|----------|------------|
|Travis CI|[![Travis CI](https://travis-ci.org/Bomret/Unit.svg?branch=master)](https://travis-ci.org/Bomret/Unit)|
|AppVeyor|[![AppVeyor](https://ci.appveyor.com/api/projects/status/muw7l4o91ohjoui5?svg=true)](https://ci.appveyor.com/project/StefanReichel/unit)|

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