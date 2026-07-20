---
name: tdd
description: This skill should be used when the user asks to implement a usecase/feature with TDD, says "let's TDD this", "start TDD", "add a new usecase test-first", or otherwise wants a feature built one failing test at a time following this repo's RED/GREEN commit convention. Triggers on "/tdd".
version: 1.0.0
---

# TDD Workflow

Drives feature/usecase work through this repo's strict test-first process: gather intent, agree on test scenario names, checklist them, then implement one scenario at a time — test fails first (RED), gets approved, then made to pass (GREEN).

Follow the four phases below in order. Do not skip ahead — each phase gates the next.

## Phase 1 — Intake

Ask the user, in one message:

1. Is this a **new** usecase/feature, or are we adding scenarios to an **existing** one?
2. (If new) What is the **name** of the usecase? Use this exact name for the interface/class/test-class following the repo's convention (e.g. `CreateUfcEvents` → `ICreateUfcEvents`/`CreateUfcEvents` with a single `ExecuteAsync` method, test class `CreateUfcEventsTest`).
3. What is the **responsibility** of this feature — what should it actually do, and what's in/out of scope?
4. (If new) Where does it live — which feature folder, e.g. `src/UFC.Events.Manager.API/Features/UFCEvents/<UseCaseName>/`?
5. What are the **business scenarios** this feature needs to handle? Ask the user to type these out themselves, in plain language (the happy path plus any edge cases/validation/error cases they care about) — don't propose these for them.

Wait for answers before proceeding. If "existing usecase" is chosen, skip questions 2 and 4 and instead locate the existing interactor, interface, and test file yourself.

## Phase 2 — Scenario naming

Translate the business scenarios from question 5 into test scenario names using this repo's convention: `GivenX_WhenY_ThenZ` (see existing tests under `tests/UFC.Events.Manager.API.Tests/Features/`). Each business scenario should map to one test name; don't invent additional scenarios beyond what the user described.

Present the list as a numbered markdown list, nothing else yet — no test code, no implementation. The user will edit/reorder/add/remove names and give feedback. Revise and re-present until they explicitly confirm the list is final. Do not proceed to Phase 3 on an implicit "looks fine" — get an explicit go-ahead.

## Phase 3 — Checklist

Once the scenario list is confirmed, create one task per scenario (use `TaskCreate`), in the agreed order, all pending. This is the shared source of truth for progress — don't re-derive or silently reorder it afterward.

## Phase 4 — One scenario at a time

For each scenario, in checklist order:

1. Mark the task `in_progress`.
2. Write **only the test** for that scenario — following the repo's `// Arrange` / `// Act` / `// Assert` structure and Moq-based dependency mocking (see `CreateUfcEventsTest.cs` / `GetUfcEventsTest.cs` for the pattern). Do not write or modify production code in this step. If the usecase doesn't exist yet, the test may fail to compile — that counts as RED.
3. Run it (`dotnet test --filter "Name=<ScenarioName>"`) and confirm it fails.
4. Show the user the failing test and stop. Wait for explicit approval before writing any implementation — this is the checkpoint where they confirm the assertions actually capture the intended behavior.
5. On approval, write the minimal production code to make the test pass (GREEN) — no extra scenarios, no gold-plating beyond this one test.
6. Run the full test suite (not just the new test) to confirm it passes and nothing else broke.
7. Mark the task `completed`. If the user wants to commit, follow the repo's paired-commit convention: a RED commit for the failing test, a GREEN commit for the implementation (see `git log` for examples like `GivenEvents_WhenCreating_ThenTheEventsShouldBePersisted (RED)` / `(GREEN)`). Only commit when asked — see repo-wide git safety rules.
8. Move to the next pending task and repeat from step 1.

Stop when the checklist is empty. Don't batch multiple scenarios' tests together, and don't jump to implementation before a RED test has been shown and approved.
