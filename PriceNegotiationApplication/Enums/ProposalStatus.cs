namespace PriceNegotiationApp.Enums
{
    /// <summary>
    /// Represents the status of a price proposal in the negotiation process.
    /// </summary>
    public enum ProposalStatus
    {
        /// <summary>
        /// The proposal is still pending and has not been accepted or rejected.
        /// </summary>
        Pending,

        /// <summary>
        /// The proposal has been accepted.
        /// </summary>
        Accepted,

        /// <summary>
        /// The proposal has been rejected.
        /// </summary>
        Rejected
    }
}