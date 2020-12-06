package com.function;

import com.microsoft.azure.functions.ExecutionContext;
import com.microsoft.azure.functions.HttpMethod;
import com.microsoft.azure.functions.HttpRequestMessage;
import com.microsoft.azure.functions.HttpResponseMessage;
import com.microsoft.azure.functions.HttpStatus;
import com.microsoft.azure.functions.annotation.AuthorizationLevel;
import com.microsoft.azure.functions.annotation.FunctionName;
import com.microsoft.azure.functions.annotation.HttpTrigger;

import java.util.Optional;

/**
 * Day 5 of Advent of Code 2020 as an Azure function.
 */
public class Function {

    @FunctionName("Day5")
    public HttpResponseMessage run(
            @HttpTrigger(
                name = "req",
                methods = {HttpMethod.GET, HttpMethod.POST},
                authLevel = AuthorizationLevel.ANONYMOUS)
                HttpRequestMessage<Optional<String>> request,
            final ExecutionContext context) {
        context.getLogger().info("Java HTTP trigger processed a request.");

        // Parse query parameter
        final String query = request.getQueryParameters().get("input");
        final String input = request.getBody().orElse(query);

        if (input == null) {
            return request.createResponseBuilder(HttpStatus.BAD_REQUEST).body("Day 5. No input given.").build();
        } else {
            return request.createResponseBuilder(HttpStatus.OK).body("Day 5 is not implemented yet. Your input is " + input).build();
        }
    }
}
