// Solve the day 1 puzzle of Advent of Code 2020 part 1
function solvePart1(context, input) {
    context.log('Solving day 1, part 1');
    const splittedInput = input.split(',');
    for (var i = 0; i < splittedInput.length; i++) {
        const firstNumber = parseInt(splittedInput[i]);
        for (var j = i + 1; j < splittedInput.length; j++) {
            const secondNumber = parseInt(splittedInput[j]);
            if (firstNumber + secondNumber === 2020) {
                return firstNumber * secondNumber;
            }
        }
    }
    return 0;
}

// Solve the day 1 puzzle of Advent of Code 2020 part 2
function solvePart2(context, input) {
    context.log('Solving day 1, part 2');
    const splittedInput = input.split(',');
    for (var i = 0; i < splittedInput.length; i++) {
        const firstNumber = parseInt(splittedInput[i]);
        for (var j = i + 1; j < splittedInput.length; j++) {
            const secondNumber = parseInt(splittedInput[j]);
            for (var k = j + 1; k < splittedInput.length; k++) {
                const thirdNumber = parseInt(splittedInput[k]);
                if (firstNumber + secondNumber + thirdNumber === 2020) {
                    return firstNumber * secondNumber * thirdNumber;
                }
            }
        }
    }
    return 0;
}

module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const input = (req.query.input || (req.body && req.body.input));
    const part = (req.query.part || (req.body && req.body.part));
    const result = input
        ? 'Part 1: ' + solvePart1(context, input) + '\n'
        + 'Part 2: ' + solvePart2(context, input)
        : 'Day 1. No input given.';

    context.res = {
        // status: 200, /* Defaults to 200 */
        body: result
    };
}