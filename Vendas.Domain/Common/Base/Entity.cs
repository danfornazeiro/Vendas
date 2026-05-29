namespace Vendas.Domain.Common.Base
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
        public DateTime? DataAtualizacao { get; protected set; }

        protected Entity() 
        { 
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;            
        }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected void SetDataAtualizacao()
        {
            DataAtualizacao = DateTime.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other) return false;
            if(ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        { 
            if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);

            return left.Equals(right); 
        }

        public static bool operator !=(Entity left, Entity right)
        {          
            return !(left == right);
        }
    }
}
