module Pages.NewberryRevenuePage

open Feliz
open NBData.Data
open Feliz.UseDeferred
open Feliz.MaterialUI
open Feliz.MaterialUI.MaterialTable
open Fable.Core.Experimental
open NBData.Components

let budgetSourcePath =
    "../json/Newberry_Revenue_Proposal_2020.json"

let getBudgetData =
    async {
        let! budgetData = getBudget budgetSourcePath
        return budgetData
    }

let render =
    React.functionComponent (fun () ->
        let budgetData = React.useDeferred (getBudgetData, [||])

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
            | Deferred.Resolved content -> revenueReport' { Rows = content }

        Html.div [ dataRowsResult ])

