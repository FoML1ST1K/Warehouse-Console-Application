using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseConsoleTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void AddThreePallets_WithThreeBoxesEach_BoxesCountCorrectAndExpiryDateSet() {
            // Arrange
            var pallet1 = new Pallet("P1", 100, 100, 100);
            var box1_1 = new Box("B1_1", 10, 10, 10, 5, expiryDate: new DateTime(2025, 1, 10));
            var box1_2 = new Box("B1_2", 10, 10, 10, 6, expiryDate: new DateTime(2025, 1, 20));
            var box1_3 = new Box("B1_3", 10, 10, 10, 7, expiryDate: new DateTime(2025, 2, 1));

            var pallet2 = new Pallet("P2", 100, 100, 100);
            var box2_1 = new Box("B2_1", 10, 10, 10, 8, expiryDate: new DateTime(2024, 12, 15));
            var box2_2 = new Box("B2_2", 10, 10, 10, 9, expiryDate: new DateTime(2025, 3, 5));
            var box2_3 = new Box("B2_3", 10, 10, 10, 10, expiryDate: new DateTime(2024, 11, 20));

            var pallet3 = new Pallet("P3", 100, 100, 100);
            var box3_1 = new Box("B3_1", 10, 10, 10, 12, expiryDate: new DateTime(2024, 5, 25));
            var box3_2 = new Box("B3_2", 10, 10, 10, 13, expiryDate: new DateTime(2024, 8, 10));
            var box3_3 = new Box("B3_3", 10, 10, 10, 14, expiryDate: new DateTime(2024, 9, 5));

            // Act
            pallet1.AddBox(box1_1);
            pallet1.AddBox(box1_2);
            pallet1.AddBox(box1_3);

            pallet2.AddBox(box2_1);
            pallet2.AddBox(box2_2);
            pallet2.AddBox(box2_3);

            pallet3.AddBox(box3_1);
            pallet3.AddBox(box3_2);
            pallet3.AddBox(box3_3);

            var actualBoxesCountPallet1 = pallet1.Boxes.Count;
            var actualBoxesCountPallet2 = pallet2.Boxes.Count;
            var actualBoxesCountPallet3 = pallet3.Boxes.Count;

            // Assert
            Assert.AreEqual(3, actualBoxesCountPallet1);
            Assert.AreEqual(3, actualBoxesCountPallet2);
            Assert.AreEqual(3, actualBoxesCountPallet3);

            Assert.AreEqual(new DateTime(2025, 1, 10), pallet1.ExpiryDate);
            Assert.AreEqual(new DateTime(2024, 11, 20), pallet2.ExpiryDate);
            Assert.AreEqual(new DateTime(2024, 5, 25), pallet3.ExpiryDate);
        }
        [TestMethod]
        public void Pallets_GroupedByExpiryDate_CorrectGrouping() {
            // Arrange
            var pallet1 = new Pallet("P1", 120, 100, 100);
            var box1 = new Box("B1", 10, 10, 10, 5, expiryDate: new DateTime(2024, 12, 31));
            var pallet2 = new Pallet("P2", 120, 100, 100);
            var box2 = new Box("B2", 10, 10, 10, 5, expiryDate: new DateTime(2025, 1, 15));

            pallet1.AddBox(box1);
            pallet2.AddBox(box2);

            var pallets = new List<Pallet> { pallet1, pallet2 };

            // Act
            var groupedPallets = pallets.GroupBy(p => p.ExpiryDate);

            // Assert
            Assert.AreEqual(2, groupedPallets.Count()); 
        }
        [TestMethod]
        public void Pallet_AddBox_ExceedsPalletSize_ThrowsException() {
            // Arrange
            var pallet = new Pallet("P1", 50, 50, 50);
            var oversizedBox = new Box("B1", 60, 60, 60, 5, expiryDate: new DateTime(2025, 1, 15));

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => pallet.AddBox(oversizedBox));
        }
        [TestMethod]
        public void Pallet_AddBox_DuplicateId_DoesNotThrowException() {
            // Arrange
            var pallet = new Pallet("P1", 120, 100, 100);
            var box1 = new Box("B1", 10, 10, 10, 5, expiryDate: new DateTime(2024, 5, 25));
            var box2 = new Box("B1", 10, 10, 10, 5, expiryDate: new DateTime(2024, 11, 14)); 

            // Act
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            // Assert
            Assert.AreEqual(2, pallet.Boxes.Count);
        }

    }
}
