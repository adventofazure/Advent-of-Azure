namespace Company.Function

open System
open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Newtonsoft.Json
open Microsoft.Extensions.Logging

module Day6 =
    // Define a nullable container to deserialize into.
    [<AllowNullLiteral>]
    type InputContainer() =
        member val Input = "" with get, set

    // For convenience, it's better to have a central place for the literal.
    [<Literal>]
    let Input = "input"

    [<FunctionName("Day6")>]
    let run ([<HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("F# HTTP trigger function processed a request.")

            let inputOpt = 
                if req.Query.ContainsKey(Input) then
                    Some(req.Query.[Input].[0])
                else
                    None

            use stream = new StreamReader(req.Body)
            let! reqBody = stream.ReadToEndAsync() |> Async.AwaitTask

            let data = JsonConvert.DeserializeObject<InputContainer>(reqBody)

            let input =
                match inputOpt with
                | Some n -> n
                | None ->
                   match data with
                   | null -> ""
                   | ic -> ic.Input
            
            let responseMessage =             
                if (String.IsNullOrWhiteSpace(input)) then
                    "Day 6. No input given."
                else
                    "Day 6 is not implemented yet. Your input is " + input

            return OkObjectResult(responseMessage) :> IActionResult
        } |> Async.StartAsTask