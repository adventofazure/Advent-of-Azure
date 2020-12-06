package com.function;

import com.microsoft.azure.functions.ExecutionContext;
import com.microsoft.azure.functions.HttpMethod;
import com.microsoft.azure.functions.HttpRequestMessage;
import com.microsoft.azure.functions.HttpResponseMessage;
import com.microsoft.azure.functions.HttpStatus;
import com.microsoft.azure.functions.annotation.AuthorizationLevel;
import com.microsoft.azure.functions.annotation.FunctionName;
import com.microsoft.azure.functions.annotation.HttpTrigger;
import java.util.logging.Logger;
import java.util.stream.Collectors;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

class SeatInformation {
    private int seatId;
    private int row;
    private int column;

    public SeatInformation(int seatId, int row, int column) {
        this.seatId = seatId;
        this.row = row;
        this.column = column;
    }

    public int getSeatId() {
        return seatId;
    }

    public int getRow() {
        return row;
    }

    public int getColumn() {
        return column;
    }
}

/**
 * Day 5 of Advent of Code 2020 as an Azure function.
 */
public class Function {

    private SeatInformation getSeatInformation(Logger logger, String boardingPass) {
        if (boardingPass.length() != 10) {
            return null;
        }
        // Find the row
        int currentRowSize = 128;
        int currentRowMin = 0;
        int currentRowMax = currentRowSize - 1;
        for (int i = 0; i < 7; i++) {
            currentRowSize /= 2;
            char direction = boardingPass.charAt(i);
            if (direction == 'F') {
                currentRowMax -= currentRowSize;
            }
            else if (direction == 'B') {
                currentRowMin += currentRowSize;
            }
        }
        if (currentRowMin != currentRowMax) {
            return null; // Something went wrong
        }
        int row = currentRowMin;
        // Find the column
        int currentColumnSize = 8;
        int currentColumnMin = 0;
        int currentColumnMax = 7;
        for (int i = 7; i < 10; i++) {
            currentColumnSize /= 2;
            char direction = boardingPass.charAt(i);
            if (direction == 'L') {
                currentColumnMax -= currentColumnSize;
            }
            else if (direction == 'R') {
                currentColumnMin += currentColumnSize;
            }
        }
        if (currentColumnMin != currentColumnMax) {
            return null; // Something went wrong
        }
        int column = currentColumnMin;
        int seatId = row * 8 + column;
        return new SeatInformation(seatId, row, column);
    }

    private List<SeatInformation> getSeats(Logger logger, String input) {
        List<SeatInformation> seats = new ArrayList<SeatInformation>();
        String[] parts = input.split(",");
        for (String part : parts) {
            SeatInformation seatInformation = getSeatInformation(logger, part);
            if (seatInformation != null) {
                seats.add(seatInformation);
            }

        }
        return seats;
    }

    private int solvePart1(Logger logger, String input) {
        List<SeatInformation> seats = getSeats(logger, input);
        int highestSeatId = 0;
        for (SeatInformation seatInformation : seats) {
            if (seatInformation.getSeatId() > highestSeatId) {
                highestSeatId = seatInformation.getSeatId();
            }
        }
        return highestSeatId;
    }

    private int solvePart2(Logger logger, String input) {
        List<SeatInformation> seats = getSeats(logger, input);
        List<SeatInformation> filtered = seats.stream().filter(item -> item.getRow() != 0 && item.getRow() != 127).collect(Collectors.toList());
        Map<Integer, SeatInformation> seatMap = new HashMap<Integer, SeatInformation>();
        for (SeatInformation seatInformation : filtered) {
            seatMap.put(seatInformation.getSeatId(), seatInformation);
        }
        int seatMissingId = -1;
        for (int row = 1; row < 127; row++) {
            for (int column = 0; column < 8; column++) {
                int seatId = row * 8 + column;
                if (seatMap.get(seatId) == null && seatMap.get(seatId - 1) != null && seatMap.get(seatId + 1) != null) {
                    logger.info("Found " + seatId);
                    seatMissingId = seatId;
                }
            }
        }
        return seatMissingId;
    }

    @FunctionName("Day5")
    public HttpResponseMessage run(
            @HttpTrigger(
                name = "req",
                methods = {HttpMethod.GET, HttpMethod.POST},
                authLevel = AuthorizationLevel.ANONYMOUS)
                HttpRequestMessage<Optional<String>> request,
            final ExecutionContext context) {
        Logger logger = context.getLogger();
        logger.info("Java HTTP trigger processed a request.");

        // Parse query parameter
        final String query = request.getQueryParameters().get("input");
        final String input = request.getBody().orElse(query);

        if (input == null) {
            return request.createResponseBuilder(HttpStatus.BAD_REQUEST).body("Day 5. No input given.").build();
        } else {
            return request.createResponseBuilder(HttpStatus.OK).body("Part 1: " + solvePart1(logger, input) + "\n Part 2: " + solvePart2(logger, input)).build();
        }
    }
}
