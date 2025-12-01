# Advent of Code 2025 - Context & Instructions

## Project Overview
This workspace contains solutions for Advent of Code 2025 puzzles, implemented using Test-Driven Development (TDD) in C#.

## Project Structure
```
AdventOfCode2025/
├── Day01/
│   ├── input.txt              # Real puzzle input
│   ├── testinput.txt          # Test input for unit tests
│   ├── Solution1/             # Part 1 console app
│   │   ├── Solution1.csproj
│   │   ├── Program.cs
│   │   ├── [PuzzleLogic].cs   # Main logic class
│   │   └── [PuzzleLogic]Tests.cs  # xUnit tests
│   └── Solution2/             # Part 2 console app
│       ├── Solution2.csproj
│       ├── Program.cs
│       ├── [PuzzleLogic].cs
│       └── [PuzzleLogic]Tests.cs
├── Day02/
│   └── ... (same structure)
└── AOC_2025_CONTEXT.md        # This file
```

## Workflow for Each New Day

### 1. Setup Phase
```powershell
# Create day folder and input files
mkdir DayXX
cd DayXX
New-Item input.txt
New-Item testinput.txt

# Create Solution1 project
dotnet new console -n Solution1
cd Solution1
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk

# Create Solution2 project
cd ..
dotnet new console -n Solution2
cd Solution2
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
```

### 2. TDD Development Process
1. **Read and understand the puzzle** - Break down requirements into testable components
2. **Write test cases first** - Cover all scenarios including edge cases
3. **Implement minimal code** to make tests pass
4. **Refactor** for clarity and performance
5. **Run tests frequently** - Use `dotnet test` after each change
6. **Validate with test input** before running on real input
7. **Run on real input** only when all tests pass

### 3. Test Writing Guidelines
- Start with simple, obvious test cases
- Add edge cases (zero, negative, boundary conditions)
- Test state changes and side effects
- Use descriptive test method names (e.g., `RotateRight_IncrementsPosition`)
- Include tests for parsing/input processing
- Test wraparound behavior for circular problems
- Test accumulation/counting logic separately

### 4. Program.cs Pattern
```csharp
using Solution1;

string[] instructions = File.ReadAllLines("../input.txt");
var puzzleObject = new PuzzleClass();

foreach (var instruction in instructions)
{
    var (param1, param2) = PuzzleClass.ParseInstruction(instruction);
    puzzleObject.ProcessInstruction(param1, param2);
}

Console.WriteLine($"Answer: {puzzleObject.Result}");
```

## Day 1 Lessons Learned

### Problem: Safe Dial Simulation
- **Part 1**: Count times dial lands on 0 after completing each rotation
- **Part 2**: Count EVERY crossing of 0 during rotation (including mid-rotation)

### Key Insights

#### 1. Modular Arithmetic for Circular Logic
```csharp
// Right rotation
CurrentPosition = (CurrentPosition + distance) % 100;

// Left rotation (handle negative numbers)
CurrentPosition = (CurrentPosition - distance + 100 * ((distance / 100) + 1)) % 100;
```

#### 2. Mathematical Optimization for Zero Crossings
For Part 2, instead of simulating every single step:
```csharp
// Calculate how many times we cross 0 during rotation
int crosses = 0;

if (direction == 'R')
{
    int endPos = (startPos + distance) % 100;
    int fullRotations = distance / 100;
    crosses = fullRotations;
    
    // Check if we cross 0 in the partial rotation
    if (startPos > endPos || (startPos < endPos && endPos >= 100))
        crosses++;
}
```

#### 3. Off-by-One Errors in Crossing Detection
- Initially missed counting the crossing when landing exactly on 0
- Fixed by checking if `endPos == 0` and incrementing crosses

#### 4. Edge Cases to Test
- Starting at 0
- Landing on 0
- Crossing 0 during rotation
- Multiple full rotations
- Large distances (1000+)
- Alternating L/R directions
- Zero distance (no movement)

#### 5. Test Data Management
- Use `testinput.txt` for unit test data
- Use `input.txt` for final puzzle submission
- Keep test input small and verifiable by hand

### Performance Considerations
- For large distances, avoid step-by-step simulation
- Use mathematical formulas to calculate crossings directly
- Part 1 can be simple; Part 2 often requires optimization

## Completed Puzzles

### Day 1
- **Part 1 Answer**: 1007
- **Part 2 Answer**: 5820
- **Key Classes**: `SafeDial`, `SafeDialAdvanced`
- **Tests**: 19 tests each, all passing
- **Special Notes**: Part 2 required mathematical optimization for zero crossing detection

## Common Patterns

### Input Parsing
```csharp
public static (char direction, int value) ParseInstruction(string instruction)
{
    char direction = instruction[0];
    int value = int.Parse(instruction.Substring(1));
    return (direction, value);
}
```

### State Management
- Use properties with private setters for controlled state
- Maintain counters/accumulators as public readonly properties
- Reset state in constructor

### Testing xUnit Pattern
```csharp
[Fact]
public void TestName_WhenCondition_ExpectedBehavior()
{
    // Arrange
    var obj = new PuzzleClass();
    
    // Act
    obj.DoSomething();
    
    // Assert
    Assert.Equal(expectedValue, obj.ActualValue);
}
```

## Troubleshooting

### Tests Fail After Changes
1. Read the test failure message carefully
2. Check your mathematical logic (especially modular arithmetic)
3. Verify edge case handling
4. Add debug output if needed
5. Test with simple, hand-calculated examples

### Wrong Answer on Real Input
1. Verify all tests pass on test input
2. Check if Part 2 has different rules than Part 1
3. Look for optimization assumptions that may not hold
4. Consider if large numbers cause integer overflow
5. Re-read puzzle description for subtle requirements

### Performance Issues
1. Profile with large test cases
2. Replace loops with mathematical formulas where possible
3. Consider algorithmic complexity (O(n) vs O(1))
4. Avoid unnecessary string operations

## Tips for Success
1. **Read carefully** - Puzzle wording is precise and intentional
2. **Test first** - Write tests before implementation
3. **Start simple** - Get basic cases working before optimization
4. **Validate assumptions** - Test edge cases thoroughly
5. **Document special logic** - Add comments for non-obvious calculations
6. **Keep test input small** - Easy to verify by hand
7. **Don't rush to real input** - Validate thoroughly with tests first

## Next Steps
- Wait for Day 2 puzzle release
- Apply same TDD workflow
- Build on patterns established in Day 1
- Update this file with new learnings

---
*Last Updated: December 1, 2025 - Day 1 Complete*
