module NBData.Data

open Fable
open Fable.SimpleJson
open Fable.SimpleHttp

type BudgetRow =
    { Id: int
      FiscalYear: string
      RevenueCode: int
      Description: string
      Income: float
      Budget: float
      Projected: float
      Proposed: float
      ProjectedFiscalYear: string }

let getBudget (path: string): Async<BudgetRow list> =
    async {
        let! (statusCode, responseText) = Http.get path
        match statusCode with
        | 200 -> return responseText |> Json.parseAs<BudgetRow list>
        | _ ->
            printfn "Status %d => %s" statusCode responseText
            return []
    }
