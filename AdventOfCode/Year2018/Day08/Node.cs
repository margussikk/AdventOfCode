namespace AdventOfCode.Year2018.Day08
{
    internal class Node
    {
        public List<Node> Children { get; } = [];
        public List<int> MetadataEntries { get; } = [];

        public Node(Queue<int> queue)
        {
            var childrenCount = queue.Dequeue();
            var metadataEntriesCount = queue.Dequeue();

            for (int i = 0; i < childrenCount; i++)
            {
                Children.Add(new Node(queue));
            }

            for (int i = 0; i < metadataEntriesCount; i++)
            {
                MetadataEntries.Add(queue.Dequeue());
            }
        }

        public int SumOfMetadataEntries()
        {
            return Children.Sum(x => x.SumOfMetadataEntries()) +
                   MetadataEntries.Sum();
        }

        public int GetValue()
        {
            if (Children.Count == 0)
            {
                return MetadataEntries.Sum();
            }

            return MetadataEntries
                .Where(x => x <= Children.Count)
                .ToLookup(x => x)
                .Sum(x => x.Count() * Children[x.Key - 1].GetValue());
        }
    }
}
