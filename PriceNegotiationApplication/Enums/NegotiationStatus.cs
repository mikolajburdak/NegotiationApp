namespace PriceNegotiationApp.Enums
{
    /// <summary>
    /// Represents the status of a negotiation in the process.
    /// </summary>
    public enum NegotiationStatus
    {
        /// <summary>
        /// The negotiation is still pending and has not been resolved.
        /// </summary>
        Pending,

        /// <summary>
        /// The negotiation has been approved and is completed.
        /// </summary>
        Approved,

        /// <summary>
        /// The negotiation has been rejected.
        /// </summary>
        Rejected,

        /// <summary>
        /// The negotiation has been cancelled due to inactivity or other reasons.
        /// </summary>
        Cancelled
    }
}