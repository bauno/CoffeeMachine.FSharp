#r "paket: nuget Fake.DotNet.Cli
open System.Windows.Forms.ScrollableControl
nuget Fake.IO.FileSystem
nuget Fake.Core.Target
nuget Fake.IO.Zip
nuget Fake.DotNet.AssemblyInfoFile //"
#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let consoleDir = "Console"
let webApiDir = "WebApi"
let publishDirConsole  = "publish/" + consoleDir
let publishDirWebApi  = "publish/" + webApiDir

let deployDir = "deploy/"

let dockerDir = "docker/"
let dockerDeploy = "CoffeeMachine.WebApi"
let zipFile = "CoffeeMachine"
let configFile = "CoffeeMachine.WebApi.exe.config"


Target.create "Clean" (fun _ ->    
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ publishDirConsole
    ++ publishDirWebApi
    ++ deployDir
    ++ (dockerDir + dockerDeploy)
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

Target.create "Publish" (fun _ ->
    "src/app/CoffeeMachine.Main/CoffeeMachine.Main.fsproj"
    |> DotNet.publish (fun options ->             
            {options with OutputPath =  Some(options.Common.WorkingDirectory + "/" +  publishDirConsole)}
        )
    "src/app/CoffeeMachine.WebApi/CoffeeMachine.WebApi.fsproj"
    |> DotNet.publish (fun options ->             
            {options with OutputPath =  Some(options.Common.WorkingDirectory + "/" +  publishDirWebApi)}
        )
)

Target.create "Deploy" (fun _ ->
    !! (publishDirConsole + "/*")  
    ++ (publishDirConsole + "/**/*")
    |> Zip.zip publishDirConsole (deployDir + zipFile + ".Console.zip")

    !! (publishDirWebApi + "/*")  
    ++ (publishDirWebApi + "/**/*")
    |> Zip.zip publishDirWebApi (deployDir + zipFile + ".WebApi.zip")
)

Target.create "Docker"(fun _ ->
    Shell.copyDir ("./src/docker/CoffeeMachine.WebApi") ("./publish/WebApi") (fun _ -> true)
  )

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "Test"
  ==> "Publish"
  ==> "Deploy"
  ==> "Docker"
  ==> "All"

Target.runOrDefault "Test"
