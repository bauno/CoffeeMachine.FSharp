module CoffeeMachine.DrinkRepository.Main

open CoffeeMachine.DrinkRepository.Core

open System.Configuration   


let connectionString =  ConfigurationManager.AppSettings.Item("ConnectionString")


let dbName = ConfigurationManager.AppSettings.Item("DbName")

NamelessInteractive.FSharp.MongoDB.SerializationProviderModule.Register()
NamelessInteractive.FSharp.MongoDB.Conventions.ConventionsModule.Register()

let drinkRepository = drinkRepository'' connectionString dbName

let saveIntoDb =  fst drinkRepository
