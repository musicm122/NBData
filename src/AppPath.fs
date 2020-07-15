module AppPath

open Elmish
open Feliz
open Feliz.Router
open Pages
open NBData.Models

type State = { CurrentUrl: string list }

type Msg =
    | UrlChanged of string list    
    | NavigateRevenue 
    | NavigateRevenueQuery of RevenueQueryData   

let init () =
    { CurrentUrl = Router.currentPath(); }, Cmd.none

let update msg state =
    match msg with
    | UrlChanged segments -> { state with CurrentUrl = segments }, Cmd.none
    | NavigateRevenue -> state, Router.navigatePath ("revenue")
    | NavigateRevenueQuery query -> state, Router.navigatePath ("revenue", ["year", query.Year])

let render state dispatch =
    let currentPage =
        match state.CurrentUrl with        
        | [] -> Html.div [ NewberryRevenuePage.render () ]
        | [ "revenue"; Route.Query [ "year", Route.Number year ] ] -> NewberryRevenuePage.render()
        | _ -> Html.h1 "Not Found"

    Router.router
        [ Router.pathMode
          Router.onUrlChanged (UrlChanged >> dispatch)
          Router.application
              [ Html.div
                  [ prop.style [ style.padding 20 ]
                    prop.children [ currentPage ] ] ] ]
