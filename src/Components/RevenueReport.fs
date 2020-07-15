module NBData.Components

open Feliz
open NBData.Models
open NBData.Data
open Feliz.UseDeferred
open Feliz.MaterialUI
open Feliz.MaterialUI.MaterialTable
open Fable.Core.Experimental

type State =
    { Rows: BudgetRow list}

let getTableHeader =
    Html.thead
        [ Html.th "Id"
          Html.th "FiscalYear"
          Html.th "RevenueCode"
          Html.th "Description"
          Html.th "Income"
          Html.th "Budget"
          Html.th "Projected"
          Html.th "Proposed"
          Html.th "ProjectedFiscalYear" ]

let toRow (budgetItem: BudgetRow): ReactElement =
    Html.tr
        [ Html.td budgetItem.Id
          Html.td budgetItem.FiscalYear
          Html.td budgetItem.RevenueCode
          Html.td budgetItem.Description
          Html.td budgetItem.Income
          Html.td budgetItem.Budget
          Html.td budgetItem.Projected
          Html.td budgetItem.Proposed
          Html.td budgetItem.ProjectedFiscalYear ]

let toMaterialTable (budgetRows: BudgetRow list) =
    let theme = Styles.useTheme ()

    Mui.materialTable
        [ prop.style [ style.backgroundColor theme.palette.background.``default`` ]
          materialTable.title "Newberry Revenue "
          materialTable.options [ options.filtering true ]
          materialTable.columns
              [ columns.column
                  [ column.title "Id"
                    column.field<BudgetRow> (fun rd -> nameof rd.Id) ]
                columns.column
                    [ column.title "Fiscal Year"
                      column.field<BudgetRow> (fun rd -> nameof rd.FiscalYear)
                      column.lookup<string, string> [ ("2019", "2019") ] ]
                columns.column
                    [ column.title "Revenue Code"
                      column.field<BudgetRow> (fun rd -> nameof rd.RevenueCode)
                      column.type'.numeric ]
                columns.column
                    [ column.title "Income"
                      column.field<BudgetRow> (fun rd -> nameof rd.Income)
                      column.type'.currency ]
                columns.column
                    [ column.title "Budget"
                      column.field<BudgetRow> (fun rd -> nameof rd.Budget)
                      column.type'.currency ]
                columns.column
                    [ column.title "Projected"
                      column.field<BudgetRow> (fun rd -> nameof rd.Projected)
                      column.type'.currency ]
                columns.column
                    [ column.title "Proposed"
                      column.field<BudgetRow> (fun rd -> nameof rd.Proposed)
                      column.type'.currency ]
                columns.column
                    [ column.title "Projected Fiscal Year"
                      column.field<BudgetRow> (fun rd -> nameof rd.ProjectedFiscalYear) ] ]
          materialTable.data budgetRows ]

let toTable budgetRows: ReactElement =
    let rows =
        budgetRows |> List.ofSeq |> Seq.map (toRow)

    Html.table
        [ getTableHeader
          Html.tableBody [ prop.children rows ] ]

let revenueReport' =
    React.functionComponent (fun (prop: State) -> Html.tbody [ prop.Rows |> toMaterialTable ])
