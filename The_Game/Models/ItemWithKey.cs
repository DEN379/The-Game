namespace The_Game.Models
{
    public class ItemWithKey<T>
    {
        public int Id { get; set; }
        public T Item { get; set; }
    }
}