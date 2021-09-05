namespace ShogunOptimizer.ArtifactSets
{
    public class MissingSet : ArtifactSet
    {
        private readonly string name;

        public MissingSet(string name)
        {
            this.name = name;
        }

        public override string ToString() => name + " (NYI)";
    }
}
