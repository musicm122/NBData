module NBData.Router

open Browser
open Fable.React.Props
open Elmish.Navigation
open Elmish.UrlParser
open Feliz.Router
open Feliz


type State = { CurrentUrl : string list }
type Msg = UrlChanged of string list

let init() = { CurrentUrl = Router.currentUrl() }

let update (UrlChanged segments) state =
    { state with CurrentUrl = segments }

let render state dispatch =
    let currentPage =
        match state.CurrentUrl with
        | [ ] -> Html.h1 "Home"
        | [ "users" ] -> Html.h1 "Users page"
        | [ "users"; Route.Int userId ] -> Html.h1 (sprintf "User ID %d" userId)
        | _ -> Html.h1 "Not found"

    Router.router [
        Router.onUrlChanged (UrlChanged >> dispatch)
        Router.application currentPage
    ]
