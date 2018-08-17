module CoffeeMachine.WebApi

open Suave                 // always open suave
open System.Net

open CoffeeMachine.RestApp

let ipZero = IPAddress.Parse("0.0.0.0")
let port = System.Environment.GetEnvironmentVariable("PORT")

let cfg =
        { defaultConfig with
            bindings =
                 [ HttpBinding.create HTTP ipZero (uint16 port) ]
               }

[<EntryPoint>]
let main argv =
    startWebServer cfg restMachine
    0 // return an integer exit code
