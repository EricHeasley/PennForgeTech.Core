using System;

namespace PennForgeTech.Core.Domain
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
        where TId : notnull
    {
        protected Entity(TId id)
        {
            this.Id = id;
        }

        public TId Id { get; }

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

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Entity<TId>);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.GetType(), this.Id);
        }
    }
}
