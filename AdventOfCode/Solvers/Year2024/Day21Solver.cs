using AdventOfCode.Helpers.Enums;

namespace AdventOfCode.Solvers.Year2024;

internal class Day21Solver(long part1Test = 0, long part2Test = 0) : BaseSolver2024<long>(21, part1Test, part2Test)
{


    interface IKeyPad
    {
        (int row, int col) ArmPosition { get; }


        string ArmPositionKey { get; }
        long TestDirections(List<(Direction direction, int steps)> instructions, int timesToPressA = 0);
    }

    class Keypad
    {
        public (int row, int col) ArmPosition { get; set; }
        public IKeyPad ParentKeypad { get; set; }
        public long TotalInstructions = 0;

        private Dictionary<char, (int row, int col)> keycodeToPosition = new()
    {
        { '7' , (0,0)},
        { '8' , (0,1)},
        { '9' , (0,2)},
        { '4' , (1,0)},
        { '5' , (1,1)},
        { '6' , (1,2)},
        { '1' , (2,0)},
        { '2' , (2,1)},
        { '3' , (2,2)},
        { '0' , (3,1)},
        { 'A' , (3,2)},
    };
        public void EnterNumber(char character)
        {
            var targetPosition = keycodeToPosition[character];
            var path = PathToPosition(targetPosition);
            var pathCost = ParentKeypad.TestDirections(path, 1);


            //check if alternative is cheaper
            var alternativePath = path.Reverse<(Direction, int)>().ToList();

            var alternativePathValid = true;
            var coords = ArmPosition;
            foreach (var alternative in alternativePath)
            {
                for (int i = 0; i < alternative.Item2; ++i)
                {
                    coords = coords.Offset(alternative.Item1);
                    alternativePathValid &= coords != (3, 0);
                }
            }

            var alternativePathCost = alternativePathValid ? ParentKeypad.TestDirections(alternativePath, 1) : Int32.MaxValue;

            //the cost should already be the RESULT
            if (alternativePathValid && alternativePathCost < pathCost)
            {
                TotalInstructions = alternativePathCost;
            }
            else
            {
                TotalInstructions = pathCost;
            }
            ArmPosition = targetPosition;
        }

        public List<(Direction direction, int steps)> PathToPosition((int row, int col) targetPosition)
        {
            var result = new List<(Direction direction, int steps)>();

            if (ArmPosition.row > targetPosition.row)
            {
                result.Add((Direction.Up, ArmPosition.row - targetPosition.row));
            }
            if (ArmPosition.col < targetPosition.col)
            {
                result.Add((Direction.Right, targetPosition.col - ArmPosition.col));
            }

            if (ArmPosition.row < targetPosition.row)
            {
                result.Add((Direction.Down, targetPosition.row - ArmPosition.row));
            }
            if (ArmPosition.col > targetPosition.col)
            {
                result.Add((Direction.Left, ArmPosition.col - targetPosition.col));
            }

            return result;
        }
    }

    class DirectionalKeypad : IKeyPad
    {
        private Dictionary<Direction, (int row, int col)> directionToPosition = new()
        {
            { Direction.Up , (0,1)},
            { Direction.Left , (1,0)},
            { Direction.Down , (1,1)},
            { Direction.Right , (1,2)},
        };

        /// <summary>
        /// Links source and destination coordinates so we only have to count once.
        /// </summary>
        private Dictionary<(string armPositionKey, int destinationRow, int destinationCol, int steps), long> cache = new();

        public (int row, int col) ActivationPosition { get; set; }
        public (int row, int col) ArmPosition { get; set; }
        public IKeyPad ParentKeypad { get; set; }

        public int Variation { get; set; }

        public string ArmPositionKey => $"{ArmPosition.row};{ArmPosition.col};{ParentKeypad.ArmPositionKey}";

        public long TestDirections(List<(Direction direction, int steps)> instructions, int timesToPressA = 0)
        {
            var instructionCount = 0L;
            //instructions is wat we moeten DOORSTUREN naar het keypad onder ons
            // na instructions staat die arm op de juiste positie en moeten wij op A beginnen duwen
            foreach (var instruction in instructions)
            {
                //we go to position, then we press A
                var targetPosition = directionToPosition[instruction.direction];
                var cost = CheapestPastCost(targetPosition, instruction.steps);
                instructionCount += cost;
                ArmPosition = targetPosition;
            }



            //onderste arm staat nu juist, nu {additional} keren op A duwen 
            var pathHomeCost = CheapestPastCost(ActivationPosition, timesToPressA);
            //daarom eerst naar A gaan, en dan {timesToPressA} op a duwen (via parent)
            instructionCount += pathHomeCost;
            ArmPosition = ActivationPosition;

            return instructionCount;
        }

        private long CheapestPastCost((int row, int col) targetPosition, int timesToPressButton)
        {
            var cacheKey = (ArmPositionKey, targetPosition.row, targetPosition.col, timesToPressButton);

            if (cache.ContainsKey(cacheKey))
            {
                return cache[cacheKey];
            }

            //path om naar het pijltje te gaan
            var path = PathToPosition(targetPosition);
            var pathCost = ParentKeypad.TestDirections(path, timesToPressButton);

            //check if alternative is cheaper
            var alternativePath = path.Reverse<(Direction, int)>().ToList();
            var alternativePathValid = true;
            var coords = ArmPosition;
            foreach (var alternative in alternativePath)
            {
                for (int i = 0; i < alternative.Item2 && alternativePathValid; ++i)
                {
                    coords = coords.Offset(alternative.Item1);
                    alternativePathValid &= coords != (0, 0);
                }
            }
            var alternativePathCost = alternativePathValid ? ParentKeypad.TestDirections(alternativePath, timesToPressButton) : Int32.MaxValue;

            if (alternativePathValid && alternativePathCost < pathCost)
            {
                cache[cacheKey] = alternativePathCost;
                return alternativePathCost;
            }
            else
            {
                cache[cacheKey] = pathCost;
                return pathCost;
            }
        }

        public List<(Direction direction, int steps)> PathToPosition((int row, int col) targetPosition)
        {
            var result = new List<(Direction direction, int steps)>();

            if (ArmPosition.row < targetPosition.row)
            {
                result.Add((Direction.Down, targetPosition.row - ArmPosition.row));
            }
            if (ArmPosition.col < targetPosition.col)
            {
                result.Add((Direction.Right, targetPosition.col - ArmPosition.col));
            }
            if (ArmPosition.row > targetPosition.row)
            {
                result.Add((Direction.Up, ArmPosition.row - targetPosition.row));
            }
            if (ArmPosition.col > targetPosition.col)
            {
                result.Add((Direction.Left, ArmPosition.col - targetPosition.col));
            }



            return result;
        }
    }

    class ManualKeyPad : IKeyPad
    {
        public (int row, int col) ArmPosition => (0, 0);
        public string ArmPositionKey => $"{ArmPosition.row};{ArmPosition.col}";

        public long TestDirections(List<(Direction direction, int steps)> instructions, int timesToPressA = 0)
        {
            return instructions.Sum(x => x.steps) + timesToPressA;
        }
    }

    public override long Part1(string[] input)
    {
        var ownKeypad = new ManualKeyPad() { };
        var result = 0L;

        var keyPadList = new List<IKeyPad>(){
            new DirectionalKeypad() {
                ArmPosition = (0, 2),
                ActivationPosition = (0, 2),
                ParentKeypad = ownKeypad }
        };

        for (int i = 0; i < 1; ++i)
        {
            keyPadList.Add(new DirectionalKeypad()
            {
                ArmPosition = (0, 2),
                ActivationPosition = (0, 2),
                ParentKeypad = keyPadList[i]
            });
        }

        var mainKeypad = new Keypad() { ArmPosition = (3, 2), ParentKeypad = keyPadList.Last() };

        foreach (var line in input)
        {
            var instructionCount = 0L;
            foreach (var keyCode in line)
            {
                mainKeypad.EnterNumber(keyCode);
                instructionCount += mainKeypad.TotalInstructions;
            }

            var numeric = line[0..3].ToInt();

            result += instructionCount * numeric;
        }


        return result;
    }

    public override long Part2(string[] input)
    {
        var ownKeypad = new ManualKeyPad() { };
        var result = 0L;
        var keyPadList = new List<IKeyPad>(){
            new DirectionalKeypad() {
                ArmPosition = (0, 2),
                ActivationPosition = (0, 2),
                ParentKeypad = ownKeypad }
        };

        for (int i = 0; i < 24; ++i)
        {
            keyPadList.Add(new DirectionalKeypad()
            {
                ArmPosition = (0, 2),
                ActivationPosition = (0, 2),
                ParentKeypad = keyPadList[i]
            });
        }


        var mainKeypad = new Keypad() { ArmPosition = (3, 2), ParentKeypad = keyPadList.Last() };

        foreach (var line in input)
        {
            var instructionCount = 0L;
            foreach (var keyCode in line)
            {
                mainKeypad.EnterNumber(keyCode);
                instructionCount += mainKeypad.TotalInstructions;
            }

            var numeric = line[0..3].ToInt();
            Console.WriteLine($"{line}: {instructionCount}");
            result += instructionCount * numeric;
        }


        return result;
    }
}
