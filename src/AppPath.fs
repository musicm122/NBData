module AppPath

open Elmish
open Elmish.React
open Feliz
open Feliz.Router
open Pages

type State = { CurrentUrl : string list }

type Msg =
    | UrlChanged of string list
    | NavigateRevenue
    | NavigateUsers
    | NavigateToUser of int
    | NavigateToUserReplaceState of int

let init() = { CurrentUrl = Router.currentPath() }, Cmd.none

let commands() = [
    Router.navigatePath("one", "two", "three")
    Router.navigatePath("user", 15)
    Router.navigatePath("one", "two", [ "limit", "10"; "id", "20" ])
]

let update msg state =
    match msg with
    | UrlChanged segments -> { state with CurrentUrl = segments }, Cmd.none
    | NavigateRevenue -> state, Router.navigatePath("revenue")
    | NavigateUsers -> state, Router.navigatePath("users")
    | NavigateToUser userId -> state, Router.navigatePath("users", [ "id", userId ])
    | NavigateToUserReplaceState userId -> state, Router.navigatePath("users", [ "id", userId ], HistoryMode.ReplaceState)

let render state dispatch =
    let currentPage =
        match state.CurrentUrl with        
        | [ ]  ->              
             Html.div [
                NewberryRevenuePage.render()
             ]
        | [ "revenue"; Route.Query [ "year", Route.Number year ] ] ->
                NewberryRevenuePage.render()   
        | [ "users" ] ->
            Html.div [
                Html.button [
                    prop.text "Single User (History.PushState)"
                    prop.onClick (fun _ -> dispatch (NavigateToUser 10))
                ]

                Html.button [
                    prop.text "Single User (History.ReplaceState)"
                    prop.onClick (fun _ -> dispatch (NavigateToUserReplaceState 10))
                ]

                Html.button [
                    prop.text "Log Router.currentUrl()"
                    prop.onClick(fun _ -> Browser.Dom.console.log(Router.currentUrl() |> Array.ofList))
                ]

                Html.button [
                    prop.text "Log Router.formatPath(\"users\")"
                    prop.onClick(fun _ -> Browser.Dom.console.log(Router.formatPath("users")))
                ]

                Html.a [
                    prop.href (Router.formatPath("users", ["id", 10]))
                    prop.text "Single User link"
                ]
            ]

        | [ "users"; Route.Int userId ] ->
            Html.h1 (sprintf "User ID %d" userId)

        | [ "users"; Route.Query [ "id", Route.Int userId ] ] ->
            Html.div [
                Html.h1 (sprintf "Query String => User ID %d" userId)

                Html.button [
                    prop.text "Log Router.currentUrl()"
                    prop.onClick(fun _ -> Browser.Dom.console.log(Router.currentUrl() |> Array.ofList))
                ]

                Html.button [
                    prop.text "Log Router.currentPath()"
                    prop.onClick(fun _ -> Browser.Dom.console.log(Router.currentPath() |> Array.ofList))
                ]
            ]

        | [ "users"; Route.Query [ "search", username ] ] ->
            Html.div [
                Html.h1 (sprintf "Query string => Username %s" username)

                Html.button [
                    prop.text "Log Router.currentUrl()"
                    prop.onClick(fun _ -> Browser.Dom.console.log(Router.currentUrl() |> Array.ofList))
                ]

                Html.button [
                    prop.text "Log Router.currentPath()"
                    prop.onClick(fun _ -> Browser.Dom.console.log(Router.currentPath() |> Array.ofList))
                ]
            ]

        | _ ->
            Html.h1 "Not Found"

    Router.router [
        Router.pathMode
        Router.onUrlChanged (UrlChanged >> dispatch)
        Router.application [
            Html.div [
                prop.style [ style.padding 20 ]
                prop.children [
                    currentPage
                ]
            ]
        ]
    ]