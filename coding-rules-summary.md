# Coding Rules Summary

## Rules Loaded from `.mdc` Files

This document summarizes the rules from the `rules/` folder that should be applied during development.

---

## 1. Absolute Priority Premise (APP)
**Source:** `rules/absolute-priority-premise.mdc`  
**Apply:** Always

### Purpose
Measure code quality objectively by calculating "mass" values for code components. Lower mass indicates better, simpler code.

### Mass Values
- **Constant (1)**: Literal values (`5`, `"hello"`, `true`, `[]`)
- **Binding/Scalar (1)**: Variables, parameters, local names
- **Invocation (2)**: Function/method calls
- **Conditional (4)**: Control flow decisions (`if`, `switch`, `?:`)
- **Loop (5)**: Iteration constructs (`while`, `for`, `forEach`, `map`)
- **Assignment (6)**: Mutating variables (highest mass)

### Calculation
```
Total Mass = (constants × 1) + (bindings × 1) + (invocations × 2) +
             (conditionals × 4) + (loops × 5) + (assignments × 6)
```

### Key Principles
- Lower mass = Better code
- Functional style naturally scores lower
- Immutable approaches preferred
- Simple expressions preferred over complex control structures
- Use during refactor phase to compare code quality

---

## 2. Human-in-the-Loop TDD Rules
**Source:** `rules/human-in-the-loop.mdc`  
**Apply:** During TDD process

### Rule 1: End-of-Phase Confirmation
**Pause at the end of EVERY TDD phase** and ask for permission to continue.

#### After Red Phase
- Summarize: Which test was activated, prediction made, failure type
- Ask: "Red phase complete. Should I proceed to Green phase?"

#### After Green Phase
- Summarize: Implementation approach, confirmation tests pass, trade-offs
- Ask: "Green phase complete. Should I proceed to Refactor phase?"

#### After Refactor Phase
- Summarize: Refactorings attempted/completed, naming changes, mass calculations
- Ask: "Refactor phase complete. Should I proceed to the next test?"

### Rule 2: Failed Prediction Recovery
When predictions fail:
1. Stop immediately
2. Explain the prediction failure
3. Assess implications
4. Ask for guidance: "My prediction was incorrect. Should I continue or investigate further?"

### Why This Matters
- Human maintains full control
- Educational opportunity for guidance
- Prevents over-implementation
- Quality assurance at every step

---

## 3. Rules of Simple Design
**Source:** `rules/simple-design.mdc`  
**Apply:** Always (especially during Refactor phase)

### The Four Rules (Priority Order)

1. **Tests Pass** - All tests must pass (highest priority)
2. **Reveals Intent** - Code clearly expresses what it does
3. **No Duplication (DRY)** - Don't repeat yourself
4. **Fewest Elements** - Minimize classes, methods, and code elements

### Application Guidelines
- Apply rules in order: 1 → 2 → 3 → 4
- Never violate higher-priority rule for lower-priority one
- If rule #3 conflicts with rule #2, choose clarity over DRY

### Integration with TDD
- **Red Phase**: Focus on rule #1
- **Green Phase**: Focus on rule #1 (minimal working code)
- **Refactor Phase**: Apply rules #2, #3, #4 while preserving #1

---

## 4. Test-Driven Development (TDD) Rules
**Source:** `rules/tdd.mdc`  
**Apply:** Always

### TDD Mindset
**Expected discomfort is a sign you're doing it right:**
- Hardcoded returns feel "too simple" - but they're correct minimal steps
- Urge to implement ahead is strong - resist this
- Minimal steps feel inefficient - but actually accelerate development
- Push through this discomfort - it indicates correct discipline

### Core TDD Process

#### 1. Test List First
- Create test list using `it.todo()` for **BASE FUNCTIONALITY ONLY**
- Not advanced features
- Understand scope of core feature

#### 2. One Test at a Time
- Convert exactly **ONE** `it.todo()` to executable test
- All others remain as `it.todo()`
- Never have more than one failing test
- Don't think ahead or implement for future tests

#### 3. Red-Green-Refactor Cycle

**Red Phase (Compilation Error)**
- Start with non-existent function
- Test fails with compilation error
- Ensures starting from scratch

**Red Phase (Runtime Error)**
- Create empty function returning undefined/wrong value
- Test fails with assertion error
- Verifies test works as expected

**Green Phase**
- Implement minimal code to pass test
- Don't add features for future tests
- Don't optimize or refactor yet

**Refactor Phase**
- **MUST attempt at least one refactoring**
- If no improvement possible, document why
- **Naming Evaluation (First Priority)**:
  - Does name clearly describe function based on all tests so far?
  - Has purpose become clearer through latest test?
  - Rename if name doesn't capture current full intent
- Apply ATP to measure improvements (calculate mass)
- Apply 4 Rules of Simple Design
- Document why no refactoring if none improves code

### Common TDD Failure Modes (Avoid These!)
- Planning beyond base functionality
- Multiple active tests
- Implementing beyond tests
- Skipping predictions
- Avoiding refactoring
- Premature abstraction
- Ignoring the uncomfortable

---

## 5. TypeScript Development Rules
**Source:** `rules/typescript-development.mdc`  
**Apply:** For TypeScript projects

### Module Import Rules
**Use explicit `.js` file extensions for ESNext modules:**

```typescript
// ✅ Correct
import { myFunction } from "./my-module.js";

// ❌ Incorrect
import { myFunction } from "./my-module";
```

**Why:** ESNext modules require explicit extensions; TypeScript compiles `.ts` → `.js`

### Testing Framework
**Use Vitest as the standard testing framework:**

```typescript
// ✅ Correct
import { describe, it, expect } from "vitest";

// ❌ Incorrect
import { describe, it, expect } from "jest";
```

### File Naming Conventions
**Test files use `.spec.ts` extension:**

```
✅ Correct: calculator.spec.ts
❌ Incorrect: calculator.test.ts
```

**Why:** Tests are specifications - they document expected behavior

---

## Application Priority

### Always Applied
1. TDD Rules (highest priority - guides entire process)
2. Rules of Simple Design (during refactoring)
3. Absolute Priority Premise (during refactoring for measurement)

### Context-Dependent
4. Human-in-the-Loop TDD (during TDD sessions)
5. TypeScript Development Rules (for TypeScript projects)

---

## Integration Strategy

These rules work together synergistically:

1. **TDD Rules** provide the overall process framework
2. **Human-in-the-Loop** ensures engagement at every step
3. **Rules of Simple Design** guide refactoring decisions
4. **Absolute Priority Premise** provides objective measurements
5. **TypeScript Rules** ensure proper technical implementation

**Key Principle:** When rules conflict, follow the priority order above. Always favor working tests and human control over automation.
