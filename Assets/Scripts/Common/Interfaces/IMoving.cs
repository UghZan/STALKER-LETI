namespace Common
{
    public interface IMoving
    {
        public float movingSpeed { get; }
        public float externalSpeedMultiplier { get; set; }
    }
}