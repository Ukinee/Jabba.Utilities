namespace Utilities.Extensions.ListExtensions
{
    public static partial class ListExtension
    {
        public static (IEnumerable<T> added, IEnumerable<T> removed) Diff<T>(this IEnumerable<T> sourceCollection, IEnumerable<T> changedCollection)
        {
            if (sourceCollection is not HashSet<T> sourceSet)
            {
                sourceSet = [..sourceCollection];
            }

            if (changedCollection is not HashSet<T> changedSet)
            {
                changedSet = [..changedCollection];
            }

            IEnumerable<T> removed = sourceSet.Except(changedSet);
            IEnumerable<T> added = changedSet.Except(sourceSet);

            return (added, removed);
        }
    }
}
