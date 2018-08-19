#r "paket: nuget Fake.DotNet.Cli
open Fake.SystemHelper
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.DotNet.AssemblyInfoFile //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let buildDir  = "build/"
let levelDir = "./levels/"
let deployDir = "./deploy/"
let dockerDir = "./docker/"
let dockerDeploy = "CoffeeMachine"
let zipFile = "CoffeeMachine.zip"
let configFile = "CoffeeMachine.WebApi.exe.config"


Target.create "Clean" (fun _ ->    
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ buildDir
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

Target.create "Deploy" (fun _ ->
    "src/app/CoffeeMachine.Main/CoffeeMachine.Main.fsproj"
    |> DotNet.publish (fun options ->             
            Trace.log (sprintf "Working dir: %s" options.Common.WorkingDirectory)
            {options with OutputPath =  Some(options.Common.WorkingDirectory + "/" +  buildDir)}
        )
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "Deploy"
  ==> "All"

Target.runOrDefault "Test"
