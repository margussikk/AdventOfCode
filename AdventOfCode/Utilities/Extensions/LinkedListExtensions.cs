namespace AdventOfCode.Utilities.Extensions;
internal static class LinkedListExtensions
{
    public static LinkedListNode<T> NextCircular<T>(this LinkedListNode<T> node, int distance = 1)
    {
        for (var i = 0; i < distance; i++)
        {
            node = node.Next ?? node.List?.First ?? throw new InvalidOperationException("Couldn't get the next item in the circular linked list");
        }

        return node;
    }

    public static LinkedListNode<T> PreviousCircular<T>(this LinkedListNode<T> node, int distance = 1)
    {
        for (var i = 0; i < distance; i++)
        {
            node = node.Previous ?? node.List?.Last ?? throw new InvalidOperationException("Couldn't get the previous item in the circular linked list");
        }

        return node;
    }
}
