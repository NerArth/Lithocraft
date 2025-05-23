# How to Use Custom Build Targets
## Build a Specific Module:
Run the following command in the terminal from the project directory:

```batch
dotnet msbuild -t:BuildCore -p:Module=core
```
This will build the Lithocraft-Core module and place the output in bin/Release/Lithocraft-Core/.

## Build All Modules:
Run this command:

```batch
dotnet msbuild -t:BuildAllModules -p:Module=all
```
This will build all modules in sequence and output them to their respective directories.