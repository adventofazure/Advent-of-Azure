module.exports = async function (context, req) {
    context.log('JavaScript HTTP trigger function processed a request.');

    const input = (req.query.input || (req.body && req.body.input));
    const responseMessage = input
        ? "Day 1 is not implemented yet. Your input was " + input
        : "Day 1. No input given.";

    context.res = {
        // status: 200, /* Defaults to 200 */
        body: responseMessage
    };
}