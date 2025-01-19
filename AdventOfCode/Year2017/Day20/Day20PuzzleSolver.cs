using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2017.Day20;

[Puzzle(2017, 20, "Particle Swarm")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private List<Particle> _particles = [];

    public void ParseInput(string[] inputLines)
    {
        _particles = inputLines.Select(Particle.Parse)
                               .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        Particle closestParticle;

        var particles = _particles.Select(p => p.Clone()).ToList();
        var particlesByAccelerations = particles.ToLookup(p => ManhattanDistance(p.Acceleration));

        var smallestAccelerationParticles = particlesByAccelerations[particlesByAccelerations.Min(x => x.Key)].ToList();
        if (smallestAccelerationParticles.Count == 1)
        {
            closestParticle = smallestAccelerationParticles[0];
        }
        else
        {
            while (!smallestAccelerationParticles.All(p => p.IsVelocityIncreasing()))
            {
                foreach (var particle in smallestAccelerationParticles)
                {
                    particle.Tick();
                }
            }

            var particlesByVelocities = smallestAccelerationParticles.ToLookup(p => ManhattanDistance(p.Velocity));

            var smallestVelocityParticles = particlesByVelocities[particlesByVelocities.Min(x => x.Key)].ToList();
            if (smallestVelocityParticles.Count == 1)
            {
                closestParticle = smallestVelocityParticles[0];
            }
            else
            {
                closestParticle = smallestVelocityParticles.OrderBy(p => p.Position.ManhattanDistanceTo(Coordinate3D.Zero))
                                                           .First();
            }
        }

        var answer = particles.IndexOf(closestParticle);

        return new PuzzleAnswer(answer, 119);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var particles = _particles.Select(p => p.Clone()).ToHashSet();

        for (var i = 0; i < 100; i++)
        {
            var collidedParticles = particles.ToLookup(p => p.Position)
                                             .Where(x => x.Count() > 1)
                                             .SelectMany(x => x.ToList());

            particles.ExceptWith(collidedParticles);

            foreach (var particle in particles)
            {
                particle.Tick();
            }
        }

        return new PuzzleAnswer(particles.Count, 471);
    }

    private static long ManhattanDistance(Vector3D vector)
    {
        return Math.Abs(vector.DX) + Math.Abs(vector.DY) + Math.Abs(vector.DZ);
    }
}