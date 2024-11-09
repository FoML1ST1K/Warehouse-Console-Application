using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarehouseTests {
    [TestClass]
    public class WarehouseTests {
        [TestMethod]
        public void Box_ProductionDateSetsExpiryDate() {
            // Arrange
            DateTime productionDate = new DateTime(2023, 1, 1);
            var box = new Box("B1", 10, 10, 10, 5, productionDate: productionDate);

            // Act
            DateTime? expectedExpiryDate = productionDate.AddDays(100);

            // Assert
            Assert.AreEqual(expectedExpiryDate, box.ExpiryDate);
        }

        [TestMethod]
        public void Pallet_AddBox_IncreasesWeight() {
            // Arrange
            var pallet = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5);
            var box2 = new Box("B2", 30, 20, 10, 7);

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);
            double expectedWeight = 30 + box1.Weight + box2.Weight; // Base weight + weights of boxes

            // Assert
            Assert.AreEqual(expectedWeight, pallet.Weight);
        }

        [TestMethod]
        public void Pallet_CalculateTotalVolume_ReturnsCorrectVolume() {
            // Arrange
            var pallet = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5);
            var box2 = new Box("B2", 30, 20, 10, 7);
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            // Act
            double expectedVolume = pallet.CalculateVolume() + box1.CalculateVolume() + box2.CalculateVolume();
            double actualVolume = pallet.CalculateTotalVolume();

            // Assert
            Assert.AreEqual(expectedVolume, actualVolume);
        }

        [TestMethod]
        public void Pallet_AddBox_ThrowsExceptionWhenBoxExceedsPalletSize() {
            // Arrange
            var pallet = new Pallet("P1", 50, 50, 50);
            var oversizedBox = new Box("B1", 60, 20, 60, 5);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => pallet.AddBox(oversizedBox));
        }

        [TestMethod]
        public void Pallet_ExpiryDate_IsMinimumExpiryOfBoxes() {
            // Arrange
            var pallet = new Pallet("P1", 120, 100, 15);
            var box1 = new Box("B1", 30, 20, 10, 5, expiryDate: new DateTime(2024, 1, 1));
            var box2 = new Box("B2", 30, 20, 10, 7, expiryDate: new DateTime(2023, 12, 15));
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            // Act
            DateTime? expectedExpiryDate = new DateTime(2023, 12, 15);

            // Assert
            Assert.AreEqual(expectedExpiryDate, pallet.ExpiryDate);
        }
    }
}
