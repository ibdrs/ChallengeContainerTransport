using ChallengeContainerTransport.Domain.Rules;
using ChallengeContainerTransport.Domain.Rules.Interfaces;
using ContainerChallenge.Algorithm;
using ContainerChallenge.Export;


Console.WriteLine("Container Challenge (Console)");


var request = new PlacementRequest
{
    Length = 10,
    Width = 4,

    NormalCount = 128,
    ValuableCount = 4,
    CoolableCount = 18,
    ValuableCoolableCount = 2,

    NormalWeightTons = 30,
    ValuableWeightTons = 30,
    CoolableWeightTons = 30,
    ValuableCoolableWeightTons = 30
};


var placementRules = new IPlacementRule[]
{
    new CoolableMustBeFrontRowRule(),
    new ValuableMustBeEdgeRowRule(),
    new NoContainerAboveValuableRule(),
    new MaxWeightAboveRule(),
};

var shipRules = new IShipRule[]
{
    new BalanceRule(),
    new MinimumLoadRule(), // off when debugging
};

var placementValidator = new PlacementValidator(placementRules);
var shipValidator = new ShipValidator(shipRules);

var algorithm = new MyAlgorithm(placementValidator, shipValidator);

// RUN
var response = algorithm.Run(request);
var ship = response.Ship;

// console output + debug
Console.WriteLine($"Unplaced containers: {response.UnplacedCount}");

foreach (var u in response.Unplaced)
{
    Console.WriteLine($"- Id {u.Id} ({u.Type}, {u.WeightTons}t)");
    foreach (var reason in u.Reasons)
        Console.WriteLine($"  * {reason}");
}

if (response.Errors?.FirstOrDefault().Length > 0)
{
    Console.WriteLine("Ship validation errors:");
    foreach (var error in response.Errors)
        Console.WriteLine($"- {error}");
}

Console.WriteLine("\r\n");

var exporter = new VisualizerExporter();
var url = exporter.ToUrl(ship);

Console.WriteLine("Visualizer URL:");
Console.WriteLine(url);
