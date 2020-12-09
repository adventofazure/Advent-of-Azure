const express = require('express');
const morgan = require('morgan');
const bodyParser = require('body-parser');

const app = express();
app.use(morgan('combined'));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

const solvePart1 = (input) => {
  console.log("Solving part 1");
  return 1;
};

const solvePart2 = (input) => {
  console.log("Solving part 2");
  return 2;
};

app.post('/api/day8', (req, res) => {
  const input = req.body.input;
  if (input) {
    res.send("Part 1: " + solvePart1(input) + "\n Part 2: " + solvePart2(input));
  }
  res.send('Day 8. No input given.');
});

var listener = app.listen(process.env.PORT || 5050, function() {
 console.log('listening on port ' + listener.address().port);
});
