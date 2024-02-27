# VSCode Configurator

This is a CLI tool to quickly bootstrap a new project for VSCode. More specifically, it's to reduce the amount of common tasks I perform.

> ⚠️ **Note:**
> 
> It's mainly just a tool I'm writing for myself.

## 📄 Docs

You can view the docs for installing and using the CLI here:

- [Docs](./docs/README.md)

## 🏗️ Building from source

### 🧰 Pre-requisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - You will also need to install the pre-requisites for your platform [located here](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=net7%2Cwindows#prerequisites).
        - For Linux based platforms, you primarily need to ensure that packages for `clang` and `zlib` (dev) packages are installed to your system.

### 🧱 Building

> **⚠️ Note:**
> 
> Before building, you need to know the ["runtime identifier"](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids) for your platform. For simplicity, these docs will use `linux-x64`. Replace that value with what you use, if needed.
> 
> For example if:
> * You're building on a **x64 Linux-based system**, the identifier would be `linux-x64`.
> * You're building on an **Apple-silicon macOS system**, the identifier would be `osx-arm64`.

#### Command-line

1. Set your current directory to where you cloned the repo.
2. Run the following command:

```plain
dotnet publish ./src/Configurator/ --configuration "Release" --runtime "linux-x64" --self-contained
```

The compiled binary will be located in the `./artifacts/publish/Configurator/` directory in the local repo.

#### Visual Studio Code

1. Open the command palette (`Shift+Ctrl+P` **(Windows/Linux)** / `Shift+Cmd+P` **(macOS)**).
2. Type in **Tasks: Run Task** and press `Enter`.
   * **Ensure that is the selected option before pressing `Enter`.**
3. Select **Publish: Configurator**.
4. Select your platform's runtime identifier.

The compiled binary will be located in the `./artifacts/publish/Configurator/` directory in the local repo.

## 🗂️ Dependencies used

- [`System.CommandLine`](https://github.com/dotnet/command-line-api)

## 🤝 License

The source code for this project is licensed with the [MIT License](LICENSE).
