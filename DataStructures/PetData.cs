namespace PET.DataStructures
{
    public abstract class PetData
    {
        public readonly string name;

        public abstract int GetItem();

        public abstract int GetBuff();

        //public abstract string Save();

        public PetData(string name)
        {
            this.name = name;
        }
    }
}