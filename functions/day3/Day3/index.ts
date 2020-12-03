import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const solve = (context: Context, input: string[], right: number, down: number) => {
    context.log('Solving with parameters: right: ' + right + ', down: ' + down);
    var treeAmount = 0;
    var x = 0;
    var y = 0;
    while (y < input.length-1) {
        // Move right
        for (var i = 0; i < right; i++) {
            x++;
            if (x === input[y].length) {
                x = 0;
            }
        }
        // Move down
        y += down;
        const row = input[y];
        const item = row.charAt(x);
        if (item === '#') {
            treeAmount++;
        }
    }
    return treeAmount;
};

// Solve Advent of Code 2020 part 1
const solvePart1 = (context, input) => {
    context.log('Solving part 1');
    return solve(context, input, 3, 1);
}

// Solve Advent of Code 2020 part 2
const solvePart2 = (context, input) => {
    context.log('Solving part 2');
    const first = solve(context, input, 1, 1);
    const second = solve(context, input, 3, 1);
    const third = solve(context, input, 5, 1);
    const fourth = solve(context, input, 7, 1);
    const fifth = solve(context, input, 1, 2);
    return first * second * third * fourth * fifth;
}

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    const input = (req.query.input || (req.body && req.body.input));
    const responseMessage = input
        ? 'Part 1: ' + solvePart1(context, input.split(',')) + '\n'
        + 'Part 2: ' + solvePart2(context, input.split(','))
        : 'Day 3. No input given.';

    context.res = {
        // status: 200, /* Defaults to 200 */
        body: responseMessage
    };
};

export default httpTrigger;