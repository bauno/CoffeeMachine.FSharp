module CoffeeMachine.DrinkRepository.Data

open DrinkMaker.Data
open MongoDB.Bson


type BeverageReportDb = {
    Id: BsonObjectId
    Beverage: BeverageType
    Price: float
}


type BeverageReport = {
    Beverage: BeverageType
    Price: float
}