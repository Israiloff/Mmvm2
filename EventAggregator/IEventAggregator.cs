using System;

namespace Israiloff.Cashbox.Component.Event.Aggregator
{
    /// <summary>
    /// Functionality interface for an Event Aggregator
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Send a message 
        /// </summary>
        /// <remarks>
        /// Sends the message and waits for processing on the UI thread before returning.
        /// </remarks>
        /// <typeparam name="TMessage">Type of the message</typeparam>
        /// <param name="message">Message to send</param>
        void SendMessage<TMessage>(TMessage message);

        /// <summary>
        /// Post a message 
        /// </summary>
        /// Posts the message for later processing on the UI thread, returning immediately.
        /// <typeparam name="TMessage">Type of the message</typeparam>
        /// <param name="message">Message to send</param>
        void PostMessage<TMessage>(TMessage message);

        /// <summary>
        /// Register a message handler
        /// </summary>
        /// <param name="eventHandler">Message handler to add.</param>
        /// <returns>Registered delegate</returns>
        Action<T> RegisterHandler<T>(Action<T> eventHandler);

        /// <summary>
        /// Unregister a message handler
        /// </summary>
        /// <param name="eventHandler">Message handler to remove.</param>
        void UnregisterHandler<T>(Action<T> eventHandler);
    }
}