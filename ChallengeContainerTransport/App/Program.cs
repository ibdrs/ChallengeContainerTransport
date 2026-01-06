using ContainerChallenge.Algorithm;
using ContainerChallenge.Export;
using ContainerChallenge.Validation;

Console.WriteLine("Container Challenge (Console)");

// TODO: read inputs properly; for now hardcode example
var request = new PlacementRequest
{
    Length = 3,
    Width = 3,
    NormalCount = 10,
    ValuableCount = 2,
    CoolableCount = 2,
    ValuableCoolableCount = 1,
    NormalWeightTons = 30,
    ValuableWeightTons = 30,
    CoolableWeightTons = 30,
    ValuableCoolableWeightTons = 30
};

var validator = new ConstraintsValidator(maxWeightAboveTons: 120, maxStackHeight: 8);
var algorithm = new PlacementAlgorithm(validator);

var ship = algorithm.GenerateLayout(request);

var exporter = new VisualizerExporter();
var url = exporter.ToUrl(ship);

Console.WriteLine("Visualizer URL:");
Console.WriteLine(url);
