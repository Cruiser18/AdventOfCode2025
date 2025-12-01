# Advent of Code 2025 - TDD Workflow Instructions

## Project Structure Pattern

Each day follows this structure:
```
DayXX/
  ├── input.txt              # Real puzzle input
  ├── testinput.txt          # Test input for unit tests
  ├── Solution1/             # Part 1 console application
  │   ├── Solution1.csproj
  │   ├── Program.cs         # Entry point, reads input.txt
  │   ├── [PuzzleClass].cs   # Main puzzle logic
  │   └── [PuzzleClass]Tests.cs  # xUnit tests
  └── Solution2/             # Part 2 console application
      ├── Solution2.csproj
      ├── Program.cs         # Entry point, reads input.txt
      ├── [PuzzleClass].cs   # Main puzzle logic
      └── [PuzzleClass]Tests.cs  # xUnit tests
```

## Setup Workflow for Each Day

### 1. Create Project Structure
```powershell
# Create day folder
New-Item -ItemType Directory -Path "DayXX"
cd DayXX

# Create input files
New-Item -ItemType File -Path "input.txt"
New-Item -ItemType File -Path "testinput.txt"

# Create Solution1 console application
dotnet new console -n Solution1
cd Solution1

# Add xUnit testing packages
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk

cd ..

# Create Solution2 console application
dotnet new console -n Solution2
cd Solution2

# Add xUnit testing packages
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
```

### 2. Test-Driven Development Cycle

**For each part (Solution1 and Solution2):**

1. **Write Tests First** - Create `[PuzzleClass]Tests.cs`:
   - Use `testinput.txt` for test data
   - Cover all requirements from puzzle description
   - Include edge cases (empty input, single item, boundaries, etc.)
   - Use descriptive test method names (e.g., `Dial_StartsAt50`, `Rotate_Right_WrapsAround`)
   - Use `[Fact]` for individual tests, `[Theory]` for parameterized tests

2. **Implement Logic** - Create `[PuzzleClass].cs`:
   - Keep business logic separate from I/O
   - Make classes testable (public methods, observable state)
   - Consider performance for large inputs (optimize algorithms)
   - Use clear, descriptive naming

3. **Update Program.cs**:
   - Read from `input.txt` (use relative path: `../input.txt`)
   - Parse input appropriately
   - Instantiate puzzle class and process input
   - Output the answer clearly

4. **Run Tests**:
   ```powershell
   dotnet test
   ```
   - All tests must pass before running on real input
   - Fix any failures before proceeding

5. **Run on Real Input**:
   ```powershell
   dotnet run
   ```
   - Verify answer makes sense
   - Submit to Advent of Code

## Key Patterns from Day 1

### Circular Array/Modulo Arithmetic
- For right rotation: `(position + distance) % size`
- For left rotation: `(position - distance + size * buffer) % size`
  - Buffer ensures result stays positive: `size * ((distance / size) + 1)`

### Counting Events During Process vs At End
- **Part 1 Pattern**: Count state after each operation
  ```csharp
  public void Process(params)
  {
      // Do operation
      // Check condition AFTER operation
      if (condition) count++;
  }
  ```

- **Part 2 Pattern**: Count events during operation
  ```csharp
  public void Process(params)
  {
      // Calculate how many times event occurs DURING operation
      count += CalculateEventsDuring(params);
      // Update state
  }
  ```

### Optimization Techniques
- When counting crossings/passages through a point in circular movement:
  - Mathematical calculation is faster than simulation
  - For crossing point 0 during rotation from A to B by distance D:
    - Right: crosses = floor((A + D) / size) - floor(A / size)
    - Left: crosses = floor(A / size) - floor((A - D) / size)
  - Handles wrap-around automatically

### Input Parsing
- Use static helper methods for parsing:
  ```csharp
  public static (char direction, int distance) ParseInstruction(string instruction)
  {
      char direction = instruction[0];
      int distance = int.Parse(instruction.Substring(1));
      return (direction, distance);
  }
  ```

### Test File Organization
- Keep tests in same namespace as implementation
- Use `File.ReadAllLines()` for testinput.txt
- Use descriptive test names that explain what's being tested
- Group related tests with comments

### Program.cs Pattern
```csharp
var lines = File.ReadAllLines("../input.txt");
var puzzle = new PuzzleClass();

foreach (var line in lines)
{
    var (param1, param2) = PuzzleClass.ParseInstruction(line);
    puzzle.ProcessInstruction(param1, param2);
}

Console.WriteLine($"Answer: {puzzle.Result}");
```

## Testing Best Practices

1. **Test Independence**: Each test should be independent
2. **Arrange-Act-Assert**: Clear structure in each test
3. **Edge Cases**: Test boundaries, empty inputs, single items, wraps
4. **Test Data**: Use `testinput.txt` for consistent test data
5. **Verify Expected Behavior**: Test against puzzle examples
6. **Performance**: If optimization changes behavior, verify with tests first

## Performance Considerations

- For large iteration counts (>1000), prefer mathematical solutions over simulation
- Use modulo arithmetic to avoid large number calculations
- Consider memory usage for large state tracking

## Validation Checklist

Before submitting answers:
- [ ] All unit tests pass (`dotnet test`)
- [ ] Tests cover all puzzle requirements
- [ ] Tests cover edge cases
- [ ] Code runs successfully on test input
- [ ] Code runs successfully on real input (`dotnet run`)
- [ ] Answer is reasonable given puzzle constraints
- [ ] Code is clean and well-organized

## Day 1 Specific Lessons

### Safe Dial Problem
- **Part 1**: Count when dial lands exactly on 0 after rotation
- **Part 2**: Count every time dial passes through 0 during rotation
- Starting position: 50
- Dial range: 0-99 (100 positions)
- Instructions: R/L followed by number (e.g., "R23", "L17")

### Key Insight
The difference between "lands on" vs "passes through" is critical:
- "Lands on" = check position AFTER movement
- "Passes through" = count crossings DURING movement

This pattern likely applies to other AOC puzzles where the question distinguishes between final state vs intermediate states.

## Tools Used
- `dotnet new console` - Create console applications
- `dotnet add package` - Add NuGet packages
- `dotnet test` - Run unit tests
- `dotnet run` - Execute application
- xUnit framework for testing
