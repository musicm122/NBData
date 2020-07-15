module NBData.Data

open Fable
open Fable.SimpleJson
open Fable.SimpleHttp
open NBData.Models

let getBudget (path: string): Async<BudgetRow list> =
    async {
        let! (statusCode, responseText) = Http.get path
        match statusCode with
        | 200 -> return responseText |> Json.parseAs<BudgetRow list>
        | _ ->
            printfn "Status %d => %s" statusCode responseText
            return []
    }
