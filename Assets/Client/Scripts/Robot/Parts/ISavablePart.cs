using Client.Scripts.Service.Model;

namespace Client.Scripts.Robot.Parts
{
    /// <summary>
    /// Part which can be serialized and deserialized
    /// </summary>
    public interface ISavablePart
    {
        /// <summary>
        /// Initialize part with parameters
        /// </summary>
        /// <param name="data">Part data</param>
        void Deserialize(PartData data);

        /// <summary>
        /// Serialization of part
        /// </summary>
        /// <returns>Data of part</returns>
        PartData Serialize();
    }
}