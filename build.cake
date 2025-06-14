var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");



Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

Task("Default")
.Does(() => {
	Information("Hello World!");
});

RunTarget(target);
