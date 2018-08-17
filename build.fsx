#r "paket: nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.DotNet.AssemblyInfoFile //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.create "Clean" (fun _ ->    
    !! "src/**/bin"
    ++ "src/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    AssemblyInfoFile.createFSharp "./src/app/CoffeeMachine.Core/Properties/AssemblyInfo.fs"
        [AssemblyInfo.InternalsVisibleTo "CoffeeMachine.Tests.Xunit"]
    !! "src/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)


Target.create "Test" (fun _ ->
    !! "src/test/**/*.fsproj"
    |> Seq.iter (DotNet.test id)
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "All"

Target.runOrDefault "All"
