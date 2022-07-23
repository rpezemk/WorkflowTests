namespace ProcessDocument.Model
{
    /// <summary>
    /// Rejestr PH
    /// </summary>
    public class RejestrPH
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Rejestr
        /// </summary>
        public string Rejestr { get; set; }
        /// <summary>
        /// Próg wartościowy MIN
        /// </summary>
        public decimal Prog { get; set; }
        /// <summary>
        /// Próg wartościowy MAX
        /// </summary>
        public decimal ProgMax { get; set; }
    }
}
