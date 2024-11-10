using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WarehouseConsoleTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void AddBox_WhenOneBoxAdded_BoxesCountEqualOne() {
            // Arrange
            var expectedBoxesCount = 1;
            var pallet = new Pallet("", 0, 0, 0, 0);
            var box = new Box("", 0, 0, 0, 0, DateTime.Now);
            // Act
            pallet.AddBox(box);
            var actualBoxesCount = pallet.Boxes.Count;
            // Assert
            Assert.AreEqual(actualBoxesCount, expectedBoxesCount);
        }
    }
}
