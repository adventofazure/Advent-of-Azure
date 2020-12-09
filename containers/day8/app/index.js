const express = require('express');
const morgan = require('morgan');
const bodyParser = require('body-parser');

const app = express();
app.use(morgan('combined'));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

const parseCommands = (input) => {
  const commands = input.split(',');
  var parsedCommands = [];
  commands.forEach(element => {
    const parts = element.split(' ');
    const command = parts[0];
    const value = parts[1];
    const parsedCommand = {
      command,
      value: +value,
      executed: false
    };
    parsedCommands.push(parsedCommand);
  });
  return parsedCommands;
};

const solvePart1 = (input) => {
  console.log("Solving part 1");
  const parsedCommands = parseCommands(input);
  var acc = 0;
  var commandIndex = 0;
  var commandExecutedTwice = false;
  while (!commandExecutedTwice) {
    const parsedCommand = parsedCommands[commandIndex];
    if (parsedCommand.executed) {
      commandExecutedTwice = false;
      break;
    }
    parsedCommands[commandIndex].executed = true;
    if (parsedCommand.command === 'nop') {
      commandIndex++;
    } else if (parsedCommand.command === 'acc') {
      acc += parsedCommand.value;
      commandIndex++;
    } else if (parsedCommand.command === 'jmp') {
      commandIndex += parsedCommand.value;
    }
  }
  return acc;
};

const solvePart2 = (input) => {
  console.log("Solving part 2");
  return 2;
};

app.post('/api/day8', (req, res) => {
  const input = req.body.input;
  if (input) {
    res.send("Part 1: " + solvePart1(input) + "\n Part 2: " + solvePart2(input));
  } else {
    res.send('Day 8. No input given.');
  }
});

var listener = app.listen(process.env.PORT || 5050, function() {
 console.log('listening on port ' + listener.address().port);
});
