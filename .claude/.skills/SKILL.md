---
title: Transcript Claim-Check
date: "2026-07-01"
tags:
  - type/reference
  - system/agent-config
status: active
name: transcript-claim-check
global: true
model: sonnet
description: "Verify a summary/meeting-note synthesized from a transcript against its source: a Sonnet 5 subagent proves each factual claim (SUPPORTED/OVERSTATED/CONTRADICTED/UNSUPPORTED), catching contradiction- and fabrication-class errors before it ships. Run after any transcript→summary synthesis. Triggers: 'claim-check', 'verify the summary', 'prove the claims'."
---

# Transcript Claim-Check

A mandatory **verification pass** that runs AFTER a summary or meeting note is synthesized from a transcript and BEFORE it ships, updates project state, or seeds an outbound draft. It proves every factual claim in the summary against the source transcript.

This is the transcript counterpart to `humanizer`: humanizer is a finishing pass for **style**, this is a finishing pass for **facts**. It exists because ASR-transcript synthesis silently produces contradiction-class errors — a "welcome/optional" rendered as "required", a "he declined the reschedule" rendered as "propose a new date". Style passes never catch those; only re-checking each claim against the source does.

## When to run

- After any ASR / AuraFlow transcript → cleaned meeting note or summary (the `project-file-workflow` meeting-note path, and any ad-hoc transcript summary).
- After you lift claims from a transcript into project state (`CURRENT-STATE.md` / `NEXT-STEPS.md`), an outbound email/brief, or a memory.
- Guardrail: [[feedback_transcript-claim-check-canonical]] — never ship a transcript-derived summary/brief/state-update without this pass. Order: synthesize → **claim-check (fix)** → humanizer → ship.

## Inputs

- The generated summary / note (path or in-context text).
- The **full** source transcript file(s) — every part (multi-part AuraFlow recordings split into `-1`, `-2`; include all).

## Procedure

1. Collect the summary + ALL source transcript files. Confirm you have the complete transcript, not an excerpt.
2. Spawn a **Sonnet 5 verifier** subagent in an isolated context: `Agent` tool with `subagent_type: general-purpose`, `model: sonnet`. Pass it the full transcript text, the full summary text, and the **Verifier prompt** below. Isolated context is the point — the verifier reads the raw transcript fresh, uncolored by whatever the synthesis decided.
3. The verifier extracts every atomic factual claim and returns the verdict table (format below). It judges and quotes only; it never rewrites.
4. Act on the verdicts, in priority order:
   - **CONTRADICTED** → fix immediately. The source says otherwise. This is the class that bites.
   - **UNSUPPORTED** → remove or correct. Not in the transcript = likely fabricated.
   - **OVERSTATED** → soften to the source's actual hedge/scope.
   - **UNVERIFIABLE** → added context/inference or the transcript is ambiguous. Keep only if you can independently source it; otherwise cut it or mark it explicitly as inference. Never let it read as a transcript fact.
   - **SUPPORTED** → leave.
5. Fix every non-SUPPORTED claim in the summary **and everywhere it already propagated** (project state, drafts, memory leaves, Hermes bridge notes). Re-run the pass if you made substantive edits.
6. Report the roll-up (claims / supported / overstated / contradicted / unsupported / unverifiable) so the fix is auditable.

## Claim taxonomy — the verifier checks ALL of these

- **Numbers & money** — amounts, %, counts, hours, ranges.
- **Dates & deadlines** — who owes what, by when.
- **Names & entities** — people, orgs, products, programs (ASR mangles proper nouns).
- **Requirement / modality** — "must / required / mandatory" vs "welcome / optional / nice-to-have". ⚠ high-risk: "mile widziane" → "wymóg" is a contradiction.
- **Negation / polarity** — "is / is not", "included / excluded". ASR fragments ("Nie, nie…") flip meaning.
- **Status / closure** — "open / pending / to-do" vs "declined / closed / done". ⚠ high-risk: "propose a date" when the source closed the topic.
- **Causal / conditional** — "X because Y", "if X then Y loses validity".
- **Quotes / attribution** — who said what.

## Verifier prompt (paste verbatim to the Sonnet 5 subagent, with the two texts)

> You are a fact-verification auditor. You are given (A) a raw meeting/interview TRANSCRIPT (ASR output — possibly garbled, possibly multi-part) and (B) a SUMMARY synthesized from it. Your only job is to prove or disprove every factual claim in the SUMMARY against the TRANSCRIPT. You do not rewrite, improve, soften, or add anything.
>
> Method:
> 1. Read the full TRANSCRIPT first and form your own understanding BEFORE reading the SUMMARY. Read for meaning: ASR mangles proper nouns and sometimes flips polarity with fragments ("Nie, nie…").
> 2. Decompose the SUMMARY into atomic factual claims (one assertion each), covering: numbers/money, dates/deadlines, names/entities, requirement-modality (must vs optional/welcome), negation/polarity, status/closure (open vs declined/done), causal/conditional, quotes/attribution.
> 3. For EACH claim assign exactly one verdict:
>    - SUPPORTED — transcript clearly states it. Quote the line(s).
>    - OVERSTATED — transcript supports only a weaker/hedged version. Quote the line + state the real strength.
>    - CONTRADICTED — transcript states the opposite or something incompatible. Quote the line.
>    - UNSUPPORTED — not in the transcript at all.
>    - UNVERIFIABLE — context/inference not sourced from the transcript, OR the transcript is genuinely ambiguous. Say which.
> 4. Be adversarial: the burden is on the summary. If you cannot point to a transcript line, it is NOT supported. Give extra scrutiny to requirement-modality, negation/polarity, and status/closure claims — they flip meaning silently.
>
> Output ONLY a markdown table with columns: `# | Claim | Verdict | Transcript evidence (quoted line, or NOT FOUND) | Note`. Then one final line: `ROLL-UP: <n> claims — <supported>/<overstated>/<contradicted>/<unsupported>/<unverifiable>; VERDICT: PASS (all supported) | FIX (list the #s)`.

## Rules

- **Never** ship, send, commit, or update state from a transcript-derived summary that still has an unresolved CONTRADICTED or UNSUPPORTED claim.
- ASR transcripts are garbled — match on **meaning**, not exact string; quote the closest supporting line.
- If the transcript is genuinely ambiguous on a claim, it is UNVERIFIABLE — surface it, do **not** resolve it in the summary's favor.
- The verifier composes and fixes nothing; it only judges and quotes. Fixes stay with the orchestrator (the context holder), same division of labour as the changelog/gotcha clerks.
- Verifier model is **Sonnet 5** (`model: sonnet`) — cheap enough to run every time, adversarial by instruction.
