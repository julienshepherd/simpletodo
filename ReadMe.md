# How to build and deploy

1. Have Visual Studio 2022 with Universal Windows Platform workload installed.
2. Open the SimpleToDo.sln file in Visual Studio.
3. Hit F5, or Debug > Start debugging.

# Rationale

* Minimal app with few third-party packages installed.
* It uses StyleCop to enforce code style consistency and documentation reminders.
* It relies on editorconfig to ignore warnings from generated code files (e.g. *.g.cs) and tell Visual Studio to show suggestions consistent with StyleCop.
* Realm is the data storage solution, while it causes a strong link between the UI layer and data layer, its API is a quick win for the goal of this app.
