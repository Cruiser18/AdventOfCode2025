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

## Day 2 Lessons Learned

### Problem: Finding Invalid IDs (Repeated Patterns)
- **Part 1**: IDs are invalid if pattern is repeated exactly twice (11, 1212, 123123)
- **Part 2**: IDs are invalid if pattern is repeated at least twice (includes 123123123, 1111111, etc.)

### Key Insights

#### 1. Pattern Detection Algorithm
For checking if a number is made of repeated patterns:
```csharp
// Try all possible pattern lengths from 1 to length/2
for (int patternLength = 1; patternLength <= length / 2; patternLength++)
{
    if (length % patternLength == 0) // Must divide evenly
    {
        string pattern = idStr.Substring(0, patternLength);
        // Check if entire string is this pattern repeated
    }
}
```

**Why this works:**
- Any repeated pattern must divide the total length evenly
- Only need to check patterns up to half the length (minimum 2 repetitions)
- Check divisors first to avoid unnecessary string comparisons

#### 2. Code Reuse Between Parts
When Part 2 is a generalization of Part 1:
- Keep infrastructure methods identical (ParseRange, FindInvalidIdsInRange, SumInvalidIds)
- Only modify core logic (IsInvalidId)
- Copy tests structure but adapt expected results

#### 3. LINQ for Functional Clarity
Prefer LINQ for complex multi-step operations:
```csharp
public static long SumInvalidIds(string input)
{
    return input.Split(',')
        .SelectMany(range =>
        {
            var (start, end) = ParseRange(range.Trim());
            return FindInvalidIdsInRange(start, end);
        })
        .Sum();
}
```

**Benefits:**
- Eliminates mutable state (no sum variable, no nested loops)
- Lower ATP mass (40 → 16, 60% reduction)
- More declarative and easier to understand

#### 4. File Path Handling in Program.cs
```csharp
// Use absolute paths to avoid working directory issues
string inputPath = @"c:\mjolner-code\AdventOfCode2025\Day02\input.txt";
string input = File.ReadAllText(inputPath).Replace("\n", "").Replace("\r", "");
```

**Why:**
- `dotnet run` working directory can vary
- Relative paths (`../input.txt`) often fail
- Absolute paths ensure reliability

#### 5. Efficient Range Processing
For large ranges (1188511880-1188511890):
- Use `long` instead of `int` for ID storage
- Still iterate sequentially (no clever math optimization needed)
- Pattern checking is O(n) per number but n is small (< 10 digits)

#### 6. Test Strategy for Pattern Variations
Test various repetition counts:
- 2 times: 11, 1212, 123123
- 3 times: 123123123
- 5 times: 1212121212  
- 7 times: 1111111
- Non-repeated: 123

This ensures algorithm works for any repetition count ≥ 2.

### Edge Cases Discovered
- Single digit numbers (1-9) cannot be invalid (need at least 2 repetitions)
- Numbers like 111 (3 times) vs 1111 (4 times) - both invalid in Part 2
- Large numbers still process efficiently (string operations on ~10 digits)

## Completed Puzzles

### Day 1
- **Part 1 Answer**: 1007
- **Part 2 Answer**: 5820
- **Key Classes**: `SafeDial`, `SafeDialAdvanced`
- **Tests**: 19 tests each, all passing
- **Special Notes**: Part 2 required mathematical optimization for zero crossing detection

### Day 2
- **Part 1 Answer**: 28846518423
- **Part 2 Answer**: 31578210022
- **Key Classes**: `InvalidIdFinder`, `InvalidIdFinderAdvanced`
- **Tests**: 9 tests (Part 1), 6 tests (Part 2), all passing
- **Special Notes**: 
  - Part 1: IDs repeated exactly twice
  - Part 2: IDs repeated at least twice (generalization)
  - Used LINQ for cleaner functional approach in SumInvalidIds
  - Absolute file paths needed for input reading in Program.cs

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
