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
    let getCountPart1 (log: ILogger) (groupAnswers: string) =
        let answers =
            groupAnswers.Split(",")
            |> List.ofArray
            |> List.fold (+) ""
            |> Seq.toList
            |> List.fold (fun (acc: Set<char>) (a: char) -> if acc.Contains a then acc else acc.Add a) Set.empty
        answers.Count

    // Solve Advent of Code 2020, day 6, part 1
    let solvePart1 (log: ILogger) (input: string) =
        log.LogInformation("Solving part 1")
        input.Split(",,")
        |> List.ofArray
        |> List.sumBy (fun a -> getCountPart1 log a )
        |> string

    let getCountPart2 (log: ILogger) (groupAnswers: string) =
        let answers =
            groupAnswers.Split(",")
            |> List.ofArray
        let personCount = answers.Length

        let handleAnswer (acc: Map<char, int>) (x: char) =
            match acc.TryGetValue x with
              | true, value -> acc.Add(x, value + 1)
              | _ -> acc.Add(x, 1)

        let answerMap =
            answers
            |> List.fold (+) ""
            |> Seq.toList
            |> List.fold handleAnswer Map.empty
        let filteredMap = answerMap |> Map.filter (fun a b -> b = personCount)
        filteredMap.Count

    // Solve Advent of Code 2020, day 6, part 2
    let solvePart2 (log: ILogger) (input: string) =
        log.LogInformation("Solving part 2")
        input.Split(",,")
        |> List.ofArray
        |> List.sumBy (fun a -> getCountPart2 log a )
        |> string

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
                    "Part 1: " + (solvePart1 log input) + "\n Part 2: " + (solvePart2 log input)

            return OkObjectResult(responseMessage) :> IActionResult
        } |> Async.StartAsTask