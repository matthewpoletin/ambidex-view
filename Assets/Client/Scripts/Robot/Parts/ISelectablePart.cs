namespace Client.Scripts.Robot.Parts
{
    /// <summary>
    /// Selection and deselection actions on part
    /// </summary>
    public interface ISelectablePart
    {
        void Select();
        void Deselect();
    }
}