# Windows shutdown timer rebuild

2026-10-31

- [x] Inspect the timer and identify the Aurora and AuraFlow design references.
- [x] Rebuild the timer and its main window. Keep the Windows shutdown action safe to test.
- [x] Build the app and test the timer, cancellation, and window. Record the results.

## Review

The new timer window follows the AuraFlow layout. It uses the October Aurora palette, wordmark, and three bundled typefaces.
The old app passed five of six tests. The new release passed all 11 tests.
The tests cover the tray, shutdown command failure, and one shutdown request at expiry.

The self-contained x64 build passed. The visual check used the rendered window.
The packaged app started from a clean folder. The tester started and canceled a timer, then closed the app. The PC stayed on.

## Open items

none

## GitHub release v1.0.0

- [x] Check the release settings and build the Windows x64 executable.
- [x] Set up the README, wiki, and related documentation on GitHub.
- [x] Publish the executable as a release asset. Download it without authentication and compare its SHA-256 hash with the local file.

## Release review

GitHub lists v1.0.0 as the latest public release. It includes the Windows x64 executable and SHA256SUMS.txt. All 11 tests passed.

The tester downloaded the executable without signing in. Its SHA-256 hash matched the local file, the GitHub asset digest, and the published checksum.

SHA-256: 8af6f6c7b81cc242d30fd15b50a25024485696268df8ad0bcd34a7b347ad99c8

The README and two guides are public. All four wiki pages returned HTTP 200.

## Open items

none
