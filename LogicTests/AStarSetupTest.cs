using PathfindingModule;

namespace LogicTests
{
    public class AStarSetupTest
    {
        [Fact]
        public void Test1()
        {
            var x = 5;
            var y = 5;
            var unit = 16;
            var astarInstance = AStarSetup.CreateInstance(x, y, unit);
            Assert.Equal(8, astarInstance.MapUnitSize);
        }
    }
}