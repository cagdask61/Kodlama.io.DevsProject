namespace Core.Persistence.Repositories
{
    public class CommonEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public CommonEntity()
        {

        }

        public CommonEntity(int id) : this()
        {
            Id = id;
        }
    }
    public class CommonEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public CommonEntity()
        {

        }

        public CommonEntity(TKey id) : this()
        {
            Id = id;
        }
    }
}
