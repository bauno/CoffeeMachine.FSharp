module CoffeeMachine.Maker

open DrinkMaker.Data
open DrinkMaker.Core
open DrinkMaker.OrderParser.Main
open QuantityChecker
open DrinkMaker.QuantityChecker.Core
open CoffeeMachine.PriceList
open System
open Chessie.ErrorHandling
open DrinkRepository.Main

open System.Configuration


let isEmpty = Convert.ToBoolean(ConfigurationManager.AppSettings.Item("Empty"))

let makeBeverage =
  let railway =
    parseOrder
    >> bind ``Check that drink exists``
    //>>= (dummy1 >> dummy2)
    >> lift putStick
    >> bind (checkMoneyAndSetListPrice priceOf)
    >> bind ``check that beverage makes sense``
    >> bind  (checkQuantity (fun b -> isEmpty) (ignore))
    >> lift saveIntoDb
  makeBeverage' railway

let makeAnotherBeverage orderStr =
    
  let ePut  = 
      lift putStick 

  let processedOrder = 
    parseOrder orderStr
    >>= ``Check that drink exists``

  let beverage = putStick <!> processedOrder

  let post b = 
    checkMoneyAndSetListPrice priceOf b
    >>= ``check that beverage makes sense``

  let inline (=>>) result f  = lift f  result 

  let create orderStr =
    parseOrder orderStr
    >>= ``Check that drink exists``
    =>> putStick
    >>= (checkMoneyAndSetListPrice priceOf)
    >>= ``check that beverage makes sense``
    >>= (checkQuantity (fun b -> isEmpty) (ignore))
    =>> saveIntoDb 

  makeBeverage'' create
  // makeBeverage' railway
  // let processResult = putStick <!> railway 
  // let saveResult = saveIntoDb <!> processResult
  
  // validate
  // let validateOrder orderStr =
  //   let validate o b = 
  //     let parseResult = parseOrder o
  //     let validateResult = ``Check that drin exists`` parseResult
  //   parseOrder orderStr
  //   >>= ``Check that drink esists``
  
  //let makeDrink beverage = 
  //create 
  //save
