# Grounder scene visual refactor check

The new handlers reuse the existing placed-device rendering pattern and resource loader. The visual dictionary is necessary to map simulation removal IDs to GameObjects. The added test checks a concrete SpriteRenderer/texture and its cleanup, not only handler existence; its `LogAssert.Expect` accounts for Unity's EditMode-only `Destroy` diagnostic without suppressing any other error. No existing assertions were removed. Focused Unity test: `receipt:4faa155e-f02c-4d54-8062-99d4fda836a7`.
