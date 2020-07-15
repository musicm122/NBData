module Pages.NewberryRevenuePage

open Feliz
open NBData.Data
open Feliz.UseDeferred
open NBData.Components
open NBData.Models

let budgetSourcePath =
    "../json/Newberry_Revenue_Proposal_2020.json"

let getBudgetData =
    async {
        let! budgetData = getBudget budgetSourcePath
        return budgetData
    }

let render =
    React.functionComponent (fun (props: RevenueQueryData) ->
        let budgetData = React.useDeferred (getBudgetData, [||])
        let hasFilters = props.Description.Trim().Length > 0 || props.Year.Trim().Length > 0
        let dataRowsResult =
            match budgetData with
            | Deferred.HasNotStartedYet -> Html.none
            | Deferred.InProgress ->
                Html.i
                    [ prop.className
                        [ "fa"
                          "fa-refresh"
                          "fa-spin"
                          "fa-2x" ] ]
            | Deferred.Failed errorstate -> Html.div errorstate.Message
            | Deferred.Resolved content ->
                match props with 
                | p when p.Description.Trim().Length > 0 -> 
                    let filteredRows = 
                        content |> 
                            List.filter(fun (row:BudgetRow) -> row.FiscalYear = props.Year)
                    revenueReport' { Rows = filteredRows;  }    
                | p when p.Year.Trim().Length > 0 -> 
                    let filteredRows = 
                        content |> 
                            List.filter(fun (row:BudgetRow) -> row.FiscalYear = props.Year)
                    revenueReport' { Rows = filteredRows;  }        
                | false -> 
                    revenueReport' { Rows = content;  }

        Html.div [ dataRowsResult ])
