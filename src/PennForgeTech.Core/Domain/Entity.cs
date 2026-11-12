using System;

namespace PennForgeTech.Core.Domain;

/// <summary>Base class for domain objects that are identified by an id rather than by their property values.</summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    /// <summary>Initializes a new instance of the <see cref="Entity{TId}"/> class.</summary>
    /// <param name="id">The unique identifier of the entity.</param>
    protected Entity(TId id)
    {
        this.Id = id;
    }

    /// <summary>Gets the unique identifier of the entity.</summary>
    public TId Id { get; }

    /// <inheritdoc />
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.GetType() == other.GetType() && this.Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Entity<TId>);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(this.GetType(), this.Id);
    }
}
