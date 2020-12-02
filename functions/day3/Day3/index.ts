import { AzureFunction, Context, HttpRequest } from "@azure/functions"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
    context.log('HTTP trigger function processed a request.');
    const input = (req.query.input || (req.body && req.body.input));
    const responseMessage = input
        ? "Day 3 is not implemented yet. Your input is " + input
        : "Day 3. No input given.";

    context.res = {
        // status: 200, /* Defaults to 200 */
        body: responseMessage
    };

};

export default httpTrigger;