namespace Karin.Network
{
    public interface IFlushable<T>
    {
        public void Push(T item);
        public void Flush();
    }
}
