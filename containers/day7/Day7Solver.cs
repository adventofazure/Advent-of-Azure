using System;
using System.Collections.Generic;

namespace day7
{
    class Bag
    {
        public string Color { get; set; }
        public int Amount { get; set;}
        public List<Bag> Bags {get; set;}
    }

    // Solves Advent of Code 2020 day 7
    public class Day7Solver
    {
        private static Bag parseBag(string bag)
        {
            var bags = new List<Bag>();
            var trimmedBag = bag.Replace(".", "");
            var parts = trimmedBag.Split("contain");
            var bagInformation = parts[0];
            var subInformation = parts[1];
            var bagParts = bagInformation.Split(" ");
            var color = bagParts[0] + " " + bagParts[1];
            if (!subInformation.Contains("no other bags")) {
                var subBags = subInformation.Split(",");
                foreach(var subBag in subBags)
                {
                    var subParts = subBag.Trim().Split(" ");
                    var amount = Int32.Parse(subParts[0]);
                    var subColor = subParts[1] + " " + subParts[2];
                    bags.Add(new Bag()
                    {
                        Color = subColor,
                        Amount = amount,
                        Bags = new List<Bag>()
                    });
                }
            }
            return new Bag()
            {
                Color = color,
                Amount = -1,
                Bags = bags
            };
        }

        private static bool canBagContain(string bagToFind, Bag bag, Dictionary<string, Bag> bagDict)
        {
            if (bag.Bags.Count == 0)
            {
                return false;
            }
            foreach(var subBag in bag.Bags)
            {
                if (subBag.Color == bagToFind)
                {
                    return true;
                }
                Bag value = null;
                if (bagDict.TryGetValue(subBag.Color, out value))
                {
                    if (canBagContain(bagToFind, value, bagDict))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static int SolvePart1(string input)
        {
            var bags = input.Split(";");
            var bagDict = new Dictionary<string, Bag>();
            foreach (var bag in bags)
            {
                var parsedBag  = parseBag(bag);
                bagDict.Add(parsedBag.Color, parsedBag);
            }

            var bagToFind = "shiny gold";
            var canContainAmount = 0;
            foreach (KeyValuePair<string, Bag> entry in bagDict)
            {
                if (canBagContain(bagToFind, entry.Value, bagDict))
                {
                    canContainAmount++;
                }
            }

            return canContainAmount;
        }

        private static int getBagAmount(string mainBagColor, Bag bag, Dictionary<string, Bag> bagDict)
        {
            var bagAmount = 0;
            foreach (var subBag in bag.Bags)
            {
                Bag value = null;
                if (bagDict.TryGetValue(subBag.Color, out value))
                {
                    bagAmount += subBag.Amount * getBagAmount(mainBagColor, value, bagDict);
                }
            }
            return bag.Color == mainBagColor ? bagAmount : bagAmount + 1;
        }

        public static int SolvePart2(string input)
        {
            var bags = input.Split(";");
            var bagDict = new Dictionary<string, Bag>();
            foreach (var bag in bags)
            {
                var parsedBag  = parseBag(bag);
                bagDict.Add(parsedBag.Color, parsedBag);
            }

            Bag shinyGoldBag = null;
            if (bagDict.TryGetValue("shiny gold", out shinyGoldBag))
            {
                return getBagAmount("shiny gold", shinyGoldBag, bagDict);
            }

            return 0;
        }        
    }
}
