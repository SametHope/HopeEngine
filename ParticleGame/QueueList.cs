using System.Collections;

namespace HopeEngine;

/// <summary>
/// Represents a custom data structure that combines the characteristics of a list and a queue.
/// Allows enqueuing additions and removals, which are processed later to modify the main list.
/// <see cref="IEnumerable{T}"/> interface returns the enumarator for the main list.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class QueueList<T> : IEnumerable<T>
{
    private readonly List<T> _mainList = new List<T>();
    private readonly Queue<T> _pendingRemovals = new Queue<T>();
    private readonly Queue<T> _pendingAdditions = new Queue<T>();

    /// <summary>
    /// Gets the main list of elements.
    /// </summary>
    public IReadOnlyList<T> MainList => _mainList;

    /// <summary>
    /// Gets the collection of elements pending removal.
    /// </summary>
    public IReadOnlyCollection<T> PendingRemovals => _pendingRemovals.ToList();

    /// <summary>
    /// Gets the collection of elements pending addition.
    /// </summary>
    public IReadOnlyCollection<T> PendingAdditions => _pendingAdditions.ToList();

    /// <summary>
    /// Enqueues an item for removal.
    /// </summary>
    /// <param name="item">The item to be removed.</param>
    public void EnqueueRemoval(T item)
    {
        _pendingRemovals.Enqueue(item);
    }

    /// <summary>
    /// Enqueues an item for addition.
    /// </summary>
    /// <param name="item">The item to be added.</param>
    public void EnqueueAddition(T item)
    {
        _pendingAdditions.Enqueue(item);
    }

    /// <summary>
    /// Enqueues a collection of items for removal.
    /// </summary>
    /// <param name="items">The items to be removed.</param>
    public void EnqueueRemovalRange(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            _pendingRemovals.Enqueue(item);
        }
    }

    /// <summary>
    /// Enqueues a collection of items for addition.
    /// </summary>
    /// <param name="items">The items to be added.</param>
    public void EnqueueAdditionRange(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            _pendingAdditions.Enqueue(item);
        }
    }

    /// <summary>
    /// Processes pending removals by removing items from the main list.
    /// </summary>
    /// <param name="count">The number of removals to process.</param>
    public void ProcessPendingRemovals(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T itemToRemove = _pendingRemovals.Dequeue();
            _mainList.Remove(itemToRemove);
        }
    }

    /// <summary>
    /// Processes all pending removals by removing items from the main list.
    /// </summary>
    public void ProcessPendingRemovals()
    {
        ProcessPendingRemovals(_pendingRemovals.Count);
    }

    /// <summary>
    /// Processes pending additions by adding items to the main list.
    /// </summary>
    /// <param name="count">The number of additions to process.</param>
    public void ProcessPendingAdditions(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T itemToAdd = _pendingAdditions.Dequeue();
            _mainList.Add(itemToAdd);
        }
    }

    /// <summary>
    /// Processes all pending additions by adding items to the main list.
    /// </summary>
    public void ProcessPendingAdditions()
    {
        ProcessPendingAdditions(_pendingAdditions.Count);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        return _mainList.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
