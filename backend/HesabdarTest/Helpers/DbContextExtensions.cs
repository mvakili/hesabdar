
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Linq;
using System.Threading;

public static class DbContextExtensions
{
    public static void ResetValueGenerators(this DbContext context)
    {
        var cache = context.GetService<IValueGeneratorCache>();

        foreach (var keyProperty in context.Model.GetEntityTypes()
            .Select(e => e.FindPrimaryKey().Properties[0])
            .Where(p => p.ClrType == typeof(int)
                        && p.ValueGenerated == ValueGenerated.OnAdd))
        {

            var generator = cache.GetOrAdd(
                keyProperty,
                keyProperty.DeclaringEntityType,
                (p, e) => new ResettableValueGenerator());

            if (generator is ResettableValueGenerator resettableGenerator)
            {
                resettableGenerator.Reset();
            }
            else
            {

            }


        }
    }
}

public class ResettableValueGenerator : ValueGenerator<int>
{
    private int _current;

    public override bool GeneratesTemporaryValues => false;

    public override int Next(EntityEntry entry)
        => Interlocked.Increment(ref _current);

    public void Reset() => _current = 0;
}