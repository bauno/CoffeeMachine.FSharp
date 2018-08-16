module CoffeeMachine.Main

open CoffeeMachine.Core
open CoffeeMachine.Maker
open CoffeeMachine.DrinkRepository.Main
open Chessie.ErrorHandling
open System.Collections.Generic

let railway =  
  displayMessage
  >> bind invalidOrder
  >> bind (print'' drinkRepository display)
  >> bind (takeOrder'' makeBeverage)


let make =
  make' railway
  //make' parallelRailway

let usage args =
  if Array.length args <> 1
    then printfn "Usage is CoffeeMachine report|<order>"
         false
    else true

let printReceipt () =
  let receipt = new List<string>();
  let display = fun line -> receipt.Add(line)
  printReceipt'' drinkRepository display
  receipt
