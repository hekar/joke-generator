# README

[![Build status](https://ci.appveyor.com/api/projects/status/y63qgr64qymqvh2r?svg=true)](https://ci.appveyor.com/project/hekar/joke-generator)


## Installation

Install [dotnet core](https://dotnet.microsoft.com/download/dotnet-core) using the [dotnet-install](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script) script

```
./dotnet-install.sh -Version 3.1.201
```

```
git clone git@github.com:hekar/joke-generator.git
dotnet restore
```

## Execute

```
dotnet run
```

## Testing

The project only contains unit tests. Integration tests will need to be added in the future.
There would need to a mechanism to mock the console and web server.

### Execute Unit Tests

```
cd JokeGeneratorUnitTests
dotnet test
```

### Code Coverage (dotcover)

[Dotcover](https://www.jetbrains.com/dotcover/) gathers code coverage results.

#### Install Dotnet 2.2.x

[Dotcover](https://www.jetbrains.com/dotcover/) requires [dotnet core 2.2.x](https://dotnet.microsoft.com/download/dotnet-core/2.2).

```
./dotnet-install.sh -Version 2.2.207
```

#### Analyze Coverage

```
cd JokeGeneratorUnitTests
dotnet dotcover test --dotCoverXml=coverage.xml
```

##### View Results

The default coverage.xml generates an HTML report under `JokeGeneratorUnitTests/dotCover.html`.

```
xdg-open JokeGeneratorUnitTests/dotCover.html
```